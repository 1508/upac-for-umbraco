namespace Upac.Core.EditorControls.RichTextEditor
{
    using System;

    using umbraco.BusinessLogic;
    using umbraco.DataLayer;
    using umbraco.cms.businesslogic.datatype;
    using umbraco.editorControls.tinymce;
    using umbraco.interfaces;

    public class RichTextEditorDataType : BaseDataType, IDataType
    {
        #region Fields

        private IDataEditor _Editor;
        private IData _baseData;
        private IDataPrevalue _prevalueeditor;

        #endregion Fields

        #region Constructors

        public RichTextEditorDataType()
        {
        }

        #endregion Constructors

        #region Properties

        public override IData Data
        {
            get
            {
                if (this._baseData == null)
                {
                    this._baseData = new DefaultData(this);
                }
                return this._baseData;
            }
        }

        public override IDataEditor DataEditor
        {
            get
            {
                if (this._Editor == null)
                {
                    this._Editor = new RichTextEditorWebControl(this.Data, ((tinyMCEPreValueConfigurator)this.PrevalueEditor).Configuration);
                }
                return this._Editor;
            }
        }

        public override string DataTypeName
        {
            get
            {
                return "Upac Richtext editor";
            }
        }

        public override Guid Id
        {
            get
            {
                return new Guid("{72B23876-5705-4fed-83D9-F5A751888E5B}");
            }
        }

        // TODO Check to se if bug has been resolved
        public override IDataPrevalue PrevalueEditor
        {
            get
            {
                if (this._prevalueeditor == null)
                {

                    // This relates to a bug in umbraco.
                    // http://umbraco.codeplex.com/WorkItem/View.aspx?WorkItemId=25057
                    // new tinyMCEPreValueConfigurator(this); will throw a bug, when there is no database field [value] on the table cmsDataTypePreValues on the newly created datatypenodeid
                    string prevalue = Application.SqlHelper.ExecuteScalar<string>("select value from cmsDataTypePreValues where datatypenodeid = @datatypenodeid", new IParameter[] { Application.SqlHelper.CreateParameter("@datatypenodeid", DataTypeDefinitionId) });
                    // It's only the first time
                    if (string.IsNullOrEmpty(prevalue))
                    {
                        string copyPreValue = Application.SqlHelper.ExecuteScalar<string>("select value from cmsDataTypePreValues where datatypenodeid = @datatypenodeid", new IParameter[] { Application.SqlHelper.CreateParameter("@datatypenodeid", -87) });
                        if (!string.IsNullOrEmpty(copyPreValue))
                        {
                            string sqlInsert = "INSERT INTO [cmsDataTypePreValues] ([datatypenodeid], [value], [sortorder], [alias]) VALUES (@datatypenodeid, @value, 0, '')";
                            IParameter[] parameters = new IParameter[] { Application.SqlHelper.CreateParameter("@value", copyPreValue), Application.SqlHelper.CreateParameter("@datatypenodeid", DataTypeDefinitionId) };
                            Application.SqlHelper.ExecuteNonQuery(sqlInsert, parameters);
                        }
                    }

                    this._prevalueeditor = new tinyMCEPreValueConfigurator(this);
                }
                return this._prevalueeditor;
            }
        }

        #endregion Properties
    }
}