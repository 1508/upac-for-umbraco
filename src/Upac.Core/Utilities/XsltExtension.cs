namespace Upac.Core.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml;
    using System.Xml.XPath;

    using umbraco.cms.businesslogic.media;
    using umbraco.presentation.nodeFactory;

    using Upac.Core.Data;
    using Upac.Core.Extensions;

    public class XsltExtension
    {
        #region Methods

        /// <summary>
        /// Clips the specified text if the text is bigger than the maxLength provided.
        /// </summary>
        /// <param name="text">The text to be clipped.</param>
        /// <param name="maxLength">Max length of the returned string.</param>
        /// <param name="ellipsis">if set to <c>true</c> show ellipsis (...)</param>
        /// <returns>A clipped string</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:Clip('Der var engang en kat hvis navn boede for enden af en gang', 17, true())"><br />
        /// Would give: Der var engang...<br />
        /// <br />
        /// <xsl:value-of select="upac:Clip('Der var engang en kat hvis navn boede for enden af en gang', 17, false())"><br />
        /// Would give: Der var engang en<br />
        /// <br />
        /// <xsl:value-of select="upac:Clip('Der var engang en kat hvis navn boede for enden af en gang', 19, true())"><br />
        /// Would give: Der var engang...<br />
        /// ]]>
        /// </code>
        /// </example>
        public static string Clip(string text, int maxLength, bool ellipsis)
        {
            return StringUtil.Clip(text, maxLength, ellipsis);
        }

        /// <summary>
        /// Creates the empty iterator. Used only internel by this class
        /// </summary>
        /// <returns>An empty XPathNodeIterator</returns>
        public static XPathNodeIterator CreateEmptyIterator()
        {
            return new XmlDocument().CreateNavigator().Select("*");
        }

        /// <summary>
        /// Creates XML that can be used as a foor loop. Starting from 1 and to the provided to parameter.
        /// </summary>
        /// <param name="to">ending point</param>
        /// <returns>An XPathNodeIterator wich can be iterated over</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:for-each select="upac:CreateForLoop(3)">
        ///		<xsl:variable name="i" select="." />
        ///		<xsl:value-of select="$i" />
        /// </xsl:for-each>
        /// Would give: 123
        /// ]]>
        /// </code>
        /// </example>
        public static XPathNodeIterator CreateForLoop(int to)
        {
            return CreateForLoop(1, to);
        }

        /// <summary>
        /// Creates XML that can be used as a foor loop. Starting from parameter from and to the provided to parameter.
        /// </summary>
        /// <param name="from">Start int</param>
        /// <param name="to">End int</param>
        /// <returns>An XPathNodeIterator wich can be iterated over</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:for-each select="upac:CreateForLoop(3, 7)">
        ///		<xsl:variable name="i" select="." />
        ///		<xsl:value-of select="$i" />
        /// </xsl:for-each>
        /// Would give: 34567
        /// ]]>
        /// </code>
        /// </example>
        public static XPathNodeIterator CreateForLoop(int from, int to)
        {
            if (from > to)
            {
                throw new Exception("from må ikke være større end to!");
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<values/>");
            for (int i = from; i <= to; i++)
            {
                XmlNode node = doc.FirstChild.AppendChild(doc.CreateElement("value"));
                node.InnerText = i.ToString();
            }
            XPathNavigator xp = doc.CreateNavigator();
            return xp.Select("//value");
        }

        /// <summary>
        /// Returns the RFC822 datetime format for the specified isodate.
        /// RFC822 should be used when generating rss feeds etc
        /// </summary>
        /// <param name="dateTime">The dateTime to convert to RFC822 format</param>
        /// <returns>RFC822 datetime format</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:CreateRfc822Date('2010-08-23T13:33:00')" />
        /// Would give: Mon, 23 Aug 2010 13:33:00 +0200
        /// ]]>
        /// </code>
        /// </example>
        public static string CreateRfc822Date(string isodate)
        {
            DateTime dateTime = DateTime.Parse(isodate);
            return DateUtil.CreateRfc822Date(dateTime);
        }

        /// <summary>
        /// Fetches a external rss/atom feed and normalise it to atom schema.
        /// </summary>
        /// <param name="url">The external URL for the feed to fetch.</param>
        /// <returns>An XPathNodeIterator iterator containing a atom schema xml.</returns>
        public static XPathNodeIterator FetchFeedAndNormaliseToAtom(string url)
        {
            XmlDocument doc = SyndicationUtil.FetchFeedAndNormaliseToAtom(url);
            if (doc != null)
            {
                XPathNavigator xp = doc.CreateNavigator();
                XPathNodeIterator iterator = xp.Select("*");
                return iterator;
            }
            return CreateEmptyIterator();
        }

        /// <summary>
        /// Formats the iso date.
        /// </summary>
        /// <param name="isodate">The isodate.</param>
        /// <param name="fieldNameInSettings">The field name in settings.</param>
        /// <returns></returns>
        public static string FormatIsoDate(string isodate, string fieldNameInSettings)
        {
            return DateUtil.FormatDate(isodate, fieldNameInSettings);
        }

        /// <summary>
        /// Formats the iso date long.
        /// </summary>
        /// <param name="isodate">The isodate.</param>
        /// <returns></returns>
        public static string FormatIsoDateLong(string isodate)
        {
            return DateUtil.FormatDateLong(isodate);
        }

        /// <summary>
        /// Formats the iso date short.
        /// </summary>
        /// <param name="isodate">The isodate.</param>
        /// <returns></returns>
        public static string FormatIsoDateShort(string isodate)
        {
            return DateUtil.FormatDateShort(isodate);
        }

        /// <summary>
        /// Formats the iso to date and time.
        /// </summary>
        /// <param name="isodate">The isodate.</param>
        /// <returns></returns>
        public static string FormatIsoDateTime(string isodate)
        {
            return DateUtil.FormatDateTime(isodate);
        }

        /// <summary>
        /// Formats the iso to time.
        /// </summary>
        /// <param name="isodate">The isodate.</param>
        /// <returns></returns>
        public static string FormatIsoTime(string isodate)
        {
            return DateUtil.FormatTime(isodate);
        }

        /// <summary>
        /// Gets the name of the current culture.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCultureName()
        {
            return UpacContext.Current.CurrentCulture.Name;
        }

        public static XPathNodeIterator GetDescendantsViaXPath(XPathNodeIterator from, string xpath)
        {
            Node node = UmbracoUtil.GetNode(from);
            if (node != null)
            {
                XPathNavigator navigator = node.ToXPathNavigator();
                return navigator.Select(xpath);
            }
            return CreateEmptyIterator();
        }

        /// <summary>
        /// Gets a dictionary item from Umbraco.
        /// If querystring parameter debug exists a debug span is also provided.
        /// </summary>
        /// <param name="key">The dictionary key</param>
        /// <returns>The value of the dictionary item. If in debug an span is also returned.</returns>
        /// <example>
        /// <code lang="xsl" title="Example when not in debug mode">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetDictionaryItem('Global-Search')" />
        /// Would give: Search
        /// ]]>
        /// </code>
        /// </example>
        /// <example>
        /// <code lang="xsl" title="When in debug mode">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetDictionaryItem('Global-Search')" />
        /// Would give: Search<span class="dictionaryDebug" title="Global-Search">¤</span>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetDictionaryItem(string key)
        {
            if (IsInUpacDebug())
            {
                return string.Format("{0}<span class=\"dictionaryDebug\" title=\"{1}\">¤</span>", umbraco.library.GetDictionaryItem(key), key);
            }
            return umbraco.library.GetDictionaryItem(key);
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static string GetFileExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            if (extension.Length == 0)
            {
                return string.Empty;
            }
            if (extension[0] != '.')
            {
                return extension;
            }
            return extension.Substring(1);
        }

        /// <summary>
        /// Gets the home node.
        /// </summary>
        /// <returns></returns>
        public static XPathNodeIterator GetHomeNode()
        {
            Node node = UpacContext.Current.HomeNode;
            if (node != null)
            {
                return node.ToXPathNodeIterator();
            }
            return CreateEmptyIterator();
        }

        /// <summary>
        /// Gets an xhtml image tag via the provided media id or image path.
        /// </summary>
        /// <param name="imagePathOrMediaId">The image path or media id.</param>
        /// <returns>An xhtml image tag</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetImageTag(1101)" />
        /// Would give: <img src="/media/1101/image.jpg" alt="media name from umbraco" />
        /// 
        /// <xsl:value-of select="upac:GetImageTag('/media/1101/image.jpg')" />
        /// Would give: <img src="/media/1101/image.jpg" />
        /// ]]>
        /// </code>
        /// </example>
        public static string GetImageTag(string imagePathOrMediaId)
        {
            return GetImageTag(imagePathOrMediaId, -1, -1);
        }

        /// <summary>
        /// Gets an xhtml image tag via the provided media id or image path
        /// </summary>
        /// <param name="imagePathOrMediaId">The image path or media id.</param>
        /// <param name="maxWidth">Max width.</param>
        /// <returns>An xhtml image tag</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetImageTag(1101, 100)" />
        /// Would give: <img src="/ImageGen/Gen.aspx?media=/media/1101/image.jpg&width=100" alt="media name from umbraco" />
        /// 
        /// <xsl:value-of select="upac:GetImageTag('/media/1101/image.jpg', 100)" />
        /// Would give: <img src="/ImageGen/Gen.aspx?media=/media/1101/image.jpg&width=100" />
        /// ]]>
        /// </code>
        /// </example>
        public static string GetImageTag(string imagePathOrMediaId, int maxWidth)
        {
            return GetImageTag(imagePathOrMediaId, maxWidth, -1);
        }

        public static string GetImageTag(string imagePathOrMediaId, int maxWidth, int maxHeight)
        {
            return GetImageTag(imagePathOrMediaId, maxWidth, maxHeight, true);
        }

        public static string GetImageTag(string imagePathOrMediaId, int maxWidth, int maxHeight, bool constrain)
        {
            if (string.IsNullOrEmpty(imagePathOrMediaId))
            {
                return string.Empty;
            }

            int mediaId = CommonUtil.ConvertToIntSafe(imagePathOrMediaId, -1);
            if (mediaId == -1)
            {
                return GetImageTag(imagePathOrMediaId, maxWidth, maxHeight);
            }

            Media media = UmbracoUtil.GetMedia(mediaId);
            if (media != null)
            {
                string file = media.GetPropertyValue("umbracoFile", string.Empty);
                if (!string.IsNullOrEmpty(file))
                {
                    string alt = HttpUtility.HtmlEncode(media.Text);

                    string thumbnailUrl = ImageUtil.ThumbnailUrl(file, maxWidth, maxHeight, constrain);
                    return GetImageTag(thumbnailUrl, alt);
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get a link tag
        /// </summary>
        /// <remarks>
        /// Gets a link tag where the href points to the node and text will come from GetMenuTitle
        /// </remarks>
        /// <param name="nodeId">The node id.</param>
        /// <returns>An a tag</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetLinkTag(1541)" />
        /// Would give: <a href="/Subjectpage/HelloWorld">Hello World</a>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetLinkTag(string nodeId)
        {
            return GetLinkTag(nodeId, string.Empty);
        }

        /// <summary>
        /// Get a link tag with overload for the link text
        /// </summary>
        /// <remarks>
        /// Gets a link tag where the href points to the node and you have provided the link text
        /// </remarks>
        /// <param name="nodeId">The node id.</param>
        /// <param name="linkText">The link text.</param>
        /// <returns>An a tag</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetLinkTag(1541, 'Hi World')" />
        /// Would give: <a href="/Subjectpage/HelloWorld">Hi World</a>
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="With empty link text">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetLinkTag(1541, '')" />
        /// Would give: <a href="/Subjectpage/HelloWorld">Hello World</a>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetLinkTag(string nodeId, string linkText)
        {
            return GetLinkTag(nodeId, linkText, string.Empty);
        }

        /// <summary>
        /// Get a link tag with overload for the link text and a css class to add to the a tag.
        /// </summary>
        /// <remarks>
        /// Gets a link tag where the href points to the node and you can add a css class
        /// </remarks>
        /// <param name="nodeId">The node id.</param>
        /// <param name="linkText">The link text.</param>
        /// <param name="cssClass">Css class</param>
        /// <returns>An a tag</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetLinkTag(1541, 'Hi World', 'highlight')" />
        /// Would give: <a href="/Subjectpage/HelloWorld" class="highlight">Hi World</a>
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="With empty link text">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetLinkTag(1541, '', 'highlight')" />
        /// Would give: <a href="/Subjectpage/HelloWorld" class="highlight">Hello World</a>
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="With empty link text and css class">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetLinkTag(1541, '', '')" />
        /// Would give: <a href="/Subjectpage/HelloWorld">Hello World</a>
        /// ]]>
        /// </code>
        /// </example>
        public static string GetLinkTag(string nodeId, string linkText, string cssClass)
        {
            Node node = UmbracoUtil.GetNode(nodeId);
            if (node != null)
            {
                string text = !string.IsNullOrEmpty(linkText) ? linkText : UmbracoUtil.GetMenuTitle(node);
                string url = LinkManager.GetNodeUrl(node);
                string linkClass = string.IsNullOrEmpty(cssClass) ? string.Empty : string.Format(" class=\"{0}\"", cssClass);
                if (text != string.Empty && url != string.Empty)
                {
                    string tag = string.Format("<a href=\"{0}\"{1}>{2}</a>", url, linkClass, text);
                    return tag;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the media path via a media id
        /// </summary>
        /// <param name="mediaId">The media id.</param>
        /// <returns>The file path to the media</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetMediaPath(1101)" />
        /// Would give: /media/1101/image.jpg
        /// ]]>
        /// </code>
        /// </example>
        public static string GetMediaPath(int mediaId)
        {
            Media media = UmbracoUtil.GetMedia(mediaId);
            if (media != null)
            {
                return media.GetPropertyValue("umbracoFile", string.Empty);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the OEmbed HTML.
        /// </summary>
        /// <param name="url">The OEmbed URL/Source.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns>The OEmbed HTML</returns>
        public static string GetOEmbedHtml(string url, string maxWidth, string maxHeight)
        {
            return OEmbedUtil.GetOEmbedHtml(url, maxWidth, maxHeight);
        }

        /// <summary>
        /// Gets the readable file size via bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>A readable file size</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:GetReadableFileSizeViaBytes(1073741824)" />
        /// Would give: 1 GB
        /// 
        /// <xsl:value-of select="upac:GetReadableFileSizeViaBytes(1048576)" />
        /// Would give: 1 MB
        /// 
        /// <xsl:value-of select="upac:GetReadableFileSizeViaBytes(1024)" />
        /// Would give: 1 KB
        /// 
        /// <xsl:value-of select="upac:GetReadableFileSizeViaBytes(600)" />
        /// Would give: 600 Bytes
        /// ]]>
        /// </code>
        /// </example>
        public static string GetReadableFileSizeViaBytes(string bytes)
        {
            int bytesAsInt = CommonUtil.ConvertToIntSafe(bytes, -1);
            if (bytesAsInt > -1)
            {
                return CommonUtil.GetReadableFileSizeViaBytes(bytesAsInt);
            }
            return string.Empty;
        }

        public static int GetTemplateIdFromAlias(string templateAlias)
        {
            return umbraco.cms.businesslogic.template.Template.GetTemplateIdFromAlias(templateAlias);
        }

        public static string GetThumbnailUrl(string imagePathOrMediaId, int width)
        {
            return GetThumbnailUrl(imagePathOrMediaId, width, -1);
        }

        public static string GetThumbnailUrl(string imagePathOrMediaId, int width, int height)
        {
            return GetThumbnailUrl(imagePathOrMediaId, width, height, true);
        }

        public static string GetThumbnailUrl(string imagePathOrMediaId, int width, int height, bool constrain)
        {
            int mediaId = CommonUtil.ConvertToIntSafe(imagePathOrMediaId, -1);

            if (mediaId > -1)
            {
                return ImageUtil.ThumbnailUrl(mediaId, width, height, constrain);
            }
            return ImageUtil.ThumbnailUrl(imagePathOrMediaId, width, height, constrain);
        }

        public static bool IsInUpacDebug()
        {
            return HttpContext.Current.Request.QueryString["debug"] != null;
        }

        public static string IsoDateNow()
        {
            return DateUtil.ToIsoDate(DateTime.Now);
        }

        /// <summary>
        /// A simple method which returns true if the node should be included in navigations.
        /// The following things must be true
        /// 1. The node must not have set the property hideFromNavigation true.
        /// 2. The node must have a template associated
        /// </summary>
        /// <param name="ni">The node to check.</param>
        /// <returns>True if the node should be shown in navigations</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:variable name="startNode" select="$currentPage/ancestor-or-self::*[@level=2]" />
        /// <xsl:variable name="nodes" select="$node/*[upac:MenuInclude(.)]" />
        /// <ul>
        ///    <xsl:for-each select="$nodes">
        ///      <li>
        ///         <a href="{upac:UrlViaNodeId(@id)}"><xsl:value-of select="@nodeName"/></a>
        ///      </li>
        ///   </xsl:for-each>
        /// </ul>
        /// ]]>
        /// </code>
        /// </example>
        public static bool MenuInclude(XPathNodeIterator ni)
        {
            if (ni.MoveNext() == false)
            {
                return false;
            }

            XPathNavigator nav = ni.Current;

            if (nav.SelectSingleNode("@isDoc") == null)
            {
                return false;
            }

            // Redirect node should always be shown
            if (nav.Name == Configuration.ConfigurationManager.DocumentTypeAliases.Redirect.Value)
            {
                return true;
            }

            // Need to check if there is a template on the node.
            // Nodes with no template, should not be shown
            if (nav.SelectSingleNode("@template").ValueAsInt == 0)
            {
                return false;
            }

            XPathNavigator node = nav.SelectSingleNode(Upac.Core.Configuration.ConfigurationManager.PropertyAliases.HideFromNavigation.Value);
            if (node != null)
            {
                return (node.Value != "1");
            }

            return true;
        }

        public static string MenuTitle(XPathNodeIterator ni)
        {
            Node node = UmbracoUtil.GetNode(ni);
            if (node != null)
            {
                return UmbracoUtil.GetMenuTitle(node);
            }
            return string.Empty;
        }

        /// <summary>
        /// Converts a string to a bool. If the input string is equal 1 true is returned otherwise false.
        /// </summary>
        /// <param name="input">The input to be converted</param>
        /// <returns>The converted input</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:ToBool('1')" />
        /// Would give: true()
        /// 
        /// <xsl:value-of select="upac:ToBool('0')" />
        /// Would give: false()
        /// 
        /// <xsl:value-of select="upac:ToBool('')" />
        /// Would give: false()
        /// 
        /// <xsl:value-of select="upac:ToBool('Hello World')" />
        /// Would give: false()
        /// ]]>
        /// </code>
        /// </example>
        public static bool ToBool(string input)
        {
            return input == "1";
        }

        /// <summary>
        /// Converts a string to a number. If the input string is empty or can not be converted, the defaultNumber is returned instead.
        /// </summary>
        /// <param name="input">The input to be converted</param>
        /// <param name="defaultNumber">The default number to return if the input can not be converted.</param>
        /// <returns>The converted input</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:ToNumber('123', 0)" />
        /// Would give: 123
        /// 
        /// <xsl:value-of select="upac:ToNumber('XXX', 0)" />
        /// Would give: 0
        /// ]]>
        /// </code>
        /// </example>
        public static int ToNumber(string input, int defaultNumber)
        {
            return CommonUtil.ConvertToIntSafe(input, defaultNumber);
        }

        /// <summary>
        /// Get the url to a node via node id.
        /// </summary>
        /// <remarks>
        /// This method also handles redirect nodes which can point to internal or external pages.
        /// </remarks>
        /// <param name="nodeId">The node id.</param>
        /// <returns>An url</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:UrlViaNodeId(1541)" />
        /// Would give: /Subjectpage/HelloWorld
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="How to use on A">
        /// <![CDATA[
        /// <a href="{upac:UrlViaNodeId(1541)}">Hello World</a>
        /// Would give: <a href="/Subjectpage/HelloWorld">Hello World</a>
        /// ]]>
        /// </code>
        /// </example>
        public static string UrlViaNodeId(string nodeId)
        {
            return UrlViaNodeId(nodeId, string.Empty);
        }

        /// <summary>
        /// Get the url to a node via node id and appending the provided alternate template
        /// </summary>
        /// <remarks>
        /// This method also handles redirect nodes which can point to internal or external pages.
        /// <param name="nodeId">The node id.</param>
        /// <param name="alternateTemplate">The alternate template.</param>
        /// <returns>An url with the alternate template appended to the url</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:UrlViaNodeId(1541, 'RSS')" />
        /// Would give: /Subjectpage/HelloWorld/RSS
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="How to use on A">
        /// <![CDATA[
        /// <a href="{upac:UrlViaNodeId(1541, 'RSS')}">Hello World</a>
        /// Would give: <a href="/Subjectpage/HelloWorld/RSS">Hello World</a>
        /// ]]>
        /// </code>
        /// </example>
        public static string UrlViaNodeId(string nodeId, string alternateTemplate)
        {
            return UrlViaNodeId(nodeId, alternateTemplate, false);
        }

        /// <summary>
        /// Get the url to a node via node id and appending the provided alternate template and can include the domain incl. http://
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="alternateTemplate">The alternate template. May be empty.</param>
        /// <param name="includeDomain">if set to <c>true</c> include domain and http.</param>
        /// <returns>An url</returns>
        /// <example>
        /// <code lang="xsl" title="">
        /// <![CDATA[
        /// <xsl:value-of select="upac:UrlViaNodeId(1541, 'RSS', true())" />
        /// Would give: http://www.domain.dk/Subjectpage/HelloWorld/RSS
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="How to use on A">
        /// <![CDATA[
        /// <a href="{upac:UrlViaNodeId(1541, 'RSS', true())}">Hello World</a>
        /// Would give: <a href="http://www.domain.dk/Subjectpage/HelloWorld/RSS">Hello World</a>
        /// ]]>
        /// </code>
        /// <code lang="xsl" title="How to use on A">
        /// <![CDATA[
        /// <a href="{upac:UrlViaNodeId(1541, '', true())}">Hello World</a>
        /// Would give: <a href="http://www.domain.dk/Subjectpage/HelloWorld/">Hello World</a>
        /// ]]>
        /// </code>
        /// </example>
        public static string UrlViaNodeId(string nodeId, string alternateTemplate, bool includeDomain)
        {
            int id = CommonUtil.ConvertToIntSafe(nodeId, -1);
            return (id == -1) ? string.Empty : LinkManager.GetNodeUrl(id, alternateTemplate, includeDomain);
        }

        private static string GetImageTag(string imagePath, string altText)
        {
            UrlString url = new UrlString(imagePath);
            string xhtmlImageUrl = url.GetUrl(true);

            if (!string.IsNullOrEmpty(imagePath) && !string.IsNullOrEmpty(altText))
            {
                return string.Format("<img src=\"{0}\" alt=\"{1}\" />", xhtmlImageUrl, altText);
            }
            if (!string.IsNullOrEmpty(imagePath))
            {
                return string.Format("<img src=\"{0}\" />", xhtmlImageUrl);
            }
            return string.Empty;
        }

        #endregion Methods
    }
}