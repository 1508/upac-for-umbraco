namespace Upac.CampaignMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using CampaignMonitorAPIWrapper;

    using log4net;

    using List = CampaignMonitorAPIWrapper.CampaignMonitorAPI.List;

    public class Api
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(typeof(Api));

        #endregion Fields

        #region Methods

        public bool AddSubscriber(string email, string name, string listId)
        {
            Assert.EnsureStringValue(email, "email");
            Assert.EnsureStringValue(listId, "listId");

            if (IsSubscribed(email, listId))
            {
                log.InfoFormat("No need to subscribe email: {0} - name: {1} to the campaignmonitor list {3} - the email was already in the list");
                return true;
            }

            Result<int> result = Subscriber.AddAndResubscribe(Settings.ApiKey, listId, email, name);
            if (result.Code == 0) //The subscription was successful.
            {
                log.InfoFormat("Successful subscribe email: {0} - name: {1} to the campaignmonitor list {3}");
                return true;
            }
            log.ErrorFormat("Could not subscribe email: {0} - name: {1} to the campaignmonitor list {3} error code: {4} error message: {5}", email, name, listId, result.Code, result.Message);
            return false;
        }

        public List<List> GetLists()
        {
            log.Debug("GetLists()");
            string cackeKey = string.Format(
                "CampaignMonitor.Api.GetLists()#apiKey:{0}#ApiClientId:{1}",
                Settings.ApiKey,
                Settings.ApiClientId);
            List<List> lists;

            if (CacheUtil.Exist(cackeKey))
            {
                log.Debug("Return lists from cache");
                lists = CacheUtil.Get<List<List>>(cackeKey);
            }
            else
            {
                Result<List<List>> result;
                try
                {
                    result = Client.GetLists(Settings.ApiKey, Settings.ApiClientId);
                }
                catch (Exception e)
                {
                    log.Error("GetLists()", e);
                    throw;
                }

                if (result.Code == 100 || result.Code == 100)
                {
                    string message = string.Format("GetLists() error! result.Code: {0} result.Message: {1}", result.Code, result.Message);
                    log.Error(message);
                    throw new Exception(message);
                }
                lists = result.ReturnObject;
                CacheUtil.Insert(cackeKey, lists, 5);
            }

            log.DebugFormat("GetLists() returned {0} lists", lists.Count);
            return lists;
        }

        public List<string> GetListsFromSubscriber(string email)
        {
            List<string> active = new List<string>();
            List<List> lists = GetLists();
            foreach (List list in lists)
            {
                if (IsSubscribed(email, list.ListID))
                {
                    active.Add(list.ListID);
                }
            }
            return active;
        }

        public bool IsSubscribed(string email, string listId)
        {
            Result<bool> result = Subscribers.GetIsSubscribed(Settings.ApiKey, listId, email);
            if (result.Code == 100 || result.Code == 101)
            {
                string message = string.Format("GetActiveListsOnSubscriber() error! result.Code: {0} result.Message: {1}", result.Code, result.Message);
                log.Error(message);
                throw new Exception(message);
            }
            return result.ReturnObject;
        }

        public void RemoveSubscriberFromAllLists(string email)
        {
            log.InfoFormat("Trying to unsubscribe email: {0} from all lists");
            List<string> lists = GetListsFromSubscriber(email);
            foreach (string listId in lists)
            {
                RemoveSubscriberFromList(email, listId);
            }
        }

        public bool RemoveSubscriberFromList(string email, string listId)
        {
            log.DebugFormat("Trying to unsubscribe email: {0} from the campaignmonitor list {1}", email, listId);
            //http://www.campaignmonitor.com/api/method/subscriber-unsubscribe/
            Result<int> result = CampaignMonitorAPIWrapper.Subscriber.Unsubscribe(Settings.ApiKey, listId, email);

            //0: Success
            if (result.Code == 0) //The unsubscription was successful.
            {
                log.InfoFormat("The unsubscription for {0} on campaignmonitor list {1} was successful", email, listId);
                return true;
            }
            if (result.Code == 202) //Already removed/Not in list
            {
                log.InfoFormat("The email {0} was already removed (not in list) on campaignmonitor list {1}", email, listId);
                return true;
            }
            log.ErrorFormat("Could not unsubscribe email: {0} - from the campaignmonitor list {1} error code: {2} error message: {3}", email, listId, result.Code, result.Message);
            return false;
        }

        #endregion Methods
    }
}