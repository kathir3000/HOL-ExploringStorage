namespace PhotoAlbum
{
    using System;
    using System.IO;
    using System.Web.Configuration;
    using System.Diagnostics;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.WindowsAzure.StorageClient;

    public class Global : System.Web.HttpApplication
    {
        private static string imageStorePath;

        // Application_Start is called after the OnStart method.
        protected void Application_Start(object sender, EventArgs e)
        {
            if (imageStorePath == null)
            {
                ImageStorePath = WebConfigurationManager.AppSettings["ImageStorePath"];
            }

            // initialize storage account configuration setting publisher
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                string connectionString = RoleEnvironment.GetConfigurationSettingValue(configName);
                configSetter(connectionString);
            });

            try
            {
                // initialize the local cache for the Azure drive
                LocalResource cache = RoleEnvironment.GetLocalResource("LocalDriveCache");
                CloudDrive.InitializeCache(cache.RootPath + "cache", cache.MaximumSizeInMegabytes);

                // retrieve storage account 
                CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");

                // retrieve URI for the page blob that contains the cloud drive from configuration settings 
                string imageStoreBlobUri = RoleEnvironment.GetConfigurationSettingValue("ImageStoreBlobUri");

                // unmount any previously mounted drive.
                foreach (var drive in CloudDrive.GetMountedDrives())
                {
                    var mountedDrive = account.CreateCloudDrive(drive.Value.PathAndQuery);
                    mountedDrive.Unmount();
                }

                // create the Windows Azure drive and its associated page blob
                CloudDrive imageStoreDrive = account.CreateCloudDrive(imageStoreBlobUri);
                try
                {
                    imageStoreDrive.Create(16);
                }
                catch (CloudDriveException)
                {
                    // drive already exists
                }

                // mount the drive and initialize the application with the path to the image store on the Azure drive
                Global.ImageStorePath = imageStoreDrive.Mount(cache.MaximumSizeInMegabytes / 2, DriveMountOptions.None);
            }
            catch (CloudDriveException driveException)
            {
                Trace.WriteLine("Error: " + driveException.Message);
            }

        }

        protected void Application_End(object sender, EventArgs e)
        {
            // obtain a reference to the cloud drive and unmount it
            CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            string imageStoreBlobUri = RoleEnvironment.GetConfigurationSettingValue("ImageStoreBlobUri");
            CloudDrive imageStoreDrive = account.CreateCloudDrive(imageStoreBlobUri);
            imageStoreDrive.Unmount();
        }

        public static string ImageStorePath
        {
            get
            {
                return imageStorePath;
            }

            set
            {
                imageStorePath = value;
            }
        }
    }
}