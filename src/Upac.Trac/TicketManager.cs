namespace Upac.Trac
{
    using System;
    using System.Net;

    using CookComputing.XmlRpc;

    public class TicketManager
    {
        #region Fields

        private ITicket _ticket;

        #endregion Fields

        #region Methods

        public void Connect(string url, string userName, string password)
        {
            _ticket = XmlRpcProxyGen.Create<ITicket>();
            _ticket.Url = url;
            _ticket.PreAuthenticate = true;
            _ticket.Credentials = new NetworkCredential(userName, password);
        }

        public void CreateTicket(TicketInfo ticket)
        {
            if (string.IsNullOrEmpty(ticket.Summary)) throw new ArgumentNullException("ticket.Summary");

            if (string.IsNullOrEmpty(ticket.Description)) throw new ArgumentNullException("ticket.Description");

            if (string.IsNullOrEmpty(ticket.Type)) throw new ArgumentNullException("ticket.Type");

            if (string.IsNullOrEmpty(ticket.Priority)) throw new ArgumentNullException("ticket.Priority");

            if (string.IsNullOrEmpty(ticket.Component)) throw new ArgumentNullException("ticket.Component");

            XmlRpcStruct tempAttributes = new XmlRpcStruct();

            foreach (object key in ticket.Attributes.Keys)
            {

                if ((((string)key) != TicketAttributes.Description) && (((string)key) != TicketAttributes.Summary))
                {

                    tempAttributes.Add(key, ticket.Attributes[key]);

                }

            }

            int id = _ticket.Create(ticket.Summary, ticket.Description, ticket.Attributes);

            ticket.TicketId = id;
        }

        public void DeleteTicket(int ticketId)
        {
            _ticket.Delete(ticketId);
        }

        public void DeleteTicket(TicketInfo ticket)
        {
            ValidateTicket(ticket);

            DeleteTicket(ticket.TicketId);
        }

        public string[] GetAvailableActions(int id)
        {
            return _ticket.GetAvailableActions(id);
        }

        public string[] GetAvailableActions(TicketInfo ticket)
        {
            ValidateTicket(ticket);
            return _ticket.GetAvailableActions(ticket.TicketId);
        }

        public int[] GetRecentChanges(DateTime since)
        {
            return _ticket.GetRecentChanges(since);
        }

        public void UpdateTicket(TicketInfo ticket, string comment)
        {
            ValidateTicket(ticket);

            object[] values = _ticket.Update(ticket.TicketId, comment, ticket.Attributes);

            ticket.Update(values);
        }

        private void ValidateTicket(TicketInfo ticket)
        {
            if (ticket == null) throw new ArgumentNullException("ticket");

            if (ticket.TicketId <= 0) throw new ArgumentException("ticketId must be greater then 0");
        }

        #endregion Methods
    }
}