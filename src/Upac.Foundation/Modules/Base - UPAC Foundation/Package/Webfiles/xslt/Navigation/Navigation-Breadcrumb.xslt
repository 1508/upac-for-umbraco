<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:upac="urn:upac" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets upac ">


<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>

	<xsl:template match="/">
		<div class="breadcrumb">
			<hr class="accessibility" />
			<p class="accessibility">
				<strong>
					<xsl:value-of select="umbraco.library:GetDictionaryItem('Accessibility-BreadcrumbNavigation')"/>
				</strong>
			</p>
			<span>
				<xsl:value-of select="umbraco.library:GetDictionaryItem('Global-Breadcrumb-YouAreHere')"/>
			</span>
			<ul>
					<li>
						<a href="/">
							<xsl:value-of select="umbraco.library:GetDictionaryItem('Global-Breadcrumb-Frontpage')"/>
						</a>
					</li>
					<xsl:call-template name="OutputNodes">
						<xsl:with-param name="nodes" select="$currentPage/ancestor-or-self::*[upac:MenuInclude(.) and @level &gt; 1]" />
					</xsl:call-template>
			</ul>
		</div>
	</xsl:template>

	<xsl:template name="OutputNodes">
		<xsl:param name="nodes" />
		<xsl:for-each select="$nodes">
			<li>
				/
			</li>
			<li>
				<a href="{upac:UrlViaNodeId(@id)}">
					<xsl:if test="$currentPage/@id = @id">
						<xsl:attribute name="class">
							<xsl:value-of select="'active'" />
						</xsl:attribute>
					</xsl:if>
					<xsl:value-of select="upac:MenuTitle(.)"/>
				</a>
			</li>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>