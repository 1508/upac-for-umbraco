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
		<xsl:variable name="showEditDate" select="$currentPage/showEditDate = '1'" />
		<xsl:variable name="showToTop" select="$currentPage/showToTop = '1'" />
		<xsl:if test="$showEditDate or $showToTop">
			<div class="pageinfo-bottom">
			<xsl:if test="$showEditDate">
				<div class="pageinfo-lastupdateddate">
					<xsl:value-of select="umbraco.library:GetDictionaryItem('Global-Pageinfo-LastUpdatedDate')"/>
					<xsl:value-of select="upac:FormatIsoDateShort($currentPage/@updateDate)" />
				</div>
			</xsl:if>
			<xsl:if test="$showToTop">
				<div class="pageinfo-totop">
					<a href="#top">
						<xsl:value-of select="umbraco.library:GetDictionaryItem('Global-Pageinfo-ToTop')"/>
					</a>
				</div>
			</xsl:if>
			</div>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>