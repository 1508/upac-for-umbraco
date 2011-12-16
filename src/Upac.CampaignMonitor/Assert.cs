namespace Upac.CampaignMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class Assert
    {
        #region Methods

        public static void AreEqual(string value1, string value2)
        {
            if (value1 != value2)
            {
                throw new ArgumentException("value1 and value2 must be equal", value1);
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