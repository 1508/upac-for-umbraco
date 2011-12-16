namespace Upac.Core.EditorControls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class SettingsHelperDatatype : umbraco.cms.businesslogic.datatype.AbstractDataEditor
    {
        #region Fields

        private SettingsHelper settingsHelper = new SettingsHelper();

        #endregion Fields

        #region Constructors

        public SettingsHelperDatatype()
        {
            base.RenderControl = settingsHelper;
            settingsHelper.Init += new EventHandler(m_control_Init);
            base.DataEditorControl.OnSave += new umbraco.cms.businesslogic.datatype.AbstractDataEditorControl.SaveEventHandler(DataEditorControl_OnSave);
        }

        #endregion Constructors

        #region Properties

        public override string DataTypeName
        {
            get
            {
                return "Upac Settings Helper";
            }
        }

        public override Guid Id
        {
            get
            {
                return new Guid("{84289882-1FC2-446d-A044-5B8C5A22FD8D}");
            }
        }

        #endregion Properties

        #region Methods

        void DataEditorControl_OnSave(EventArgs e)
        {
            settingsHelper.Save();
        }

        void m_control_Init(object sender, EventArgs e)
        {
            settingsHelper.NodeId = ((umbraco.cms.businesslogic.datatype.DefaultData) base.Data).NodeId;
        }

        #endregion Methods
    }
}