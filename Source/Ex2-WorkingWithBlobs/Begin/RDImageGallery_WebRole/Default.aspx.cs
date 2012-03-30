namespace RDImageGallery_WebRole
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Linq;
    using System.Web.UI.WebControls;

    public partial class _Default : System.Web.UI.Page
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void upload_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Cast out blob instance and bind it's metadata to metadata repeater
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBlobDataBound(object sender, ListViewItemEventArgs e)
        {
        }

        /// <summary>
        /// Delete an image blob by Uri
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnDeleteImage(object sender, CommandEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnCopyImage(object sender, CommandEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnSnapshotImage(object sender, CommandEventArgs e)
        {
        }
    }
}