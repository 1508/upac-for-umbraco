<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:upac="urn:upac" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets upac ">


<xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>

<xsl:param name="currentPage"/>
<xsl:variable name="home" select="$currentPage/ancestor-or-self::*[@level=1]" />

<xsl:template match="/">

	<xsl:variable name="logoSrc">
		<xsl:choose>
			<!-- Danish Logo -->
			<xsl:when test="name($home) != 'FrontpageEnglishSite'">
				<xsl:value-of select="'/gfx/logo.png'"/>
			</xsl:when>
			<!-- English Logo -->
			<xsl:otherwise>
				<xsl:value-of select="'/gfx/logo-uk.png'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<div class="logo">
		<a href="/" title="{upac:GetDictionaryItem('Global-Frontpage')}">
			<img alt="{upac:GetDictionaryItem('Global-Frontpage')}" src="{$logoSrc}" />
			<span>
				<xsl:value-of select="upac:GetDictionaryItem('Global-Frontpage')"/>
			</span>
		</a>
	</div>
</xsl:template>

</xsl:stylesheet>