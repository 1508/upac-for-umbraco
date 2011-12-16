namespace Upac.Dashboards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using umbraco.cms.businesslogic;
    using umbraco.DataLayer;

    public partial class PropertyNameChanger : System.Web.UI.UserControl
    {
        #region Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetupPropertyNames();
                SetupDictionary();
            }
        }

        protected void Save(object sender, EventArgs e)
        {
            string oldName = ddlFind.SelectedValue;
            string newName = string.Format("#{0}", ddlDictionary.SelectedValue);

            if (!string.IsNullOrEmpty(oldName))
            {

                string sql = @"
                            UPDATE
                                [cmsPropertyType]
                            SET
                                [Name] = @NewName
                            WHERE
                                [Name] = @OldName
                ";

                ISqlHelper helper = umbraco.BusinessLogic.Application.SqlHelper;
                int rowsAffected = helper.ExecuteNonQuery(sql,
                                                   new[]
                                                       {
                                                           helper.CreateParameter("@NewName", newName),
                                                           helper.CreateParameter("@OldName", oldName)
                                                       });
                ltrRowsAffected.Text = rowsAffected.ToString();
                SetupPropertyNames();
            }
        }

        private void SetupDictionary()
        {
            DictionaryListItems helper = new DictionaryListItems();
            ddlDictionary.Items.AddRange(helper.GetListItems().ToArray());
            ddlDictionary.DataBind();
        }

        private void SetupPropertyNames()
        {
            string sql = @"
                                SELECT
                                    DISTINCT([Name]),
                                    (
                                        SELECT
                                            COUNT([Name])
                                        FROM
                                            [cmsPropertyType]
                                        WHERE
                                            [Name] = [Main].[Name]
                                    ) AS [Count]
                                FROM
                                    [cmsPropertyType] [Main]
                                WHERE
                                    [Name] NOT LIKE '#%'
                                ORDER BY
                                    [Name]

                                ";
            IRecordsReader reader = umbraco.BusinessLogic.Application.SqlHelper.ExecuteReader(sql, new IParameter[0]);
            ddlFind.Items.Clear();
            ddlFind.Items.Add(new ListItem(string.Empty, string.Empty));
            while (reader.Read())
            {

                string name = reader.GetString("Name");
                int count = reader.GetInt("Count");
                ListItem item = new ListItem();
                item.Text = string.Format("{0} (on {1} properties)", name, count);
                item.Value = name;
                ddlFind.Items.Add(item);
            }
            reader.Dispose();
            ddlFind.DataBind();
        }

        #endregion Methods
    }
}