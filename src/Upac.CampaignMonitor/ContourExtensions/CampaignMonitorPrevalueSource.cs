namespace Upac.CampaignMonitor.ContourExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using CampaignMonitorAPIWrapper.CampaignMonitorAPI;

    using Umbraco.Forms.Core;

    public class CampaignMonitorPrevalueSource : Umbraco.Forms.Core.FieldPreValueSourceType
    {
        #region Constructors

        public CampaignMonitorPrevalueSource()
        {
            base.Id = new Guid("{13FCB029-E301-49b3-97BF-CA1D7876271B}");
            base.Name = "Campaign Monitor Lists";
            base.Description = "Uses Campaign Monitor Lists as prevalues";
        }

        #endregion Constructors

        #region Properties

        [Umbraco.Forms.Core.Attributes.Setting("Lists to include", description = "The selected lists will be prevalues", control = "Upac.CampaignMonitor.ContourExtensions.ListsFieldSetting", assembly = "Upac.CampaignMonitor")]
        public string ListsToInclude
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public override List<PreValue> GetPreValues(Field field)
        {
            List<PreValue> list = new List<PreValue>();
            string[] selectedLists = ListsToInclude.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            Api api = new Api();
            List<List> lists = api.GetLists();

            foreach (string selectedList in selectedLists)
            {
                List mailingList = lists.SingleOrDefault(l => l.ListID == selectedList);
                if (mailingList != null)
                {
                    PreValue pv = new PreValue();
                    pv.Id = mailingList.ListID;
                    pv.Value = mailingList.Name;
                    if (field != null)
                    {
                        pv.Field = field.Id;
                    }
                    list.Add(pv);
                }
            }
            return list;
        }

        public override List<Exception> ValidateSettings()
        {
            List<Exception> exs = new List<Exception>();
            return exs;
        }

        #endregion Methods
    }
}