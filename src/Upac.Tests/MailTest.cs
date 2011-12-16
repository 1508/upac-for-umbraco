namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Upac.Core.Mail;
    using Upac.Core.Utilities;

    [TestFixture]
    public class MailTest
    {
        #region Methods

        [Test]
        public void TestMailUtilSendMail()
        {
            bool sendt = MailUtil.SendMail("cpa@1508.dk", "cpa@1508.dk", "Test", "Body", false);
            Assert.IsTrue(sendt);
        }

        [Test]
        public void TestMessage()
        {
            Message message = new Message();
            message.IncludeFooter = false;
            message.IncludeHeader = false;

            message.To.Add("cpa@1508.dk");
            message.From = "cpa@1508.dk";
            message.Subject = "Test";

            message.Body = "Hej <strong>$tester</strong>, mon det virker <ul>#foreach ($i in $foobar)<li>$i</li>#end</ul>";

            message.TemplateVariables.Add("tester", "Christian Palm");
            message.TemplateVariables.Add("foobar", new[] { "1", "2", "3" });

            bool sendt = message.Send();
            Assert.IsTrue(sendt);
        }

        #endregion Methods
    }
}