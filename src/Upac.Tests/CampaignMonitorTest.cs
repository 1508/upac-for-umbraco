namespace Upac.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Upac.CampaignMonitor;

    using List = CampaignMonitorAPIWrapper.CampaignMonitorAPI.List;

    [TestFixture]
    public class CampaignMonitorTest
    {
        #region Methods

        [Test]
        public void TestAddSubscriberAndRemoveSubscriberFromList()
        {
            // First add
            Upac.CampaignMonitor.Api api = new Upac.CampaignMonitor.Api();
            List<List> lists = api.GetLists();
            foreach (List list in lists)
            {
                Console.WriteLine("Adding {0} to list {1}", "cpa@1508.dk", list.ListID);
                bool added = api.AddSubscriber("cpa@1508.dk", "Christian Palm", list.ListID);
                Assert.IsTrue(added);
            }

            // Then remove
            foreach (List list in lists)
            {
                Console.WriteLine("Removing {0} from list {1}", "cpa@1508.dk", list.ListID);
                bool removed = api.RemoveSubscriberFromList("cpa@1508.dk", list.ListID);
                Assert.IsTrue(removed);
            }
        }

        [Test]
        public void TestGetLists()
        {
            Upac.CampaignMonitor.Api api = new Upac.CampaignMonitor.Api();
            List<List> lists = api.GetLists();
            foreach (List list in lists)
            {
                Console.WriteLine("List id: {0}, list name: {1}", list.ListID, list.Name);
            }
        }

        [Test]
        public void TheBigTest()
        {
            string emailToTestWith = "cpa@1508.dk";

            Upac.CampaignMonitor.Api api = new Upac.CampaignMonitor.Api();
            List<List> lists = api.GetLists();

            Assert.GreaterOrEqual(lists.Count, 2);

            foreach (List list in lists)
            {
                Console.WriteLine("Adding {0} to list {1}", emailToTestWith, list.ListID);
                bool added = api.AddSubscriber(emailToTestWith, string.Empty, list.ListID);
                Assert.IsTrue(added);
            }

            List<string> subscriberLists = api.GetListsFromSubscriber(emailToTestWith);
            Assert.AreEqual(lists.Count, subscriberLists.Count);

            bool isSubscribed = api.IsSubscribed(emailToTestWith, lists[0].ListID);
            Assert.IsTrue(isSubscribed);

            api.RemoveSubscriberFromList(emailToTestWith, lists[0].ListID);
            subscriberLists = api.GetListsFromSubscriber(emailToTestWith);
            Assert.AreEqual(lists.Count - 1, subscriberLists.Count);

            isSubscribed = api.IsSubscribed(emailToTestWith, lists[1].ListID);
            Assert.IsTrue(isSubscribed);
        }

        #endregion Methods
    }
}