namespace Upac.Core.Webcontrols
{
    using System.Web.UI;

    using log4net;

    using Upac.Core.Utilities;

    /// <summary>
    /// OEmbed webcontrol
    /// </summary>
    [ToolboxData("<{0}:OEmbed runat=server></{0}:Setting>")]
    public class OEmbed : Control
    {
        #region Fields

        /// <summary>
        /// The log4net logger
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(OEmbed));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the height of the max.
        /// </summary>
        /// <value>The height of the max.</value>
        public string MaxHeight
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the width of the max.
        /// </summary>
        /// <value>The width of the max.</value>
        public string MaxWidth
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source
        {
            get; set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Renders the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        protected override void Render(HtmlTextWriter output)
        {
            Log.Debug("Render(HtmlTextWriter output)");
            Log.DebugFormat("Source: {0}", this.Source);
            output.Write(OEmbedUtil.GetOEmbedHtml(this.Source, this.MaxWidth, this.MaxHeight));
        }

        #endregion Methods
    }
}