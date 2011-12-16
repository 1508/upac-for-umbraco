namespace Upac.Core.Events
{
    using System;

    using log4net;

    using umbraco.cms.businesslogic;
    using umbraco.cms.businesslogic.property;
    using umbraco.cms.businesslogic.template;
    using umbraco.cms.businesslogic.web;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Configuration;
    using Upac.Core.Data;
    using Upac.Core.Extensions;
    using Upac.Core.Utilities;

    public static class SetDefaultValues
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(SetDefaultValues));

        #endregion Fields

        #region Methods

        public static void SetDefaultValuesOnNew(Document newDocument, umbraco.cms.businesslogic.NewEventArgs e)
        {
            log.Info("SetDefaultValuesOnNew Startet");
            log.InfoFormat("Document id: {1} - Document Type Alias: {0}", newDocument.ContentType.Alias, newDocument.Id);
            // We wrap it in a try since this part should not break for the user in umbraco
            try
            {
                string path = newDocument.Path;
                if (string.IsNullOrEmpty(path))
                {
                    StopWithError("Path on new document not found!");
                    return;
                }

                string[] strings = StringUtil.Split(path, ",", true);
                string strHomeDocId = strings[1];

                // Now we have the doc home id, lets see if the default value document based on document type alias exists
                string cacheKeyDefaultValueDocumentTypeAlias = string.Format("SetDefaultValuesOnNew#homeDoc:{0}#DefaultValueDocumentTypeAlias:{1}", strHomeDocId, newDocument.ContentType.Alias);
                int defaultValueDocId = -1;
                if (CacheUtil.Exist(cacheKeyDefaultValueDocumentTypeAlias))
                {
                    defaultValueDocId = CacheUtil.Get<int>(cacheKeyDefaultValueDocumentTypeAlias);
                }
                else
                {
                    // Does not exist in the cache, lets try to look it up
                    int defaultValuesFolderDocId = GetDefaultValuesFolderId(strHomeDocId);
                    if (defaultValuesFolderDocId == -1)
                    {
                        StopWithError("Could not load default values folder id!");
                        return;
                    }

                    // For speed we use the published node factory
                    Node node = UmbracoUtil.GetNode(defaultValuesFolderDocId);
                    if (node == null)
                    {
                        StopWithError(string.Format("Could not load published node for default values folder id: {0}!", defaultValuesFolderDocId));
                        return;
                    }

                    Node defaultValueNode = node.GetDescendantViaDocumentTypePath(newDocument.ContentType.Alias);
                    if (defaultValueNode != null)
                    {
                        defaultValueDocId = defaultValueNode.Id;
                        CacheUtil.Insert(cacheKeyDefaultValueDocumentTypeAlias, defaultValueDocId, 10, false);
                    }
                    else
                    {
                        log.DebugFormat("No default node for document type alias: {0}", newDocument.ContentType.Alias);
                    }
                }

                if (defaultValueDocId > -1)
                {
                    Document defaultValueDocument = new Document(true, defaultValueDocId);

                    log.DebugFormat("ContentType {0} match. Let's start copying", newDocument.ContentType.Alias);

                    umbraco.cms.businesslogic.property.Properties newDocumentProperties = newDocument.GenericProperties;
                    umbraco.cms.businesslogic.property.Properties defaultValueProperties = defaultValueDocument.GenericProperties;

                    for (int i = 0; i < newDocumentProperties.Count; i++)
                    {
                        umbraco.cms.businesslogic.property.Property newDocumentProperty = newDocumentProperties[i];
                        umbraco.cms.businesslogic.property.Property defaultDocumentProperty = defaultValueProperties[i];

                        if (newDocumentProperty.Value != defaultDocumentProperty.Value)
                        {
                            log.DebugFormat("Set value on property alias {0}", newDocumentProperty.PropertyType.Alias);
                            newDocumentProperty.Value = defaultDocumentProperty.Value;
                        }

                    }
                }
                log.Info("SetDefaultValuesOnNew Ended");
            }
            catch (Exception ex)
            {
                log.Error("Could not complete SetDefaultValuesOnNew - stopped with exception", ex);
            }
        }

        private static int GetDefaultValuesFolderId(string strHomeDocId)
        {
            int defaultValuesFolderId = -1;
            string cacheDefaultValuesFolderId = string.Format("SetDefaultValuesOnNew#homeDoc:{0}#defaultValuesFolderId", strHomeDocId);
            if (CacheUtil.Exist(cacheDefaultValuesFolderId))
            {
                defaultValuesFolderId = CacheUtil.Get<int>(cacheDefaultValuesFolderId);
                if (defaultValuesFolderId > -1)
                {
                    return defaultValuesFolderId;
                }
            }
            else
            {
                log.InfoFormat("Trying to find default values folder for home id: {0}", strHomeDocId);
                Document home = new Document(true, int.Parse(strHomeDocId));
                foreach (Document child in home.Children)
                {
                    log.InfoFormat("Test to see if we have the ConfigurationContainer below home. Current ContentType.Alias: {0}", child.ContentType.Alias);
                    if (child.ContentType.Alias == ConfigurationManager.DocumentTypeAliases.ConfigurationContainer.Value)
                    {
                        foreach (Document grandChild in child.Children)
                        {
                            log.InfoFormat("Test to see if we have the {1} below the ConfigurationContainer. Current ContentType.Alias: {0}", grandChild.ContentType.Alias, ConfigurationManager.DocumentTypeAliases.ConfigurationDefaultValuesContainer.Value);
                            if (grandChild.ContentType.Alias == ConfigurationManager.DocumentTypeAliases.ConfigurationDefaultValuesContainer.Value)
                            {
                                log.InfoFormat("We have found the default values folder id: {0}", grandChild.Id);
                                defaultValuesFolderId = grandChild.Id;
                                CacheUtil.Insert(cacheDefaultValuesFolderId, defaultValuesFolderId, 10, false);
                                return defaultValuesFolderId;
                            }
                        }
                    }
                }
            }
            return defaultValuesFolderId;
        }

        private static void StopWithError(string errorMessage)
        {
            log.Error(errorMessage);
            log.Info("SetDefaultValuesOnNew Ended could not complete");
        }

        #endregion Methods
    }
}