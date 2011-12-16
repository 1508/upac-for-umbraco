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

  <div class="primarynavigation">
      <hr class="accessibility" />
        <p class="accessibility">
          <strong>
            <xsl:value-of select="umbraco.library:GetDictionaryItem('Accessibility-PrimaryNavigation')"/>
          </strong>
        </p>
      <ul class="clearfix">
        <xsl:variable name="nodeContainer" select="$currentPage/ancestor-or-self::*[@level=1]" />
        <xsl:if test="string($nodeContainer/@id)">
          <xsl:variable name="nodes" select="$nodeContainer/*[upac:MenuInclude(.)]" />
          <xsl:if test="count($nodes) > 0">
            <xsl:for-each select="$nodes">
              <li>
                <a href="{upac:UrlViaNodeId(./@id)}">
                  <xsl:if test="./@id = $currentPage/ancestor-or-self::*/@id">
                    <xsl:attribute name="class">
                      <xsl:choose>
                        <xsl:when test="./@id = $currentPage/@id">
                          <xsl:value-of select="'open active'"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="'open'"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:attribute>
                  </xsl:if>
                  <xsl:value-of select="upac:MenuTitle(.)"/>
                </a>
              </li>
            </xsl:for-each>
          </xsl:if>
        </xsl:if>
      </ul>
    </div>

</xsl:template>

</xsl:stylesheet>