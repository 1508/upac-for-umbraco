namespace Upac.Membership.Installer.UserControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Xml;

    using umbraco.cms.businesslogic.datatype;
    using umbraco.cms.businesslogic.member;
    using umbraco.cms.businesslogic.propertytype;
    using umbraco.cms.businesslogic.web;

    using Upac.Core.Packager;

    public partial class MembershipInstaller : System.Web.UI.UserControl
    {
        #region Methods

        protected void CreateMemberTypeAndMemberGroup(object sender, EventArgs e)
        {
            MemberGroup[] memberGroups = MemberGroup.GetAll;
            bool memberGroupExist = false;
            foreach (MemberGroup memberGroup in memberGroups)
            {
                if (memberGroup.Text == "Extranet")
                {
                    memberGroupExist = true;
                }
            }
            if (memberGroupExist == false)
            {
                MemberGroup memberType = MemberGroup.MakeNew("Extranet", umbraco.BasePages.UmbracoEnsuredPage.CurrentUser);
            }

            MemberType[] memberTypes = MemberType.GetAll;
            bool memberTypeExist = false;
            foreach (MemberType memberType in memberTypes)
            {
                if (memberType.Text == "Extranet")
                {
                    memberTypeExist = true;
                }
            }
            if (memberTypeExist == false)
            {
                MemberType memberType = MemberType.MakeNew(umbraco.BasePages.UmbracoEnsuredPage.CurrentUser, "Standard");
                memberType.Alias = "Standard";
                memberType.Text = "Standard";
                memberType.IconUrl = "memberType.gif";
                memberType.Thumbnail = "member.png";

                int tabInfoId = memberType.AddVirtualTab("Info");
                int tabPersonalId = memberType.AddVirtualTab("Personal");
                int tabServicesId = memberType.AddVirtualTab("Services");
                int tabDataId = memberType.AddVirtualTab("Data");

                DataTypeDefinition textstring = DataTypeDefinition.GetDataTypeDefinition(-88);
                DataTypeDefinition checkbox = DataTypeDefinition.GetDataTypeDefinition(-49);
                DataTypeDefinition upload = DataTypeDefinition.GetDataTypeDefinition(-90);
                DataTypeDefinition datePickerWithTime = DataTypeDefinition.GetDataTypeDefinition(-36);
                DataTypeDefinition numeric = DataTypeDefinition.GetDataTypeDefinition(-51);
                DataTypeDefinition textboxMultiple = DataTypeDefinition.GetDataTypeDefinition(-89);

                // Info
                memberType.AddPropertyType(textstring, "nickname", "Nickname");
                memberType.getPropertyType("nickname").TabId = tabInfoId;

                memberType.AddPropertyType(upload, "profilePicture", "Profile picture");
                memberType.getPropertyType("profilePicture").TabId = tabInfoId;

                // Personal
                memberType.AddPropertyType(datePickerWithTime, "birthdate", "Birthdate");
                memberType.getPropertyType("birthdate").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "gender", "Gender");
                memberType.getPropertyType("gender").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "firstname", "Firstname");
                memberType.getPropertyType("firstname").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "surname", "Surname");
                memberType.getPropertyType("surname").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "locationAddress", "Address");
                memberType.getPropertyType("locationAddress").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "locationZip", "Zip code");
                memberType.getPropertyType("locationZip").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "locationCity", "City");
                memberType.getPropertyType("locationCity").TabId = tabPersonalId;

                memberType.AddPropertyType(textstring, "locationCountry", "Country");
                memberType.getPropertyType("locationCountry").TabId = tabPersonalId;

                // Servives
                memberType.AddPropertyType(textstring, "twitterAlias", "Twitter alias");
                memberType.getPropertyType("twitterAlias").TabId = tabServicesId;

                memberType.AddPropertyType(textstring, "flickrAlias", "Flickr alias");
                memberType.getPropertyType("flickrAlias").TabId = tabServicesId;

                // Data
                memberType.AddPropertyType(checkbox, "approved", "Approved");
                memberType.getPropertyType("approved").TabId = tabDataId;

                memberType.AddPropertyType(checkbox, "lock", "Lock user");
                memberType.getPropertyType("lock").TabId = tabDataId;

                memberType.AddPropertyType(numeric, "failed_logins", "Failed Password Attempts");
                memberType.getPropertyType("failed_logins").TabId = tabDataId;

                memberType.AddPropertyType(textboxMultiple, "comments", "Comment");
                memberType.getPropertyType("comments").TabId = tabDataId;

                memberType.AddPropertyType(datePickerWithTime, "last_login", "LastLogin");
                memberType.getPropertyType("last_login").TabId = tabDataId;

                memberType.AddPropertyType(textstring, "SecretCode", "Hemmelig kode");
                PropertyType typeSecretCode = memberType.getPropertyType("SecretCode");
                typeSecretCode.TabId = tabDataId;
                typeSecretCode.Description = "Bruges blandt andet til aktivering/sletning emails";

            }
        }

        protected void InstallSettingsAndContent(object sender, EventArgs e)
        {
            Upac.Core.Packager.NodeSettingInstaller settingInstaller = new NodeSettingInstaller("Upac.Membership", "Upac.Membership.Installer.Settings.xml");
            settingInstaller.Install();

            Upac.Core.Packager.NodeInstaller nodeInstaller = new NodeInstaller("Upac.Membership", "Upac.Membership.Installer.Content.xml");
            Document[] websites = Document.GetRootDocuments();

            foreach (Document website in websites)
            {
                if (website != null && website.ContentType.Alias == "Folder - Siteroot")
                {
                    Document[] rootDocChildren = website.Children;
                    if (rootDocChildren.Length > 0)
                    {
                        Document frontpage = rootDocChildren[0];
                        nodeInstaller.InstallNodesInDocument(frontpage);
                    }
                }

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void UpdateMacroes(object sender, EventArgs e)
        {
            umbraco.cms.businesslogic.macro.Macro.GetByAlias("Upac.Membership.Create").RenderContent = false;
            umbraco.cms.businesslogic.macro.Macro.GetByAlias("Upac.Membership.Login").RenderContent = false;
            umbraco.cms.businesslogic.macro.Macro.GetByAlias("Upac.Membership.Activate").RenderContent = false;
            umbraco.cms.businesslogic.macro.Macro.GetByAlias("Upac.Membership.PasswordRecovery").RenderContent = false;
            umbraco.cms.businesslogic.macro.Macro.GetByAlias("Upac.Membership.YourProfile").RenderContent = false;
            umbraco.cms.businesslogic.macro.Macro.GetByAlias("Upac.Membership.Logout").RenderContent = false;
        }

        protected void UpdateWebConfig(object sender, EventArgs e)
        {
            string path = HostingEnvironment.MapPath("/web.config");
            XmlDocument document = new XmlDocument();
            document.Load(path);
            XmlNode node = document.SelectSingleNode("configuration/system.web/membership/providers/add[@name = 'UmbracoMembershipProvider']");
            XmlNode newNode = node.Clone();
            newNode.Attributes["defaultMemberTypeAlias"].Value = "Standard";

            XmlAttribute attribute = document.CreateAttribute("umbracoApprovePropertyTypeAlias");
            attribute.Value = "approved";
            newNode.Attributes.Append(attribute);

            attribute = document.CreateAttribute("umbracoLockPropertyTypeAlias");
            attribute.Value = "lock";
            newNode.Attributes.Append(attribute);

            attribute = document.CreateAttribute("umbracoFailedPasswordAttemptsPropertyTypeAlias");
            attribute.Value = "failed_logins";
            newNode.Attributes.Append(attribute);

            attribute = document.CreateAttribute("umbracoCommentPropertyTypeAlias");
            attribute.Value = "comments";
            newNode.Attributes.Append(attribute);

            attribute = document.CreateAttribute("umbracoLastLoginPropertyTypeAlias");
            attribute.Value = "last_login";
            newNode.Attributes.Append(attribute);

            node.ParentNode.ReplaceChild(newNode, node);

            document.Save(path);
        }

        #endregion Methods
    }
}