namespace Upac.ConsoleUtilities.FixDocumentIds
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Assert
    {
        #region Methods

        public static void EnsureDirectoryExist(string value, string parameterName)
        {
            EnsureStringValue(value, parameterName);
            if (Directory.Exists(value) == false)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Directory does not exists");
            }
        }

        public static void EnsureFileExist(string value, string parameterName)
        {
            EnsureStringValue(value, parameterName);
            if (File.Exists(value) == false)
            {
                throw new ArgumentOutOfRangeException(parameterName, "File does not exists");
            }
        }

        public static void EnsureIntValue(string value, string parameterName)
        {
            EnsureStringValue(value, parameterName);
            int outI;
            if (int.TryParse(value, out outI) == false)
            {
                throw new ArgumentOutOfRangeException(parameterName, "value must be a int");
            }
        }

        public static void EnsureNotNullReference(object value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName, "Argument cannot be null");
        }

        public static void EnsureStringValue(string value, string parameterName)
        {
            EnsureNotNullReference(value, parameterName);
            if (string.IsNullOrEmpty(value.Trim()))
                throw new ArgumentOutOfRangeException(parameterName, "string must hold a value");
        }

        #endregion Methods
    }
}