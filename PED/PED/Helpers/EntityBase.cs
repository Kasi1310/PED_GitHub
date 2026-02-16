using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WiseX.Helpers
{
    public class EntityBase
    {
        [ColumnAttribute(Exclude = true)]
        public const string DefaultView = "DEFAULT_VIEW";

        Dictionary<string, object> _customData;

        public EntityBase()
        {
            this.View = EntityBase.DefaultView;
            this._customData = new System.Collections.Generic.Dictionary<string, object>();
        }
        [ColumnAttribute(Exclude = true)]
        public string View { get; set; }

        [XmlIgnore()]
        public System.Collections.Generic.Dictionary<string, object> CustomData
        {
            get { return this._customData; }
        }

        public virtual string GetXml()
        {
            try
            {
                return this.GetXml(this.View);
            }
            catch { throw; }
        }

        public virtual string GetXml(string view)
        {
            try
            {
                switch (view)
                {
                    case EntityBase.DefaultView:
                        return this.GetDefaultViewXml();
                    default:
                        return string.Empty;
                }
            }
            catch { throw; }
        }

        protected string GetDefaultViewXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                StringBuilder xmlData = new StringBuilder();

                // To omit xml declaration.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;

                // To omit namespace.
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (XmlWriter writer = XmlWriter.Create(xmlData, settings))
                {
                    serializer.Serialize(writer, this, namespaces);
                    writer.Close();
                }

                return xmlData.ToString();
            }
            catch { throw; }
        }

        public virtual string GetXml(bool includeCustomData)
        {
            try
            {
                string entityXml = this.GetXml();

                // Open in xml document.
                XmlDocument document = new XmlDocument();
                document.LoadXml(entityXml);

                foreach (KeyValuePair<string, object> item in this.CustomData)
                {
                    // Create an element and append as child.
                    XmlNode element = document.CreateElement(item.Key);
                    if (item.Value != null)
                        element.InnerText = item.Value.ToString();

                    document.FirstChild.AppendChild(element);
                }

                // To preserve formatting.
                StringBuilder xmlData = new StringBuilder();

                // To omit xml declaration.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(xmlData, settings))
                {
                    document.WriteTo(writer);
                    writer.Close();
                }

                return xmlData.ToString();
            }
            catch { throw; }
        }


        public static string GetXml<E>(List<E> entities)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<E>), new XmlRootAttribute(string.Format("{0}List", typeof(E).Name)));
                StringBuilder xmlData = new StringBuilder();

                // To omit xml declaration.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                // To omit namespace.
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (XmlWriter writer = XmlWriter.Create(xmlData, settings))
                {
                    serializer.Serialize(writer, entities, namespaces);
                    writer.Close();
                }

                return xmlData.ToString();
            }
            catch { throw; }
        }

    }
}
