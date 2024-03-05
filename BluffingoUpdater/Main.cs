using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace BluffingoUpdater
{
    public partial class Main : Form
    {
        SettingDialog settingDialog = new SettingDialog();
        TimeMachineDialog timeMachineDialog = new TimeMachineDialog();
        WebRequest request = new WebRequest();
        JavaScriptSerializer ser = new JavaScriptSerializer();
        bool isDownloadedList = false;
        string url;

        public Main()
        {
            InitializeComponent();
            settingDialog.Visible = false;
            timeMachineDialog.Visible = false;

            request.ProgressChanged += SetProgress;
        }

        private void updateSoftwareList()
        {
            clear();

            string s = request.GetVersions();

            var obj = ser.DeserializeObject(s) as ICollection;

            foreach (KeyValuePair<string, object> item in obj)
            {
                var id = item.Key;
                var fuckYouVisualStudio = item.Value;

                var appName = "Application";
                var appVersion = "0.0.0-dummy";
                var appDownload = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
                var appReleased = "Mayvember 32st, 5259";

                Console.WriteLine(id);
                foreach (KeyValuePair<string, object> item2 in (IEnumerable)fuckYouVisualStudio)
                {
                    var id2 = item2.Key;
                    Console.WriteLine(id2 + ": " + item2.Value);
                    // oh god this seems shit
                    if (id2 == "name")
                    {
                        appName = (string)item2.Value;
                    }
                    if (id2 == "version")
                    {
                        appVersion = (string)item2.Value;
                    }
                    if (id2 == "download")
                    {
                        appDownload = (string)item2.Value;
                    }
                    if (id2 == "released")
                    {
                        string released = (string)item2.Value;
                        var parsedDate = DateTime.Parse(released);
                        appReleased = parsedDate.ToString("MMMM dd yyyy");
                    }
                }
                ListViewItem item1 = new ListViewItem(appName, 1);
                item1.SubItems.Add(appVersion);
                item1.SubItems.Add(appReleased);
                item1.SubItems.Add(appDownload);

                listView1.Items.Add(item1);
            }

            makeTheColumns();

            isDownloadedList = true;
        }

        private void clear()
        {
            isDownloadedList = false;
            listView1.Clear();
            imageList1.Images.Clear();
        }

        private void makeTheColumns()
        {
            //imageList1.Images.Add("GenericIcon", BluffingoUpdater.Properties.Resources.icon);
            imageList1.Images.Add("GenericIcon", WindowsStuff.ExtractIcon("shell32.dll", 162, true));
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].ImageIndex = 0;
            }

            listView1.Columns.Add("Application", 170, HorizontalAlignment.Left);
            listView1.Columns.Add("Version", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("Released", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("URL", 200, HorizontalAlignment.Left);
            listView1.SmallImageList = imageList1;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(url))
            {
                request.DownloadAndRunInstaller(url);
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDownloadedList)
            {
                ListView.SelectedListViewItemCollection selected = this.listView1.SelectedItems;
                foreach (ListViewItem item in selected)
                {
                    url = item.SubItems[3].Text;
                }
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            Font = SystemFonts.MessageBoxFont;
            createMenu();
            updateSoftwareList();

#if !DEBUG
            DateTime d1 = new DateTime(2016, 12, 31);
            if (d1 < DateTime.Now)
            {
                MessageBox.Show("Bluffingo's Updater is not intended to be used with the current date.", "Bluffingo's Updater", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                OpenTimeTravelDialog();
            }
#endif
        }

        private void createMenu()
        {
            MainMenu mainMenu = new MainMenu();

            MenuItem propertiesMenu = new MenuItem("Properties");

            MenuItem chooseSoftwareMenu = new MenuItem("Choose software", new EventHandler(chooseSoftwareMenuClicked));
            MenuItem timeTravelMenu = new MenuItem("Time travel", new EventHandler(TimeTravelMenuClicked));

            propertiesMenu.MenuItems.Add(chooseSoftwareMenu);
            propertiesMenu.MenuItems.Add(timeTravelMenu);

            mainMenu.MenuItems.Add(propertiesMenu);

            MenuItem helpMenu = new MenuItem("Help");
            MenuItem aboutMenu = new MenuItem("About", new EventHandler(AboutMenuClicked));

            helpMenu.MenuItems.Add(aboutMenu);

            mainMenu.MenuItems.Add(helpMenu);

            Menu = mainMenu;
        }

        private void chooseSoftwareMenuClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Opening choose software dialog.");
            if (!settingDialog.Visible)
            {
                settingDialog.ShowDialog();
            }
        }

        private void TimeTravelMenuClicked(object sender, EventArgs e)
        {
            OpenTimeTravelDialog();
        }

        private void OpenTimeTravelDialog()
        {
            Console.WriteLine("Opening time machine dialog.");
            if (!timeMachineDialog.Visible)
            {
                DialogResult timeMachineResult = timeMachineDialog.ShowDialog();
                if (timeMachineResult == DialogResult.OK)
                {
                    request.timeMachineDate = timeMachineDialog.time;
                    updateSoftwareList();
                }
            }
        }

        private void AboutMenuClicked(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        public void SetProgress(int progress)
        {
            progressBar.Value = progress;
            // makes the progress bar move faster on vista+ since it has a
            // very slow animation
            if (progressBar.Value > 0)
            {
                progressBar.Value = progress - 1;
                progressBar.Value = progress;
            }
        }
    }
}
