namespace Upac.Core.Packager.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml;

    using umbraco.cms.businesslogic.packager.standardPackageActions;
    using umbraco.interfaces;

    /// <summary>
    /// Moves a file
    /// </summary>
    /// <remarks>
    /// Can be used for renaming as well
    /// </remarks>
    public class MoveFile : IPackageAction
    {
        #region Methods

        public static string GetAttributeValueFromNode(XmlNode node, string attributeName)
        {
            string result = string.Empty;

            if (node.Attributes[attributeName] != null)
            {
                //Attribute has an value return that.
                result = node.Attributes[attributeName].InnerText;
            }
            return result;
        }

        public string Alias()
        {
            return "UpacMoveFile";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            File.Move(
                HttpContext.Current.Server.MapPath(GetSourceFileName(xmlData)),
                HttpContext.Current.Server.MapPath(GetTargetFileName(xmlData)));

            return true;
        }

        public XmlNode SampleXml()
        {
            string sample = string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" sourceFile=\"~/bin/UCommerce.Uninstaller.dll.tmp\" targetFile=\"~/bin/UCommerce.Uninstaller.dll\"/>", Alias());
            return helper.parseStringToXmlNode(sample);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            File.Delete(HttpContext.Current.Server.MapPath(GetTargetFileName(xmlData)));

            return true;
        }

        private string GetSourceFileName(XmlNode xmlData)
        {
            return GetAttributeValueFromNode(xmlData, "sourceFile");
        }

        private string GetTargetFileName(XmlNode xmlData)
        {
            return GetAttributeValueFromNode(xmlData, "targetFile");
        }

        #endregion Methods
    }
}