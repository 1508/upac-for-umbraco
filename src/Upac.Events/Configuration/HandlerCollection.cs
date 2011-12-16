namespace Upac.Events.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class HandlerCollection : ConfigurationElementCollection
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

        public HandlerElement this[int index]
        {
            get
            {
                return (HandlerElement)BaseGet(index);
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

        public new HandlerElement this[string Key]
        {
            get
            {
                return (HandlerElement)BaseGet(Key);
            }
        }

        #endregion Indexers

        #region Methods

        public void Add(HandlerElement element)
        {
            BaseAdd(element);
        }

        public void Clear()
        {
            BaseClear();
        }

        public int IndexOf(HandlerElement element)
        {
            return BaseIndexOf(element);
        }

        public void Remove(HandlerElement element)
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
            return new HandlerElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HandlerElement)element).Key;
        }

        #endregion Methods
    }
}