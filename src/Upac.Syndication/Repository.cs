namespace Upac.Syndication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    public class Repository
    {
        #region Fields

        const string Filepath = @"D:\Solutions\INT 900\Upac\Latest\Source\Upac.Syndication\UpacSyndicationData.xml";

        private readonly XElement _rootElement;

        #endregion Fields

        #region Constructors

        public Repository()
        {
            _rootElement = XElement.Load(Filepath);
        }

        #endregion Constructors

        #region Methods

        public void Delete(Entity entity)
        {
        }

        public List<Entity> GetAll()
        {
            List<Entity> entities = (from xml in _rootElement.Elements("Entry")
                                                                select new Entity
                                                                    {
                                                                        Id = new Guid(xml.Element("Id").Value),
                                                                        Name = xml.Element("Name").Value
                                                                    }).ToList();
            return entities;
        }

        public void Insert(Entity entity)
        {
        }

        public void Update(Entity entity)
        {
        }

        #endregion Methods
    }
}