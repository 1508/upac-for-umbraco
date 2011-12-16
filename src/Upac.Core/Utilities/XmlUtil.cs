namespace Upac.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    public static class XmlUtil
    {
        #region Methods

        /// <summary>
        /// Pretty Print the input XML string, such as adding indentations to each level of elements
        /// and carriage return to each line
        /// </summary>
        /// <param name="xmlText"></param>
        /// <returns>New formatted XML string</returns>
        public static String FormatNicely(String xmlText)
        {
            if (xmlText == null || xmlText.Trim().Length == 0)
                return "";

            String result = "";

            MemoryStream memStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(memStream, Encoding.Unicode);
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                xmlDoc.LoadXml(xmlText);

                xmlWriter.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                xmlDoc.WriteContentTo(xmlWriter);
                xmlWriter.Flush();
                memStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                memStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader streamReader = new StreamReader(memStream);

                // Extract the text from the StreamReader.
                String FormattedXML = streamReader.ReadToEnd();

                result = FormattedXML;
            }
            catch (Exception)
            {
                // Return the original unchanged.
                result = xmlText;
            }
            finally
            {
                memStream.Close();
                xmlWriter.Close();
            }
            return result;
        }

        #endregion Methods
    }
}