namespace Upac.Core.EditorControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using umbraco;
    using umbraco.cms.businesslogic.property;
    using umbraco.cms.businesslogic.web;

    public class SettingsHelper : System.Web.UI.HtmlControls.HtmlGenericControl
    {
        #region Fields

        public int NodeId;

        #endregion Fields

        #region Methods

        public void Save()
        {
            string[] keys = Page.Request.Form.AllKeys;
            foreach (string key in keys)
            {
                if (key.StartsWith("DictionaryHelper"))
                {
                    string strDocId = key.Substring(key.IndexOf("docId_") + 6);
                    string property = key.Remove(key.IndexOf("docId_") - 1).Substring(17);
                    string value = Page.Request.Form[key];

                    Document doc = new Document(int.Parse(strDocId));

                    if (property == "Text" || property == "Default" || property == "Help")
                    {
                        Property prop = doc.getProperty(property);
                        if (prop != null)
                        {
                            prop.Value = value;
                        }
                    }
                    else if (property == "Key")
                    {
                        doc.Text = value;
                    }

                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //Page.ClientScript.RegisterClientScriptInclude("skinBrowser", GlobalSettings.Path + "/plugins/blog4umbraco/skinBrowser.js");
            //umbraco.uicontrols.helper.AddLinkToHeader("skinBrowser", GlobalSettings.Path + "/plugins/blog4umbraco/skinBrowser.css", this.Page);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<table>");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td>Key</td>");
            sb.AppendLine("<td>Value</td>");
            sb.AppendLine("<td>Default</td>");
            sb.AppendLine("<td>Help</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            Document currentDocument = new Document(NodeId);
            if (currentDocument != null)
            {
                Document[] children = currentDocument.Children;
                foreach (Document child in children)
                {
                    if (child.ContentType.Alias == Configuration.ConfigurationManager.UpacSettings.DocumentTypes.DictionaryText)
                    {
                        string key = child.Text;
                        string text = child.getProperty("Text").Value.ToString();
                        string help = child.getProperty("Help").Value.ToString();
                        string defaultText = child.getProperty("Default").Value.ToString();
                        bool hide = child.getProperty("Hide").Value.ToString() == "1";
                        bool protectRename = child.getProperty("ProtectRename").Value.ToString() == "1";

                        if (!hide)
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td>");
                            if (protectRename)
                            {
                                sb.AppendLine(key);
                            }
                            else
                            {
                                sb.AppendLine(GenerateTextBox("DictionaryHelper_Key_docId_" + child.Id, key, "DictionaryHelperInputKey"));
                            }
                            sb.AppendLine("</td>");
                            sb.AppendLine("<td>");
                            sb.AppendLine(GenerateTextBox("DictionaryHelper_Text_docId_" + child.Id, text, "DictionaryHelperInputText"));
                            sb.AppendLine("</td>");
                            sb.AppendLine("<td>");
                            sb.AppendLine(GenerateTextBox("DictionaryHelper_Default_docId_" + child.Id, defaultText, "DictionaryHelperInputDefault"));
                            sb.AppendLine("</td>");
                            sb.AppendLine("<td>" + help + "</td>");
                            sb.AppendLine("</tr>");
                        }
                    }
                }
            }

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine(GenerateTextBox("DictionaryHelper_Key_New", string.Empty, "DictionaryHelperInputKey"));
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(GenerateTextBox("DictionaryHelper_Text_New", string.Empty, "DictionaryHelperInputText"));
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(GenerateTextBox("DictionaryHelper_Default_New", string.Empty, "DictionaryHelperInputDefault"));
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(GenerateTextBox("DictionaryHelper_Help_New", string.Empty, "DictionaryHelperInputDefault"));
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("</tbody>");

            sb.AppendLine("</table>");

            writer.Write(sb.ToString());
        }

        private string GenerateTextBox(string name, string valueText, string cssClass)
        {
            return "<input type=\"text\" class=\"" + cssClass + "\" onChange=\"this.name = '" + name + "'\" name=\"dummy\" value=\"" + HttpUtility.HtmlEncode(valueText) + "\" />";
        }

        #endregion Methods
    }
}