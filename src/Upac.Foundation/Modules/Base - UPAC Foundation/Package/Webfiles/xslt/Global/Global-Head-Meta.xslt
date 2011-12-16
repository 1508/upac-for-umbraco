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

  <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    <xsl:call-template name="OutputMeta">
      <xsl:with-param name="name" select="'description'" />
      <xsl:with-param name="content" select="$currentPage/metaDescription" />
    </xsl:call-template>

    <xsl:call-template name="OutputMeta">
      <xsl:with-param name="name" select="'keywords'" />
      <xsl:with-param name="content" select="$currentPage/metaKeywords" />
    </xsl:call-template>

    <xsl:variable name="indexThisPage" select="string($currentPage/metaNoIndex) != '1'" />
    <xsl:variable name="indexChildren" select="string($currentPage/metaNoIndexOnDescendents) != '1'" />
    <xsl:variable name="robotsMetaValue">
      <xsl:choose>
        <xsl:when test="$indexThisPage = false() and $indexChildren = false()">
          <xsl:value-of select="'NOINDEX, NOFOLLOW'"/>
        </xsl:when>
        <xsl:when test="$indexThisPage = false()">
          <xsl:value-of select="'NOINDEX, FOLLOW'"/>
        </xsl:when>
        <xsl:when test="$indexChildren = false()">
          <xsl:value-of select="'INDEX, NOFOLLOW'"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'INDEX, FOLLOW'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:call-template name="OutputMeta">
      <xsl:with-param name="name" select="'robots'" />
      <xsl:with-param name="content" select="$robotsMetaValue" />
    </xsl:call-template>

<!-- start writing XSLT -->

</xsl:template>
    
    <xsl:template name="OutputMeta">
    <xsl:param name="name" />
    <xsl:param name="content" />
    <xsl:if test="string($content)">
      <xsl:text><!-- We want nicely tabs on the html, therefore we are forcing two tabs -->
    </xsl:text>
      <meta name="{$name}" content="{$content}" />
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>