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
	
	<xsl:variable name="startlevel" select="number(2)"/>
	
<xsl:template match="/">
	<div class="secondarynavigation">
		<hr class="accessibility" />

		<p class="accessibility">
			<strong>
				<xsl:value-of select="umbraco.library:GetDictionaryItem('Accessibility-SecondaryNavigation')"/>
			</strong>
		</p>
		<xsl:variable name="startNode" select="$currentPage/ancestor-or-self::*[@level=$startlevel]" />
		<ul>
			<xsl:call-template name="OutputNode">
				<xsl:with-param name="node" select="$startNode" />
			</xsl:call-template>
		</ul>
	</div>
</xsl:template>

	<xsl:template name="OutputNode">
		<xsl:param name="node" />
		<li>
			<a href="{upac:UrlViaNodeId($node/@id)}">
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="$node/@id = $currentPage/@id">
							<xsl:value-of select="concat('open active ', 'navigationlevel-', $node/@level - $startlevel)"/>
						</xsl:when>
						<xsl:when test="$node/@id = $currentPage/ancestor-or-self::*/@id">
                            <xsl:text>open-parent</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat('navigationlevel-', $node/@level - $startlevel)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="upac:MenuTitle($node)"/>
			</a>
			<xsl:if test="$node/@id = $currentPage/ancestor-or-self::*/@id">
				<xsl:variable name="nodes" select="$node/*[upac:MenuInclude(.)]" />
				<xsl:if test="count($nodes) > 0">
					<ul>
						<xsl:for-each select="$nodes">
							<xsl:call-template name="OutputNode">
								<xsl:with-param name="node" select="." />
							</xsl:call-template>
						</xsl:for-each>
					</ul>
				</xsl:if>
			</xsl:if>
		</li>
	</xsl:template>

</xsl:stylesheet>