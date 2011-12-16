namespace Upac.Core.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.XPath;

    using umbraco.presentation.nodeFactory;

    using Upac.Core.Extensions;
    using Upac.Core.Utilities;

    /// <summary>
    /// Provides a simple way to get ancestors and children
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// NodeAxes axes = new NodeAxes();
    /// ]]>
    /// </example>
    /// <doc>
    ///		<headline>Here is some doc</headline>
    ///		<text>Here is some text
    /// New line</text>
    ///		<code>Here is some code</code>
    ///		<para>Some para</para>
    /// </doc>
    public class NodeAxes
    {
        #region Fields

        private List<Node> ancestors;
        private List<Node> children;
        private int[] levels;
        private Node node;

        #endregion Fields

        #region Constructors

        public NodeAxes(Node node)
        {
            this.node = node;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets all ancestors.
        /// </summary>
        /// <returns>The ancestors of the node</returns>
        /// <remarks>Using lazyloading</remarks>
        public List<Node> Ancestors
        {
            get
            {
                if (ancestors == null)
                {
                    int[] levels = node.Levels();
                    ancestors = new List<Node>(levels.Length);
                    foreach (int nodeId in levels)
                    {
                        Node ancestor = new Node(nodeId);
                        ancestors.Add(ancestor);
                    }
                    //list.Reverse();
                }
                return ancestors;
            }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public List<Node> Children
        {
            get
            {
                if (children == null)
                {
                    Nodes nodes = node.Children;
                    children = new List<Node>(nodes.Count);
                    foreach (Node child in nodes)
                    {
                        children.Add(child);
                    }
                }
                return children;
            }
        }

        public int[] Levels
        {
            get
            {
                if (levels == null)
                {
                    levels = node.Levels();
                }
                return levels;
            }
        }

        #endregion Properties

        #region Methods

        public Node GetAncestorAtLevel(int level)
        {
            if (level > Levels.Length)
            {
                throw new ArgumentOutOfRangeException("level", "level is to high");
            }
            int nodeId = Levels[level];
            Node nodeAtLevel = UmbracoUtil.GetNode(nodeId);
            return nodeAtLevel;
        }

        public Node GetChild(string nodeName)
        {
            foreach (Node node in Children)
            {
                if (node.Name.ToLowerInvariant() == nodeName.ToLowerInvariant())
                {
                    return node;
                }
            }
            return null;
        }

        #endregion Methods
    }
}