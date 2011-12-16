namespace Upac.Trac
{
    using System;

    using CookComputing.XmlRpc;

    [XmlRpcUrl("http://localhost/trac/login/xmlrpc")]
    public interface ITicket : IXmlRpcProxy
    {
        #region Methods

        //[XmlRpcMethod("ticket.getTicketFields")]
        //TicketField[] GetTicketFields();
        [XmlRpcMethod("ticket.create")]
        int Create(string summary, string description, XmlRpcStruct attributes);

        [XmlRpcMethod("ticket.delete")]
        void Delete(int id);

        [XmlRpcMethod("ticket.component.getAll")]
        string[] GetAllComponents();

        [XmlRpcMethod("ticket.milestone.getAll")]
        string[] GetAllMilestones();

        [XmlRpcMethod("ticket.priority.getAll")]
        string[] GetAllPriorities();

        [XmlRpcMethod("ticket.resolution.getAll")]
        string[] GetAllResolutions();

        [XmlRpcMethod("ticket.severity.getAll")]
        string[] GetAllSeverities();

        [XmlRpcMethod("ticket.type.getAll")]
        string[] GetAllTypes();

        [XmlRpcMethod("ticket.version.getAll")]
        string[] GetAllVersions();

        [XmlRpcMethod("ticket.getAvailableActions")]
        string[] GetAvailableActions(int id);

        [XmlRpcMethod("ticket.getRecentChanges")]
        int[] GetRecentChanges(DateTime since);

        [XmlRpcMethod("ticket.get")]
        object[] GetTicket(int id);

        [XmlRpcMethod("ticket.query")]
        int[] Query(string qstr);

        [XmlRpcMethod("ticket.update")]
        object[] Update(int id, string comment, XmlRpcStruct attributes);

        #endregion Methods
    }
}