namespace Upac.AddFilesToUmbracoPackage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    class Program
    {
        #region Methods

        public static XmlNode AddTextNode(XmlDocument doc, string name, string value)
        {
            XmlNode node = doc.CreateNode(XmlNodeType.Element, name, "");
            node.AppendChild(doc.CreateTextNode(value));
            return node;
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a path to an umbraco package xml file.");
                return;
            }
            if (args.Length > 2)
            {
                Console.WriteLine("Only one or two arg is accepted.");
                return;
            }

            bool useGuidForFilename = true;
            if (args.Length >= 2)
            {
                useGuidForFilename = args[1] == "1";
            }

            string  umbracoPackageFile = args[0];

            if (string.IsNullOrEmpty(umbracoPackageFile))
            {
                Console.WriteLine("Please provide a path to an umbraco package xml file.");
                umbracoPackageFile = Console.ReadLine();
            }
            Assert.EnsureFileExist(umbracoPackageFile, "umbracoPackageFile");

            // Get the path
            string packagePath = System.IO.Path.GetDirectoryName(umbracoPackageFile);
            Console.WriteLine("Package path: " + packagePath);

            // Get all files in Documents
            List<string> files = FileHelper.GetFilesRecursive(packagePath);

            if (files.Count == 0)
            {
                Console.WriteLine("No files to add, ending AddFilesToUmbracoPackage");
                return;
            }

            FileStream fsRead = new FileStream(umbracoPackageFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fsRead);
            fsRead.Dispose();

            XmlNode filesNode = xmldoc.SelectSingleNode("/umbPackage/files");
            Assert.EnsureNotNullReference(filesNode, "filesNode");

            foreach (string file in files)
            {
                if (file == umbracoPackageFile)
                {
                    continue;
                }
                string filename = System.IO.Path.GetFileName(file);
                string orgWebRelPath = System.IO.Path.GetDirectoryName(file).Replace(packagePath, string.Empty).Replace("\\", "/");
                Console.WriteLine(string.Concat("Moving file info into package: ", orgWebRelPath, "/", filename));

                XmlNode fileNode = AddTextNode(xmldoc, "file", "");

                if (useGuidForFilename)
                {
                    string newGuidFilename = string.Concat(Guid.NewGuid(), "-", filename);
                    fileNode.AppendChild(AddTextNode(xmldoc, "guid", newGuidFilename));
                    // Move the file
                    File.Move(file, Path.Combine(packagePath, newGuidFilename));
                }
                else
                {
                    fileNode.AppendChild(AddTextNode(xmldoc, "guid", filename));
                }
                fileNode.AppendChild(AddTextNode(xmldoc, "orgPath", orgWebRelPath));
                fileNode.AppendChild(AddTextNode(xmldoc, "orgName", filename));

                filesNode.AppendChild(fileNode);

            }

            FileStream fsWrite = new FileStream(umbracoPackageFile, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);

            // XML Document Saved
            xmldoc.Save(fsWrite);
            fsWrite.Dispose();

            if (useGuidForFilename)
            {
                // Only cleanup if useGuidForFilename
                // Let's clean up... all files should have been moved to the package path root
                // But we still need to clean up the folders
                string[] directories = Directory.GetDirectories(packagePath);
                foreach (string directory in directories)
                {
                    Directory.Delete(directory, true);
                }
            }
        }

        #endregion Methods
    }
}