namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    public class CommonUtilTests
    {
        #region Methods

        [Test]
        public void TestGetValueFromNameValueCollection()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            nameValueCollection.Add("emptystringparam", string.Empty);
            nameValueCollection.Add("nullstringparam", null);

            string result = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("emptystringparam", "defaultvalue", nameValueCollection);
            Assert.AreEqual("defaultvalue", result);

            result = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("nullstringparam", "defaultvalue", nameValueCollection);
            Assert.AreEqual("defaultvalue", result);

            result = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("emptystringparam", string.Empty, nameValueCollection);
            Assert.AreEqual(string.Empty, result);
            result = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("nullparam", string.Empty, nameValueCollection);
            Assert.AreEqual(string.Empty, result);

            nameValueCollection.Add("stringparam", "mystring");
            result = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("stringparam", string.Empty, nameValueCollection);
            Assert.AreEqual("mystring", result);

            nameValueCollection.Add("intparam1", "1");
            nameValueCollection.Add("intparam2", "somevalue");
            nameValueCollection.Add("intparam3", string.Empty);

            int i = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("intparam1", -1, nameValueCollection);
            Assert.AreEqual(1, i);
            i = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("intparam1", 100, nameValueCollection);
            Assert.AreEqual(1, i);
            i = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("intparam2", -1, nameValueCollection);
            Assert.AreEqual(-1, i);
            i = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("intparam3", -1, nameValueCollection);
            Assert.AreEqual(-1, i);
            i = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("nullparam", -1, nameValueCollection);
            Assert.AreEqual(-1, i);

            nameValueCollection.Add("boolparam1", "1");
            nameValueCollection.Add("boolparam2", "0");
            nameValueCollection.Add("boolparam3", string.Empty);
            nameValueCollection.Add("boolparam4", "true");
            nameValueCollection.Add("boolparam5", "false");
            nameValueCollection.Add("boolparam6", "somevalue");
            bool b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam1", true, nameValueCollection);
            Assert.AreEqual(true, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam1", false, nameValueCollection);
            Assert.AreEqual(true, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam2", true, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam2", false, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam3", false, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam3", true, nameValueCollection);
            Assert.AreEqual(true, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("nullparam", false, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("nullparam", true, nameValueCollection);
            Assert.AreEqual(true, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam4", true, nameValueCollection);
            Assert.AreEqual(true, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam4", false, nameValueCollection);
            Assert.AreEqual(true, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam5", true, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam5", false, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam6", false, nameValueCollection);
            Assert.AreEqual(false, b);
            b = Upac.Core.Utilities.CommonUtil.GetValueFromNameValueCollection("boolparam6", true, nameValueCollection);
            Assert.AreEqual(true, b);
        }

        #endregion Methods
    }
}