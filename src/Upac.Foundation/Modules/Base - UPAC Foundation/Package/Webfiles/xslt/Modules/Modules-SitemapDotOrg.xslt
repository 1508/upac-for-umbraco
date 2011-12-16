<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
	<!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:upac="urn:upac"
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets upac ">


	<xsl:output method="xml" omit-xml-declaration="yes"/>

	<xsl:param name="currentPage"/>

	<xsl:template match="/">
		<!-- change the mimetype for the current page to xml -->
		<xsl:value-of select="umbraco.library:ChangeContentType('text/xml')"/>
		<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
			<xsl:call-template name="OutputNone">
				<xsl:with-param name="node" select="$currentPage" />
			</xsl:call-template>
		</urlset>
	</xsl:template>

	<xsl:template name="OutputNone">
		<xsl:param name="node" />
		<xsl:if test="$node[@template != '0']">
			<url xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
				<loc xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
					<xsl:value-of select="concat('http://', umbraco.library:RequestServerVariables('SERVER_NAME'), upac:UrlViaNodeId($node/@id))"/>
				</loc>
				<lastmod xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
					<xsl:value-of select="umbraco.library:FormatDateTime($node/@updateDate, 'yyyy-MM-dd')"/>
				</lastmod>
			</url>
		</xsl:if>
		<xsl:for-each select="$node/node">
			<xsl:call-template name="OutputNone">
				<xsl:with-param name="node" select="." />
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	
</xsl:stylesheet>