namespace PhotoAlbum
{
    using System;
    using System.IO;
    using System.Web.Configuration;

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
        }

        protected void Application_End(object sender, EventArgs e)
        {
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