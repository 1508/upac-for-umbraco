namespace Upac.Events.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class EventCollection : ConfigurationElementCollection
    {
        #region Properties

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        #endregion Properties

        #region Indexers

        public EventElement this[int index]
        {
            get
            {
                return (EventElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new EventElement this[string Key]
        {
            get
            {
                return (EventElement)BaseGet(Key);
            }
        }

        #endregion Indexers

        #region Methods

        public void Add(EventElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(EventElement element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(EventElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Key);
        }

        public void Remove(string key)
        {
            BaseRemove(key);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EventElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((EventElement)element).Key;
        }

        #endregion Methods
    }
}