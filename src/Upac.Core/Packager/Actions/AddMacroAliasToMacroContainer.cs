namespace Upac.Core.Packager.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    using log4net;

    using umbraco.BusinessLogic;
    using umbraco.cms.businesslogic.datatype;
    using umbraco.cms.businesslogic.packager.standardPackageActions;
    using umbraco.DataLayer;
    using umbraco.interfaces;

    public class AddMacroAliasToMacroContainer : IPackageAction
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(AddMacroAliasToMacroPicker));

        private readonly Guid macroPiackerDataTypeId = new Guid("474fcff8-9d2d-11de-abc6-ad7a56d89593");

        #endregion Fields

        #region Methods

        public static string GetAttributeValueFromNode(XmlNode node, string attributeName)
        {
            return (node.Attributes[attributeName] == null) ? string.Empty : node.Attributes[attributeName].InnerText;
        }

        public string Alias()
        {
            return "AddMacroAliasToMacroContainer";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            log.Info("AddMacroAliasToMacroContainer action started");
            Log.AddSynced(LogTypes.Debug, 0, -1, "AddMacroAliasToMacroContainer action started");
            string macroAlias = GetAttributeValueFromNode(xmlData, "macroAliasToAdd");
            string dataTypeName = GetAttributeValueFromNode(xmlData, "dataTypeNameToUpdate");
            if (!string.IsNullOrEmpty(macroAlias) && !string.IsNullOrEmpty(dataTypeName))
            {
                log.InfoFormat("Add macro alias: {0} to data type with name: {1}", macroAlias, dataTypeName);
                DataTypeDefinition[] definitions = DataTypeDefinition.GetAll();
                DataTypeDefinition datatypeToUpdate = definitions.SingleOrDefault(dt => dt.Text == dataTypeName);

                if (datatypeToUpdate == null)
                {
                    log.ErrorFormat("Data type with name '{0}' not found!", dataTypeName);
                    return false;
                }

                if (datatypeToUpdate.DataType.Id != macroPiackerDataTypeId)
                {
                    log.ErrorFormat("Data type with name '{0}' has wrong data type id: {1} should be: {2}!", dataTypeName, datatypeToUpdate.DataType.Id, macroPiackerDataTypeId);
                    return false;
                }

                string prevalue = GetPreValues(datatypeToUpdate.Id);
                Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("org prevalue: {0}", prevalue));
                if (!string.IsNullOrEmpty(prevalue))
                {
                    string[] allKeys = prevalue.Split(new char[] {'|'});
                    if (allKeys.Length > 0)
                    {
                        List<string> list = new List<string>();
                        string[] str = allKeys[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (str.Length > 0)
                        {
                            list.AddRange(str);
                        }
                        if (!list.Contains(macroAlias))
                        {
                            log.InfoFormat("Adding macro alias: {0} to data type with name: {1}", macroAlias, dataTypeName);
                            list.Add(macroAlias);
                            string[] valuesAsArray = list.ToArray();
                            string newDataTypePrevalue = string.Join(",", valuesAsArray);
                            newDataTypePrevalue = newDataTypePrevalue + prevalue.Substring(prevalue.IndexOf("|"));
                            Log.AddSynced(LogTypes.Debug, 0, -1, string.Format("Saving new prevalue: {0}", newDataTypePrevalue));
                            SavePreValues(newDataTypePrevalue, datatypeToUpdate.Id);
                            log.InfoFormat("Macro alias: {0} added to data type with name: {1}", macroAlias, dataTypeName);
                        }
                    }
                }

            }
            Log.AddSynced(LogTypes.Debug, 0, -1, "AddMacroAliasToMacroContainer action ended");
            log.Info("AddMacroAliasToMacroContainer action ended");
            return true;
        }

        public XmlNode SampleXml()
        {
            return helper.parseStringToXmlNode(string.Format("<Action runat=\"install\" undo=\"false\" alias=\"{0}\" macroAliasToAdd=\"CPalm.OEmed\" dataTypeNameToUpdate=\"Macro Picker\" />", Alias()));
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }

        private static string GetPreValues(int dataDefinitionID)
        {
            return string.Format("{0}", Application.SqlHelper.ExecuteScalar<string>("select value from cmsDataTypePreValues where datatypenodeid = @datatypenodeid", new IParameter[] { Application.SqlHelper.CreateParameter("@datatypenodeid", dataDefinitionID) }));
        }

        private static void SavePreValues(string data, int dataDefinitionID)
        {
            Application.SqlHelper.ExecuteNonQuery("delete from cmsDataTypePreValues where datatypenodeid = @dtdefid", new IParameter[] { Application.SqlHelper.CreateParameter("@dtdefid", dataDefinitionID) });
            Application.SqlHelper.ExecuteNonQuery("insert into cmsDataTypePreValues (datatypenodeid,[value],sortorder,alias) values (@dtdefid,@value,0,'')", new IParameter[] { Application.SqlHelper.CreateParameter("@dtdefid", dataDefinitionID), Application.SqlHelper.CreateParameter("@value", data) });
        }

        #endregion Methods
    }
}