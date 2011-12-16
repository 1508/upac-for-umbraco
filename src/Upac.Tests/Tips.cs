namespace Upac.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    class Tips
    {
        #region Methods

        public void CastArrayToList()
        {
            string[] array = new string[] { "asd1", "asd2", "asd3" };
            IList list = array;
            ICollection<string> collectionGeneric = array;

            bool contains = list.Contains("asd1");
        }

        #endregion Methods
    }
}