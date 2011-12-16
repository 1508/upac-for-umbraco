namespace Upac.ConsoleUtilities.FixDocumentIds
{
    using System;

    class Program
    {
        #region Methods

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a path to an umbraco package xml file.");
                return;
            }
            if (args.Length > 1)
            {
                Console.WriteLine("Only one arg is accepted.");
                return;
            }

            string umbracoPackageFile = args[0];

            if (string.IsNullOrEmpty(umbracoPackageFile))
            {
                Console.WriteLine("Please provide a path to an umbraco package xml file.");
                umbracoPackageFile = Console.ReadLine();
            }
            Assert.EnsureFileExist(umbracoPackageFile, "umbracoPackageFile");

            Fixer fixer = new Fixer(umbracoPackageFile);
            fixer.Fix();
            return;
        }

        #endregion Methods
    }
}