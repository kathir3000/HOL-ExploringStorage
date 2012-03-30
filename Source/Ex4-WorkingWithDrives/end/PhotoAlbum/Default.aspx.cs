namespace PhotoAlbum
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.UI.WebControls;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.StorageClient;

    public partial class _Default : System.Web.UI.Page
    {
        protected string CurrentPath
        {
            get
            {
                string path = Request.Params["path"];
                if (String.IsNullOrEmpty(path))
                {
                    return Global.ImageStorePath;
                }

                return path;
            }
        }

        protected void LinqDataSource1_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
        {
            e.ObjectInstance = new PhotoAlbumDataSource(this.CurrentPath);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            GridView1.Columns[GridView1.Columns.Count - 1].Visible = (this.CurrentPath != Global.ImageStorePath);

            if (RoleEnvironment.IsAvailable)
            {
                MountedDrives.DataSource = from item in CloudDrive.GetMountedDrives()
                                           select new
                                           {
                                               Name = item.Key + " => " + item.Value,
                                               Value = item.Key
                                           };

                MountedDrives.DataBind();
                MountedDrives.SelectedValue = this.CurrentPath;
                SelectDrive.Visible = true;

                NewDrive.Text = MountedDrives.Items.Count < 2 ? "New Drive" : "Delete Drive";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var index = Convert.ToInt32(e.CommandArgument);
                string fileName = ((GridView)e.CommandSource).DataKeys[index].Value as string;
                File.Delete(fileName);
                SelectImageStore(CurrentPath);
            }
        }

        protected void NewDrive_Click(object sender, EventArgs e)
        {
            if (RoleEnvironment.IsAvailable)
            {
                // retrieve storage account
                CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

                // build page blob URI for the new cloud drive by changing the extension in the original URI
                string imageStoreBlobUri = RoleEnvironment.GetConfigurationSettingValue("ImageStoreBlobUri");
                string cloneStoreBlobUri = Path.ChangeExtension(imageStoreBlobUri, "bak");

                // create drive and its associated page blob
                CloudDrive clonedDrive = account.CreateCloudDrive(cloneStoreBlobUri);
                if (MountedDrives.Items.Count < 2)
                {
                    try
                    {
                        clonedDrive.Create(16);
                    }
                    catch (CloudDriveException)
                    {
                        // cloud drive already exists
                    }

                    // mount the drive and retrieve its path
                    LocalResource cache = RoleEnvironment.GetLocalResource("LocalDriveCache");
                    string clonedStorePath = clonedDrive.Mount(cache.MaximumSizeInMegabytes / 2, DriveMountOptions.None);

                    // copy the contents from the original drive to the new drive                
                    foreach (string sourceFileName in Directory.GetFiles(Global.ImageStorePath, "*.*").Where(name => name.EndsWith(".jpg") || name.EndsWith(".png")))
                    {
                        string destinationFileName = Path.Combine(clonedStorePath, Path.GetFileName(sourceFileName));
                        File.Copy(sourceFileName, destinationFileName, true);
                    }

                    SelectImageStore(clonedStorePath);
                }
                else
                {
                    clonedDrive.Unmount();
                    clonedDrive.Delete();
                    SelectImageStore(Global.ImageStorePath);
                }
            }
        }

        protected void MountedDrives_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectImageStore(MountedDrives.SelectedValue);
        }

        private void SelectImageStore(string path)
        {
            Response.Redirect(this.Request.Path + "?path=" + path);
        }
    }
}