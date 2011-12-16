namespace Upac.NantTasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    using NAnt.Core;
    using NAnt.Core.Attributes;

    /// <summary>
    /// Appends nodes from a source xml document to a target xml document
    /// </summary>
    [TaskName("appendxml")]
    public class AppendXmlTask : Task
    {
        #region Properties

        [TaskAttribute("sourcefile", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string SourceFile
        {
            get; set;
        }

        [TaskAttribute("sourcexpath", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string SourceXPath
        {
            get; set;
        }

        [TaskAttribute("targetfile", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Targetfile
        {
            get; set;
        }

        [TaskAttribute("targetxpath", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string TargetXPath
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        protected override void ExecuteTask()
        {
            //Check the file exists
            string sourceDocPath = Project.ExpandProperties(SourceFile, Location);
            if (File.Exists(sourceDocPath) == false)
            {
                throw new BuildException("The sourcefile specified does not exist");
            }

            string targetDocPath = Project.ExpandProperties(Targetfile, Location);
            if (File.Exists(targetDocPath) == false)
            {
                throw new BuildException("The targetfile specified does not exist");
            }

            XmlDocument sourceDoc = new XmlDocument();
            FileStream fsSourceRead = new FileStream(sourceDocPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            sourceDoc.Load(fsSourceRead);
            fsSourceRead.Dispose();

            XmlNodeList sourceNodes = sourceDoc.SelectNodes(SourceXPath);
            if (sourceNodes.Count < 1)
            {
                Project.Log(Level.Warning, "No nodes found via XPath in source file!");
            }
            else
            {
                Project.Log(Level.Info, string.Format("Found {0} nodes in source file via XPath", sourceNodes.Count));

                XmlDocument targetDoc = new XmlDocument();
                FileStream fsTargetRead = new FileStream(targetDocPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                targetDoc.Load(fsTargetRead);
                fsTargetRead.Dispose();

                XmlNodeList targetNodes = targetDoc.SelectNodes(TargetXPath);
                if (targetNodes.Count < 0)
                {
                    Project.Log(Level.Warning, "No target node found via XPath in target file!");
                }
                else if (targetNodes.Count > 1)
                {
                    Project.Log(Level.Warning, string.Format("{0} nodes found when trying to find one target node found via XPath in target file!", targetNodes.Count));
                }
                else
                {
                    XmlNode targetNode = targetNodes[0];
                    foreach (XmlNode sourceNode in sourceNodes)
                    {
                        XmlNode newNode = targetDoc.ImportNode(sourceNode, true);
                        targetNode.AppendChild(newNode);
                    }

                    FileStream fsWrite = new FileStream(targetDocPath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
                    try
                    {
                        targetDoc.Save(fsWrite);
                    }
                    finally
                    {
                        fsWrite.Dispose();
                    }
                    Project.Log(Level.Info, "New nodes imported");
                }
            }
        }

        #endregion Methods
    }
}