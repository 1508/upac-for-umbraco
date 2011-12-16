namespace Upac.Syndication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using umbraco.cms.presentation.Trees;

    public class LoadUpacSyndication : BaseTree
    {
        #region Constructors

        public LoadUpacSyndication(string application)
            : base(application)
        {
        }

        #endregion Constructors

        #region Methods

        public override void Render(ref XmlTree tree)
        {
            Repository repository = new Repository();
            List<Entity> entities = repository.GetAll();
            foreach (Entity entity in entities)
            {
                XmlTreeNode xNode = XmlTreeNode.Create(this);
                xNode.NodeID = entity.Id.ToString();
                xNode.Text = entity.Name;
                xNode.Action = "javascript:openUpacSyndication(" + entity.Id + ");";
                xNode.Icon = "feed.png";
                xNode.OpenIcon = "feed.png";
                tree.Add(xNode);
            }
        }

        public override void RenderJS(ref StringBuilder Javascript)
        {
            Javascript.Append(
                    @"
                        function openUpacSyndication(id) {
                                parent.right.document.location.href = 'plugins/upac/Syndication/Edit.aspx?id=' + id;
                        }");
        }

        protected override void CreateRootNode(ref XmlTreeNode rootNode)
        {
            rootNode.Icon = FolderIcon;
            rootNode.OpenIcon = FolderIconOpen;
            rootNode.NodeType = "init" + TreeAlias;
            rootNode.NodeID = "init";
        }

        #endregion Methods
    }
}