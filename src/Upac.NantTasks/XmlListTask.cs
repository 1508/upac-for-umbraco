#region Header

// Serge van den Oever (serge@macaw.nl)
// Based on idea from weblog entry: http://blogs.geekdojo.net/rcase/archive/2005/01/06/5971.aspx combined with the code of xmlpeek.
// Feedback by Matt (http://weblogs.asp.net/soever/archive/2005/05/08/406101.aspx)
// Extended by Jonni Faiga [december 1, 2006]
// Publication of this source in weblog entry:

#endregion Header

namespace Macaw
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;

    using NAnt.Core;
    using NAnt.Core.Attributes;
    using NAnt.Core.Types;

    /// <summary>
    /// Extracts text from an XML file at the locations specified by an XPath 
    /// expression, and return those texts separated by a delimiter string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the XPath expression specifies multiple nodes the node are seperated
    /// by the delimiter string, if no nodes are matched, an empty string is returned.
    /// </para>
    /// </remarks>
    /// <example>
    ///   <para>
    ///   The example provided assumes that the following XML file (xmllisttest.xml)
    ///   exists in the current build directory.
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    ///	<?xml version="1.0" encoding="utf-8" ?> 
    /// <xmllisttest>
    /// <firstnode attrib="attrib1">node1</firstnode>
    /// <secondnode attrib="attrib2">
    /// <subnode attrib="attribone">one</subnode>
    /// <subnode attrib="attribtwo">two</subnode>
    /// <subnode attrib="attribthree">three</subnode>
    /// <subnode attrib="attribtwo">two</subnode>
    /// </secondnode>
    /// <thirdnode xmlns="http://thirdnodenamespace">namespacednode</thirdnode>
    /// <fourthnode>${myproperty}</fourthnode>
    /// <fifthnode>${myproperty=='Hi'}</fifthnode>	
    /// </xmllisttest>	
    ///		]]>
    ///   </code>
    /// </example>
    /// <example>
    ///   <para>
    ///   The example reads numerous values from this file:
    ///   </para>
    ///   <code>
    ///     <![CDATA[
    /// <?xml version="1.0" encoding="utf-8" ?> 
    /// <project name="tests.build" default="test" basedir=".">
    /// 	<target name="test">
    /// 		<!-- TEST1: node exists, is single node, get value -->
    /// 		<xmllist file="xmllisttest.xml" property="prop1" delim="," xpath="/xmllisttest/firstnode"/>    
    /// 		<echo message="prop1=${prop1}"/>
    /// 		<fail message="TEST1: Expected: prop1=node1" unless="${prop1 == 'node1'}"/>
    /// 		
    /// 		<!-- TEST2: node does not exist -->
    /// 		<xmllist file="xmllisttest.xml" property="prop2" delim="," xpath="/xmllisttest/nonexistantnode" />    
    /// 		<echo message="prop2='${prop2}'"/>
    /// 		<fail message="TEST2: Expected: prop2='empty'" unless="${prop2 == ''}"/>
    /// 	
    /// 		<!-- TEST3: node exists, get attribute value -->
    /// 		<xmllist file="xmllisttest.xml" property="prop3" delim="," xpath="/xmllisttest/firstnode/@attrib" />    
    /// 		<echo message="prop3=${prop3}"/>
    /// 		<fail message="TEST3: Expected: prop3=attrib1" unless="${prop3 == 'attrib1'}"/>
    /// 	
    /// 		<!-- TEST4: nodes exists, get multiple values -->
    /// 		<xmllist file="xmllisttest.xml" property="prop5" delim="," xpath="/xmllisttest/secondnode/subnode" />    
    /// 		<echo message="prop5=${prop5}"/>
    /// 		<fail message="TEST4: Expected: prop5=one,two,three,two" unless="${prop5 == 'one,two,three,two'}"/>
    /// 	
    /// 		<!-- TEST5: nodes exists, get multiple attribute values -->
    /// 		<xmllist file="xmllisttest.xml" property="prop5" delim="," xpath="/xmllisttest/secondnode/subnode/@attrib" />    
    /// 		<echo message="prop5=${prop5}"/>
    /// 		<fail message="TEST5: Expected: prop5=attribone,attribtwo,attribthree,attribtwo" unless="${prop5 == 'attribone,attribtwo,attribthree,attribtwo'}"/>
    /// 	
    /// 		<!-- TEST6: nodes exists, get multiple values, but only unique values -->
    /// 		<xmllist file="xmllisttest.xml" property="prop6" delim="," xpath="/xmllisttest/secondnode/subnode" unique="true"/>    
    /// 		<echo message="prop6=${prop6}"/>
    /// 		<fail message="TEST6: Expected: prop6=one,two,three" unless="${prop6 == 'one,two,three'}"/>
    /// 	
    /// 		<!-- TEST7: nodes exists, get multiple attribute values -->
    /// 		<xmllist file="xmllisttest.xml" property="prop7" delim="," xpath="/xmllisttest/secondnode/subnode/@attrib" unique="true"/>    
    /// 		<echo message="prop7=${prop7}"/>
    /// 		<fail message="TEST7: Expected: prop7=attribone,attribtwo,attribthree" unless="${prop7 == 'attribone,attribtwo,attribthree'}"/>
    /// 		
    /// 		<!-- TEST8: node exists, is single node, has namespace http://thirdnodenamespace, get value -->
    /// 		<xmllist file="xmllisttest.xml" property="prop8" delim="," xpath="/xmllisttest/x:thirdnode">    
    /// 			<namespaces>
    /// 				<namespace prefix="x" uri="http://thirdnodenamespace" />
    /// 			</namespaces>
    /// 		</xmllist>
    /// 		<echo message="prop8=${prop8}"/>
    /// 		<fail message="TEST8: Expected: prop8=namespacednode" unless="${prop8 == 'namespacednode'}"/>
    /// 
    /// 		<!-- TEST9: node exists, is single node, get value expanded via current nant properties-->
    /// 		<property name="myproperty" value="Hi"/>
    /// 		<xmllist file="xmllisttest.xml" property="prop9" delim="," xpath="/xmllisttest/fourthnode"/>
    /// 		<echo message="prop9=${prop9}"/>
    /// 		<fail message="TEST9: Expected: prop1=${myproperty}" unless="${prop9 == myproperty}"/>
    /// 
    /// 		<!-- TEST10: node exists, is single node, get value expanded via current nant function-->
    /// 		<xmllist file="xmllisttest.xml" property="prop10" delim="," xpath="/xmllisttest/fifthnode"/>
    /// 		<echo message="prop10=${prop10}"/>
    /// 		<fail message="TEST10: Expected: prop10=True" unless="${prop10 == 'True'}"/>
    /// 	</target>
    /// </project>
    ///		]]>
    ///   </code>
    ///   Result when you run this code:
    ///   <code>
    ///		<![CDATA[
    /// 	test:
    /// 
    /// 	[echo] prop1="node1"
    /// 	[echo] prop2="''"
    /// 	[echo] prop3="attrib1"
    /// 	[echo] prop5="one,two,three,two"
    /// 	[echo] prop5="attribone,attribtwo,attribthree,attribtwo"
    /// 	[echo] prop6="one,two,three"
    /// 	[echo] prop7="attribone,attribtwo,attribthree"
    /// 	[echo] prop8="namespacednode"
    /// 	[echo] prop9="Hi"
    /// 	[echo] prop10="True"
    /// 
    /// 	BUILD SUCCEEDED
    ///		]]
    ///   </code>
    /// </example>
    [TaskName("xmllist")]
    public class XmlListTask : Task
    {
        #region Fields

        private string _delimiter = ",";
        private bool _expandProps = true;
        private XmlNamespaceCollection _namespaces = new XmlNamespaceCollection();
        private string _property;
        private bool _unique = false; // assume we return all values
        private FileInfo _xmlFile;
        private string _xPath;

        #endregion Fields

        #region Properties

        /// <summary>
        /// The delimiter string.
        /// </summary>
        [TaskAttribute("delim", Required = false)]
        [StringValidator(AllowEmpty = false)]
        public string Delimiter
        {
            get
            {
                return _delimiter;
            }
            set
            {
                _delimiter = value;
            }
        }

        /// <summary>
        /// If true, the any nant-style properties on the result will be
        /// expanded before returning. Default is true.
        /// </summary>
        [TaskAttribute("expandprops")]
        [BooleanValidator]
        public bool ExpandProperties
        {
            get { return _expandProps; }
            set { _expandProps = value; }
        }

        /// <summary>
        /// Namespace definitions to resolve prefixes in the XPath expression.
        /// </summary>
        [BuildElementCollection("namespaces", "namespace")]
        public XmlNamespaceCollection Namespaces
        {
            get
            {
                return _namespaces;
            }
            set
            {
                _namespaces = value;
            }
        }

        /// <summary>
        /// The property that receives the text representation of the XML inside 
        /// the nodes returned from the XPath expression, seperated by the specified delimiter.
        /// </summary>
        [TaskAttribute("property", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Property
        {
            get
            {
                return _property;
            }
            set
            {
                _property = value;
            }
        }

        /// <summary>
        /// If unique, no duplicate vaslues are returned. By default unique is false and all values are returned.
        /// </summary>
        [TaskAttribute("unique", Required = false)]
        [BooleanValidator]
        public bool Unique
        {
            get
            {
                return _unique;
            }
            set
            {
                _unique = value;
            }
        }

        /// <summary>
        /// The name of the file that contains the XML document
        /// that is going to be interrogated.
        /// </summary>
        [TaskAttribute("file", Required = true)]
        public FileInfo XmlFile
        {
            get
            {
                return _xmlFile;
            }
            set
            {
                _xmlFile = value;
            }
        }

        /// <summary>
        /// The XPath expression used to select which nodes to read.
        /// </summary>
        [TaskAttribute("xpath", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string XPath
        {
            get
            {
                return _xPath;
            }
            set
            {
                _xPath = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Executes the XML reading task.
        /// </summary>
        protected override void ExecuteTask()
        {
            Log(Level.Verbose, "Looking at '{0}' with XPath expression '{1}'.",
                XmlFile.FullName, XPath);

            // ensure the specified xml file exists
            if (!XmlFile.Exists)
            {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "The XML file '{0}' does not exist.", XmlFile.FullName), Location);
            }
            try
            {
                XmlDocument document = LoadDocument(XmlFile.FullName);
                Properties[Property] = ExpandProps(GetNodeContents(XPath, document));
            }
            catch (BuildException ex)
            {
                throw ex; // Just re-throw the build exceptions.
            }
            catch (Exception ex)
            {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Retrieving the information from '{0}' failed.", XmlFile.FullName),
                    Location, ex);
            }
        }

        /// <summary>
        /// Expands project properties in the string
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string ExpandProps(string result)
        {
            if (Properties == null || !ExpandProperties)
            {
                return result;
            }
            return Properties.ExpandProperties(result, null);
        }

        /// <summary>
        /// Gets the contents of the list of nodes specified by the XPath expression.
        /// </summary>
        /// <param name="xpath">The XPath expression used to determine the nodes.</param>
        /// <param name="document">The XML document to select the nodes from.</param>
        /// <returns>
        /// The contents of the nodes specified by the XPath expression, delimited by 
        /// the delimiter string.
        /// </returns>
        private string GetNodeContents(string xpath, XmlDocument document)
        {
            XmlNodeList nodes;

            try
            {
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(document.NameTable);
                foreach (XmlNamespace xmlNamespace in Namespaces)
                {
                    if (xmlNamespace.IfDefined && !xmlNamespace.UnlessDefined)
                    {
                        nsMgr.AddNamespace(xmlNamespace.Prefix, xmlNamespace.Uri);
                    }
                }
                nodes = document.SelectNodes(xpath, nsMgr);
            }
            catch (Exception ex)
            {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Failed to execute the xpath expression {0}.", xpath),
                    Location, ex);
            }

            Log(Level.Verbose, "Found '{0}' nodes with the XPath expression '{1}'.",
                nodes.Count, xpath);

            // collect all strings in a string collection, skip duplications if Unique is true
            StringCollection texts = new StringCollection();
            foreach (XmlNode node in nodes)
            {
                string text = node.InnerText;
                if (!Unique || !texts.Contains(text))
                {
                    texts.Add(text);
                }
            }

            // Concatenate the strings in the string collection to a single string, delimited by Delimiter
            StringBuilder builder = new StringBuilder();
            foreach (string text in texts)
            {
                if (builder.Length > 0)
                {
                    builder.Append(Delimiter);
                }
                builder.Append(text);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Loads an XML document from a file on disk.
        /// </summary>
        /// <param name="fileName">The file name of the file to load the XML document from.</param>
        /// <returns>
        /// A <see cref="XmlDocument">document</see> containing
        /// the document object representing the file.
        /// </returns>
        private XmlDocument LoadDocument(string fileName)
        {
            XmlDocument document = null;

            try
            {
                document = new XmlDocument();
                document.Load(fileName);
                return document;
            }
            catch (Exception ex)
            {
                throw new BuildException(string.Format(CultureInfo.InvariantCulture,
                    "Can't load XML file '{0}'.", fileName), Location,
                    ex);
            }
        }

        #endregion Methods
    }
}