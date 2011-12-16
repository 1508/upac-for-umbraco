namespace Upac.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Upac.Core.Utilities;

    [TestFixture]
    public class VelocityUtilTest
    {
        #region Methods

        [Test]
        public void TestNode()
        {
            Hashtable hashtable = new Hashtable {
                { "foo", "Template" },
                { "bar", "is working" },
                { "foobar", new[] { "1", "2", "3" } }
            };

            string template = "$foo $bar: #foreach ($i in $foobar)$i#end";

            string output = VelocityUtil.Evaluate(template, hashtable);

            string expected = "Template is working: 123";

            Assert.AreEqual(expected, output);
        }

        #endregion Methods
    }
}