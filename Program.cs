
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace GetBandsAlbums2xml
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool success = false;
            try
            {
                success = GetBandsAlbums();
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }

        }

        static bool GetBandsAlbums ()
        {
            string rootpath = ConfigurationManager.AppSettings["RootPath"].ToString();

            string outputfile = ConfigurationManager.AppSettings["ExportPath"].ToString();

            FileInfo fi = new FileInfo(outputfile);
            if (fi.Exists)
                fi.Delete();

            DirectoryInfo rootdir = new DirectoryInfo(rootpath);

            XmlTextWriter wri = new XmlTextWriter(outputfile, null);
            wri.WriteStartDocument();
            wri.WriteStartElement("music");



            DirectoryInfo[] bandlist = rootdir.GetDirectories();

            foreach (DirectoryInfo band in bandlist)
            {
                DirectoryInfo[] albumlist = band.GetDirectories();

                wri.WriteStartElement("band");
                wri.WriteStartElement("name");
                wri.WriteCData(band.Name);
                wri.WriteEndElement();
                wri.WriteStartElement("albums");
                foreach (DirectoryInfo album in albumlist)
                {
                    wri.WriteStartElement("album");
                    wri.WriteCData(album.Name);
                    wri.WriteEndElement();

                }
                wri.WriteEndElement();
                wri.WriteEndElement();

            }

            wri.WriteEndDocument();
            wri.Close();

            return true;


        }
    }
}
