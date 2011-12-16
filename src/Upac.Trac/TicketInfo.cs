namespace Upac.Trac
{
    using System;

    using CookComputing.XmlRpc;

    public class TicketInfo
    {
        #region Fields

        private XmlRpcStruct _attributes;
        private DateTime _created;
        private DateTime _lastModified;
        private int _ticketId;

        #endregion Fields

        #region Constructors

        public TicketInfo()
        {
        }

        public TicketInfo(object[] values)
        {
            Update(values);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The attributes for this ticket, this will include any additional fields
        /// that aren't defined explicitly as members of this class.
        /// </summary>
        public XmlRpcStruct Attributes
        {
            get
            {
                if (_attributes == null) _attributes = new XmlRpcStruct();

                return _attributes;
            }

            set { _attributes = value; }
        }

        public string Cc
        {
            get { return GetAttribute(TicketAttributes.Cc); }

            set { SetAttribute(TicketAttributes.Cc, value); }
        }

        public string Component
        {
            get { return GetAttribute(TicketAttributes.Component); }

            set { SetAttribute(TicketAttributes.Component, value); }
        }

        /// <summary>
        /// date and time the ticket was created
        /// </summary>
        public DateTime Created
        {
            get { return _created; }

            set { _created = value; }
        }

        public string Description
        {
            get { return GetAttribute(TicketAttributes.Description); }

            set { SetAttribute(TicketAttributes.Description, value); }
        }

        public string EstimatedNumberOfHours
        {
            get { return GetAttribute(TicketAttributes.EstimatedNumberOfHours); }

            set { SetAttribute(TicketAttributes.EstimatedNumberOfHours, value); }
        }

        public string Keywords
        {
            get { return GetAttribute(TicketAttributes.Keywords); }

            set { SetAttribute(TicketAttributes.Keywords, value); }
        }

        /// <summary>
        /// date and time the ticket was last modified
        /// </summary>
        public DateTime LastModified
        {
            get { return _lastModified; }

            set { _lastModified = value; }
        }

        public string Milestone
        {
            get { return GetAttribute(TicketAttributes.Milestone); }

            set { SetAttribute(TicketAttributes.Milestone, value); }
        }

        public string Owner
        {
            get { return GetAttribute(TicketAttributes.Owner); }

            set { SetAttribute(TicketAttributes.Owner, value); }
        }

        public string Priority
        {
            get { return GetAttribute(TicketAttributes.Priority); }

            set { SetAttribute(TicketAttributes.Priority, value); }
        }

        public string Reporter
        {
            get { return GetAttribute(TicketAttributes.Reporter); }

            set { SetAttribute(TicketAttributes.Reporter, value); }
        }

        public string Resolution
        {
            get { return GetAttribute(TicketAttributes.Resolution); }

            set { SetAttribute(TicketAttributes.Resolution, value); }
        }

        public string Status
        {
            get { return GetAttribute(TicketAttributes.Status); }

            set { SetAttribute(TicketAttributes.Status, value); }
        }

        public string Summary
        {
            get { return GetAttribute(TicketAttributes.Summary); }

            set { SetAttribute(TicketAttributes.Summary, value); }
        }

        /// <summary>
        /// The identifier for this ticket
        /// </summary>
        public int TicketId
        {
            get { return _ticketId; }

            set { _ticketId = value; }
        }

        public string Type
        {
            get { return GetAttribute(TicketAttributes.Type); }

            set { SetAttribute(TicketAttributes.Type, value); }
        }

        public string Version
        {
            get { return GetAttribute(TicketAttributes.Version); }

            set { SetAttribute(TicketAttributes.Version, value); }
        }

        #endregion Properties

        #region Methods

        internal void Update(object[] values)
        {
            if (values == null) throw new ArgumentNullException("values");

            if (values.Length != 4) throw new ArgumentException("values should have 4 elements");

            _ticketId = (int)values[0];

            _created = DateHelper.ParseUnixTimestamp((int)values[1]);

            _lastModified = DateHelper.ParseUnixTimestamp((int)values[2]);

            _attributes = (XmlRpcStruct)values[3];
        }

        private string GetAttribute(string name)
        {
            if (Attributes.Contains(name))
            {
                return Convert.ToString(Attributes[name]);
            }

            return null;
        }

        private void SetAttribute(string name, string value)
        {
            if (Attributes.Contains(name))
            {
                Attributes[name] = value;
            }

            else
            {
                Attributes.Add(name, value);
            }
        }

        #endregion Methods
    }
}