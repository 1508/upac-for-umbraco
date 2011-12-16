namespace Upac.Trac
{
    using System;

    public partial class TracDashboard : System.Web.UI.UserControl
    {
        #region Methods

        protected void CreateWikiPage(object sender, EventArgs e)
        {
            string password = tbPassword.Text;
            if (tbPassword.Visible == false)
            {
                password = Session["tracPassword"].ToString();
            }

            TicketManager manager = new TicketManager();
            manager.Connect("http://trac/projects/int900.0010upac/login/xmlrpc", tbUsername.Text, password);

            TicketInfo info = new TicketInfo();
            info.Summary = tbSummery.Text;
            info.Description = tbContent.Text;
            info.Milestone = ddlMilestone.SelectedValue;
            info.Type = "Projektopgave";
            info.Priority = "major";
            info.Component = "Core";
            info.EstimatedNumberOfHours = tbEstimatedNumberOfHours.Text;
            manager.CreateTicket(info);

            //tbPassword.Text = string.Empty;
            tbSummery.Text = string.Empty;
            tbContent.Text = string.Empty;

            ltrMessage.Text = "Ticket created";
        }

        protected void DeleteSessionPassword(object sender, EventArgs e)
        {
            Session["tracPassword"] = null;
            tbPassword.Visible = true;
            btSetSessionPassword.Visible = true;
            btDeleteSessionPassword.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrMessage.Text = string.Empty;
        }

        protected void SetSessionPassword(object sender, EventArgs e)
        {
            Session["tracPassword"] = tbPassword.Text;
            tbPassword.Visible = false;
            btSetSessionPassword.Visible = false;
            btDeleteSessionPassword.Visible = true;
        }

        #endregion Methods
    }
}