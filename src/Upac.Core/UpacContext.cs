namespace Upac.Core
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web;

    using Configuration;

    using Data;

    using Diagnostics;

    using umbraco.presentation;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Extensions;

    /// <summary>
    /// TODO skal kunne virke uden HttpContext.Current
    /// </summary>
    public class UpacContext
    {
        #region Fields

        // TODO skal denne ombøbes? - tjek kode bibel
        private const string ContextItemName = "UmbContext";

        #endregion Fields

        #region Constructors

        private UpacContext()
        {
            // Eager loading properties
            // Starter med forside node

            Node = Node.GetCurrent();
            Assert.EnsureNotNullReference(Node, "Node");

            NodeAxes axes = new NodeAxes(Node);

            HomeNode = axes.GetAncestorAtLevel(1);
            Assert.EnsureNotNullReference(HomeNode, "SiteRootNode");
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the current upac context.
        /// </summary>
        /// <remarks>Can not run without HttpContext.Current or UmbracoContext.Current</remarks>
        /// <value>The current UpacContext</value>
        public static UpacContext Current
        {
            get
            {
                if (HttpContext.Current.Items[ContextItemName] == null)
                {
                    Assert.EnsureNotNullReference(HttpContext.Current, "HttpContext.Current");
                    Assert.EnsureNotNullReference(UmbracoContext.Current, "UmbracoContext.Current");
                    HttpContext.Current.Items.Add(ContextItemName, new UpacContext());
                }
                return (UpacContext)HttpContext.Current.Items[ContextItemName];
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        public Node HomeNode
        {
            get;
            private set;
        }

        public Node Node
        {
            get;
            private set;
        }

        #endregion Properties
    }
}