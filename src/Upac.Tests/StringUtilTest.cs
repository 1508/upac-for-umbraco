namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    public class StringUtilTest
    {
        #region Methods

        [Test]
        public void TestClipping()
        {
            string input = "Der var engang en kat hvis navn boede for enden af en gang";
            string expected = "Der var engang...";
            string result = Upac.Core.Utilities.StringUtil.Clip(input, 17, true);
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 18, true);
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 19, true);
            expected = "Der var engang...";
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 17, false);
            expected = "Der var engang en";
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 18, false);
            expected = "Der var engang en";
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 20, false);
            expected = "Der var engang en";
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 21, false);
            expected = "Der var engang en kat";
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 21, false);
            expected = "Der var engang en kat";
            Assert.AreEqual(expected, result);

            result = Upac.Core.Utilities.StringUtil.Clip(input, 22, false);
            expected = "Der var engang en kat";
            Assert.AreEqual(expected, result);
        }

        #endregion Methods
    }
}