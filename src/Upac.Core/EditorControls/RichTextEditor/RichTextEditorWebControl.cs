namespace Upac.Core.EditorControls.RichTextEditor
{
    using System;
    using System.Web;

    using Upac.Core.Configuration;
    using Upac.Core.Extensions;

    using log4net;

    using umbraco.cms.businesslogic.template;
    using umbraco.cms.businesslogic.web;
    using umbraco.editorControls.tinyMCE3;
    using umbraco.interfaces;
    using umbraco.presentation.nodeFactory;

    public class RichTextEditorWebControl : TinyMCE
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(RichTextEditorWebControl));

        #endregion Fields

        #region Constructors

        public RichTextEditorWebControl(IData Data, string Configuration)
            : base(Data, FixWidth(Configuration, Data))
        {
        }

        #endregion Constructors

        #region Methods

        private static string FixWidth(string oldConfig, IData Data)
        {
            if (Data == null || !ConfigurationManager.UpacSettings.AppSettings.SetWidthOnRichTextEditorViaTemplateAlias)
            {
                return oldConfig;
            }

            bool oldConfigDirtyFlag = false;
            string newConfig = string.Empty;

            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                string strNodeId = context.Request.QueryString["id"];
                if (!string.IsNullOrEmpty(strNodeId))
                {
                    Document doc;
                    try
                    {
                        doc = new Document(true, int.Parse(strNodeId));
                    }
                    catch (Exception exception)
                    {
                        log.Error("Error in RichTextEditorWebControl, could not load document", exception);
                        return oldConfig;
                    }

                    SiteSettings settings = ConfigurationManager.GetSiteSettingViaDocument(doc);
                    if (settings != null)
                    {

                        Node richtextEditorSettingsNode = settings.ConfigurationNode.GetDescendantViaPath(ConfigurationManager.UpacSettings.NodeNames.RichtextEditorSettings);
                        if (richtextEditorSettingsNode != null)
                        {
                            Node profilesNode = richtextEditorSettingsNode.GetDescendantViaPath(ConfigurationManager.UpacSettings.NodeNames.RichtextEditorSettingsProfiles);

                            if (profilesNode != null && doc.Template > 0)
                            {

                                Template documentTemplate = new Template(doc.Template);

                                Node richtextProfile = profilesNode.GetDescendantViaPath(documentTemplate.Alias);
                                if (richtextProfile != null)
                                {
                                    string[] strArray = oldConfig.Split("|".ToCharArray());

                                    int newWidth = richtextProfile.GetPropertyValue("Width", -1);
                                    if (newWidth > -1)
                                    {
                                        // body i richtext editor har en margin
                                        newWidth = newWidth + 35;

                                        if (strArray.Length > 3)
                                        {
                                            if (strArray[3] == "1")
                                            {
                                                // full width
                                            }
                                            else if (strArray[4].Split(new char[] { ',' }).Length > 1)
                                            {
                                                string current_height = strArray[4].Split(new char[] { ',' })[1];
                                                strArray[4] = newWidth + "," + current_height;
                                                oldConfigDirtyFlag = true;
                                            }

                                        }
                                    }

                                    int newImageMaxWidth = richtextProfile.GetPropertyValue("ImageMaxWidth", -1);
                                    if (newImageMaxWidth > -1)
                                    {
                                        if ((strArray.Length > 7) && (strArray[7] != ""))
                                        {
                                            strArray[7] = newImageMaxWidth.ToString();
                                        }

                                    }

                                    foreach (string s in strArray)
                                    {
                                        newConfig += s + "|";
                                    }
                                    newConfig = newConfig.Remove(newConfig.Length - 1);

                                }
                            }
                        }
                    }

                }
            }

            if (oldConfigDirtyFlag)
            {
                oldConfig = newConfig;
            }

            return oldConfig;
        }

        #endregion Methods
    }
}