using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace BluffingoUpdater
{
    public class WebRequest
    {
        WebClient client = new WebClient();
        //string domain = "http://localhost";
        string domain = "http://10.0.0.178";
        string filename = "";
        public DateTime timeMachineDate = DateTime.Now;

        public WebRequest()
        {
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }

        public void DownloadAndRunInstaller(string url)
        {
            Uri uri = new Uri(url);
            filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp/example.exe");

            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                client.DownloadFileCompleted += WebClientDownloadCompleted;
                client.DownloadFileAsync(uri, filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public string GetVersions()
        {
            Stream data = client.OpenRead(domain + "/api/get_versions/" + timeMachineDate.ToString("yyyy-MM-dd"));
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return s;
        }

        public string GetSoftware()
        {
            Stream data = client.OpenRead(domain + "/api/get_software");
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return s;
        }

        private void WebClientDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(filename);
        }
    }
}
