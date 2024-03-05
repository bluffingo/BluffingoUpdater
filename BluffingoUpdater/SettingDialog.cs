using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BluffingoUpdater
{
    public partial class SettingDialog : Form
    {
        WebRequest request = new WebRequest();
        JavaScriptSerializer ser = new JavaScriptSerializer();

        public SettingDialog()
        {
            InitializeComponent();
        }

        private void Clear()
        {
            listView1.Clear();
            imageList1.Images.Clear();
        }

        private void SettingDialog_Load(object sender, EventArgs e)
        {
            Font = SystemFonts.MessageBoxFont;
            Clear();
            string s = request.GetSoftware();

            var obj = ser.DeserializeObject(s) as ICollection;

            foreach (KeyValuePair<string, object> item in obj)
            {
                var id = item.Key;
                var fuckYouVisualStudio = item.Value;

                var appName = "Application";
                var appAuthor = "Company";

                foreach (KeyValuePair<string, object> item2 in (IEnumerable)fuckYouVisualStudio)
                {
                    var id2 = item2.Key;
                    Console.WriteLine(id2);
                    Console.WriteLine(item2.Value);
                    // oh god this seems shit
                    if (id2 == "name")
                    {
                        appName = (string)item2.Value;
                    }
                    if (id2 == "author")
                    {
                        appAuthor = (string)item2.Value;
                    }
                }
                ListViewItem item1 = new ListViewItem(appName, 1);
                item1.SubItems.Add(appAuthor);

                listView1.Items.Add(item1);
            }
            MakeTheColumns();
        }

        private void MakeTheColumns()
        {
            //imageList1.Images.Add("GenericIcon", BluffingoUpdater.Properties.Resources.icon);
            imageList1.Images.Add("GenericIcon", WindowsStuff.ExtractIcon("shell32.dll", 162, true));
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].ImageIndex = 0;
            }

            listView1.Columns.Add("Application", 170, HorizontalAlignment.Left);
            listView1.Columns.Add("Author", 150, HorizontalAlignment.Left);
            listView1.LargeImageList = imageList1;
        }
    }
}
