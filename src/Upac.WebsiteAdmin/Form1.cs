namespace Upac.WebsiteAdmin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;

    using Application = Microsoft.Web.Administration.Application;

    public partial class Form1 : Form
    {
        #region Fields

        private ServerManager iisManager;

        #endregion Fields

        #region Constructors

        public Form1()
        {
            InitializeComponent();
            iisManager = new ServerManager();
            LoadProperties();
            LoadWebsites();
        }

        #endregion Constructors

        #region Methods

        private void buttonChooseDistFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Please select a folder.";
            dialog.SelectedPath = textBoxSourceDir.Text;
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSourceDir.Text = dialog.SelectedPath;
            }
        }

        private void buttonCreateWebsite_Click(object sender, EventArgs e)
        {
            if (textBoxNewWebsiteName.Text != string.Empty)
            {
                CreateWebsite(textBoxNewWebsiteName.Text);
                LoadWebsites();
            }
        }

        private void buttonDeleteWebsite_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection collection = listBoxWebsites.SelectedItems;
            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    Site selectedSite = (Site)item;
                    DeleteWebsite(selectedSite, checkBoxOnDeleteAlsoDeleteDb.Checked, checkBoxIncludeFiles.Checked);
                }
                iisManager.CommitChanges();
                LoadWebsites();
            }
        }

        private void buttonReinstallSite_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection collection = listBoxWebsites.SelectedItems;
            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    Site selectedSite = (Site)item;
                    string sitename = selectedSite.Name;
                    DeleteWebsite(selectedSite, checkBoxOnDeleteAlsoDeleteDb.Checked, checkBoxIncludeFiles.Checked);
                    iisManager.CommitChanges();
                    CreateWebsite(sitename);
                }
                LoadWebsites();
            }
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SourceDir = textBoxSourceDir.Text;
            Properties.Settings.Default.WebsitesDir = textBoxSettingsWebsitesDir.Text;
            Properties.Settings.Default.Save();
        }

        private void buttonSettingsChooseWebsitesDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Please select a folder.";
            dialog.SelectedPath = textBoxSettingsWebsitesDir.Text;
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxSettingsWebsitesDir.Text = dialog.SelectedPath;
            }
        }

        private void CreateWebsite(string sitename)
        {
            string hostheader = string.Concat(sitename, ".local");
            string rootPath = Path.Combine(Properties.Settings.Default.WebsitesDir, sitename);
            string webrootPath = Path.Combine(rootPath, "Website");
            string databasePath = Path.Combine(rootPath, "Database");

            Util.EnsureFolder(rootPath);
            Util.EnsureFolder(webrootPath);
            Util.EnsureFolder(databasePath);

            Console.WriteLine("CopyDirectory from: " + Properties.Settings.Default.SourceDir + " to: " + webrootPath);
            Util.CopyDirectory(Properties.Settings.Default.SourceDir, webrootPath);
            Console.WriteLine("CreateDatabase: " + sitename);
            Util.CreateDatabase(sitename, databasePath);
            Util.ModifyWebconfig("datalayer=SqlServer;server=.;database=" + sitename + ";user id=sa;password=password", Path.Combine(webrootPath, "web.config"));

            ApplicationPool applicationPool = iisManager.ApplicationPools.Add(sitename);
            applicationPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
            applicationPool.AutoStart = true;

            Site site = iisManager.Sites.Add(sitename, "http", "*:80:" + hostheader, webrootPath);
            site.Applications[0].ApplicationPoolName = applicationPool.Name;

            iisManager.CommitChanges();
        }

        private void DeleteWebsite(Site site, bool checkBoxOnDeleteAlsoDeleteDb, bool checkBoxIncludeFiles)
        {
            string sitename = site.Name;
            Application application = site.Applications[0];
            VirtualDirectory directory = application.VirtualDirectories[0];

            string websitePath = directory.PhysicalPath;

            DirectoryInfo di = new DirectoryInfo(websitePath);

            Trace.WriteLine(string.Format("DeleteWebsite IIS: {0}", sitename));

            string appplicationPoolName = application.ApplicationPoolName;
            site.Delete();

            foreach (ApplicationPool pool in iisManager.ApplicationPools)
            {
                if (pool.State == ObjectState.Started)
                {
                    pool.Stop();
                }
                if (pool.Name == appplicationPoolName)
                {
                    pool.Delete();
                }
            }

            Util.DeleteDatabase(sitename);

            try
            {
                di.Parent.Delete(true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Could not delete: {0}", di.Parent.FullName));
            }
            finally
            {

            }
        }

        private void DeleteWebsite(Site site)
        {
            DeleteWebsite(site, true, true);
        }

        private void LoadProperties()
        {
            textBoxSourceDir.Text = Properties.Settings.Default.SourceDir;
            textBoxSettingsWebsitesDir.Text = Properties.Settings.Default.WebsitesDir;
        }

        private void LoadWebsites()
        {
            listBoxWebsites.Items.Clear();
            SiteCollection siteCollection = iisManager.Sites;
            foreach (Site site in siteCollection)
            {
                listBoxWebsites.Items.Add(site);
            }
        }

        #endregion Methods
    }
}