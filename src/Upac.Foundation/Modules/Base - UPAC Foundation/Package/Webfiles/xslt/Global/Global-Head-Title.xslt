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
  <xsl:variable name="node" select="$currentPage/ancestor-or-self::*[@level=1]/ConfigurationContainer/ConfigurationSearchEngineOptimization" />
  <xsl:variable name="prefix" select="$node/browserTitlePrefix" />
  <xsl:variable name="suffix" select="$node/browserTitleSuffix" />
    <xsl:variable name="currentPageTitle">
      <xsl:choose>
        <xsl:when test="string($currentPage/browserTitle)">
          <xsl:value-of select="$currentPage/browserTitle"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$currentPage/@nodeName"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <title>
      <xsl:value-of select="concat($prefix, $currentPageTitle, $suffix)"/>
    </title>
</xsl:template>

</xsl:stylesheet>