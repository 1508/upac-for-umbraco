namespace Upac.Trac
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class DateHelper
    {
        #region Methods

        public static DateTime ParseUnixTimestamp(double timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }

        #endregion Methods
    }
}