namespace Upac.WebsiteAdmin
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSourceDir = new System.Windows.Forms.TextBox();
            this.buttonChooseDistFolder = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSettingsChooseWebsitesDir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSettingsWebsitesDir = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonReinstallSite = new System.Windows.Forms.Button();
            this.checkBoxIncludeFiles = new System.Windows.Forms.CheckBox();
            this.checkBoxOnDeleteAlsoDeleteDb = new System.Windows.Forms.CheckBox();
            this.buttonDeleteWebsite = new System.Windows.Forms.Button();
            this.listBoxWebsites = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonCreateWebsite = new System.Windows.Forms.Button();
            this.textBoxNewWebsiteName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source dir";
            // 
            // textBoxSourceDir
            // 
            this.textBoxSourceDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSourceDir.Location = new System.Drawing.Point(85, 24);
            this.textBoxSourceDir.Name = "textBoxSourceDir";
            this.textBoxSourceDir.Size = new System.Drawing.Size(464, 20);
            this.textBoxSourceDir.TabIndex = 1;
            // 
            // buttonChooseDistFolder
            // 
            this.buttonChooseDistFolder.Location = new System.Drawing.Point(555, 24);
            this.buttonChooseDistFolder.Name = "buttonChooseDistFolder";
            this.buttonChooseDistFolder.Size = new System.Drawing.Size(114, 22);
            this.buttonChooseDistFolder.TabIndex = 2;
            this.buttonChooseDistFolder.Text = "Choose source dir";
            this.buttonChooseDistFolder.UseVisualStyleBackColor = true;
            this.buttonChooseDistFolder.Click += new System.EventHandler(this.buttonChooseDistFolder_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(555, 127);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(114, 22);
            this.buttonSaveSettings.TabIndex = 3;
            this.buttonSaveSettings.Text = "Save settings";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSettingsChooseWebsitesDir);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxSettingsWebsitesDir);
            this.groupBox1.Controls.Add(this.buttonSaveSettings);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonChooseDistFolder);
            this.groupBox1.Controls.Add(this.textBoxSourceDir);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(683, 165);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // buttonSettingsChooseWebsitesDir
            // 
            this.buttonSettingsChooseWebsitesDir.Location = new System.Drawing.Point(555, 60);
            this.buttonSettingsChooseWebsitesDir.Name = "buttonSettingsChooseWebsitesDir";
            this.buttonSettingsChooseWebsitesDir.Size = new System.Drawing.Size(114, 22);
            this.buttonSettingsChooseWebsitesDir.TabIndex = 6;
            this.buttonSettingsChooseWebsitesDir.Text = "Choose source dir";
            this.buttonSettingsChooseWebsitesDir.UseVisualStyleBackColor = true;
            this.buttonSettingsChooseWebsitesDir.Click += new System.EventHandler(this.buttonSettingsChooseWebsitesDir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Websites dir";
            // 
            // textBoxSettingsWebsitesDir
            // 
            this.textBoxSettingsWebsitesDir.Location = new System.Drawing.Point(85, 60);
            this.textBoxSettingsWebsitesDir.Name = "textBoxSettingsWebsitesDir";
            this.textBoxSettingsWebsitesDir.Size = new System.Drawing.Size(464, 20);
            this.textBoxSettingsWebsitesDir.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonReinstallSite);
            this.groupBox2.Controls.Add(this.checkBoxIncludeFiles);
            this.groupBox2.Controls.Add(this.checkBoxOnDeleteAlsoDeleteDb);
            this.groupBox2.Controls.Add(this.buttonDeleteWebsite);
            this.groupBox2.Controls.Add(this.listBoxWebsites);
            this.groupBox2.Location = new System.Drawing.Point(12, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 238);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Websites";
            // 
            // buttonReinstallSite
            // 
            this.buttonReinstallSite.Location = new System.Drawing.Point(556, 120);
            this.buttonReinstallSite.Name = "buttonReinstallSite";
            this.buttonReinstallSite.Size = new System.Drawing.Size(113, 23);
            this.buttonReinstallSite.TabIndex = 4;
            this.buttonReinstallSite.Text = "Reinstall website";
            this.buttonReinstallSite.UseVisualStyleBackColor = true;
            this.buttonReinstallSite.Click += new System.EventHandler(this.buttonReinstallSite_Click);
            // 
            // checkBoxIncludeFiles
            // 
            this.checkBoxIncludeFiles.AutoSize = true;
            this.checkBoxIncludeFiles.Location = new System.Drawing.Point(556, 26);
            this.checkBoxIncludeFiles.Name = "checkBoxIncludeFiles";
            this.checkBoxIncludeFiles.Size = new System.Drawing.Size(82, 17);
            this.checkBoxIncludeFiles.TabIndex = 3;
            this.checkBoxIncludeFiles.Text = "Include files";
            this.checkBoxIncludeFiles.UseVisualStyleBackColor = true;
            // 
            // checkBoxOnDeleteAlsoDeleteDb
            // 
            this.checkBoxOnDeleteAlsoDeleteDb.AutoSize = true;
            this.checkBoxOnDeleteAlsoDeleteDb.Location = new System.Drawing.Point(556, 49);
            this.checkBoxOnDeleteAlsoDeleteDb.Name = "checkBoxOnDeleteAlsoDeleteDb";
            this.checkBoxOnDeleteAlsoDeleteDb.Size = new System.Drawing.Size(108, 17);
            this.checkBoxOnDeleteAlsoDeleteDb.TabIndex = 2;
            this.checkBoxOnDeleteAlsoDeleteDb.Text = "Include database";
            this.checkBoxOnDeleteAlsoDeleteDb.UseVisualStyleBackColor = true;
            // 
            // buttonDeleteWebsite
            // 
            this.buttonDeleteWebsite.Location = new System.Drawing.Point(556, 149);
            this.buttonDeleteWebsite.Name = "buttonDeleteWebsite";
            this.buttonDeleteWebsite.Size = new System.Drawing.Size(114, 23);
            this.buttonDeleteWebsite.TabIndex = 1;
            this.buttonDeleteWebsite.Text = "Delete website";
            this.buttonDeleteWebsite.UseVisualStyleBackColor = true;
            this.buttonDeleteWebsite.Click += new System.EventHandler(this.buttonDeleteWebsite_Click);
            // 
            // listBoxWebsites
            // 
            this.listBoxWebsites.FormattingEnabled = true;
            this.listBoxWebsites.Location = new System.Drawing.Point(9, 25);
            this.listBoxWebsites.Name = "listBoxWebsites";
            this.listBoxWebsites.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxWebsites.Size = new System.Drawing.Size(540, 147);
            this.listBoxWebsites.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonCreateWebsite);
            this.groupBox3.Controls.Add(this.textBoxNewWebsiteName);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(12, 444);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(683, 199);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Create website";
            // 
            // buttonCreateWebsite
            // 
            this.buttonCreateWebsite.Location = new System.Drawing.Point(556, 33);
            this.buttonCreateWebsite.Name = "buttonCreateWebsite";
            this.buttonCreateWebsite.Size = new System.Drawing.Size(113, 23);
            this.buttonCreateWebsite.TabIndex = 2;
            this.buttonCreateWebsite.Text = "Create website";
            this.buttonCreateWebsite.UseVisualStyleBackColor = true;
            this.buttonCreateWebsite.Click += new System.EventHandler(this.buttonCreateWebsite_Click);
            // 
            // textBoxNewWebsiteName
            // 
            this.textBoxNewWebsiteName.Location = new System.Drawing.Point(12, 37);
            this.textBoxNewWebsiteName.Name = "textBoxNewWebsiteName";
            this.textBoxNewWebsiteName.Size = new System.Drawing.Size(537, 20);
            this.textBoxNewWebsiteName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(283, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Website name (no spaces, only letters and only lowercase)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 655);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSourceDir;
        private System.Windows.Forms.Button buttonChooseDistFolder;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxWebsites;
        private System.Windows.Forms.Button buttonDeleteWebsite;
        private System.Windows.Forms.CheckBox checkBoxOnDeleteAlsoDeleteDb;
        private System.Windows.Forms.CheckBox checkBoxIncludeFiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSettingsWebsitesDir;
        private System.Windows.Forms.Button buttonSettingsChooseWebsitesDir;
        private System.Windows.Forms.Button buttonReinstallSite;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNewWebsiteName;
        private System.Windows.Forms.Button buttonCreateWebsite;

    }
}

