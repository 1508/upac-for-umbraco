namespace Upac.Dashboards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI.WebControls;

    using umbraco.cms.businesslogic;

    public class DictionaryListItems
    {
        #region Fields

        List<ListItem> items = new List<ListItem>();

        #endregion Fields

        #region Methods

        public List<ListItem> GetListItems()
        {
            Dictionary.DictionaryItem[] dictionaryItems = umbraco.cms.businesslogic.Dictionary.getTopMostItems;
            IOrderedEnumerable<Dictionary.DictionaryItem> orderedEnumerable = dictionaryItems.OrderBy(item => item.key);
            foreach (var item in orderedEnumerable)
            {
                items.Add(new ListItem(item.key, item.key));
                SetupDictionaryChildren(item, 1);
            }
            return items;
        }

        private void SetupDictionaryChildren(Dictionary.DictionaryItem item, int level)
        {
            Dictionary.DictionaryItem[] children = item.Children;
            IOrderedEnumerable<Dictionary.DictionaryItem> orderedEnumerable = children.OrderBy(c => c.key);
            if (children.Count() > 0)
            {
                foreach (Dictionary.DictionaryItem child in orderedEnumerable)
                {
                    string name = child.key;
                    for (int i = 0; i < level; i++)
                    {
                        name = "-" + name;
                    }
                    items.Add(new ListItem(name, child.key));
                    SetupDictionaryChildren(child, level + 1);
                }
            }
        }

        #endregion Methods
    }
}