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

	<xsl:variable name="years" select="$currentPage/PublicationYearFolder" />
	<xsl:variable name="currentYearToShow">
		<xsl:choose>
			<xsl:when test="string(umbraco.library:RequestQueryString('year'))">
				<xsl:value-of select="string(umbraco.library:RequestQueryString('year'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$years">
					<xsl:sort data-type="number" order="descending" select="@nodeName"/>
					<xsl:if test="position() = 1">
						<xsl:value-of select="@nodeName"/>
					</xsl:if>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<xsl:template match="/">
		<!-- Ensure we have more than 0 years and we have a current year to show -->
		<xsl:if test="count($years) &gt; 0 and string($currentYearToShow)">

			<!-- if more than one year - write out the year navigation -->
			<xsl:if test="count($years) &gt; 1">
				<ul class="modules-news-year-navigation list-navigation">
					<xsl:for-each select="$years">
						<xsl:sort data-type="number" order="descending" select="@nodeName"/>
						<li>
							<a href="{upac:UrlViaNodeId($currentPage/@id)}?year={@nodeName}">
								<xsl:if test="@nodeName = $currentYearToShow">
									<xsl:attribute name="class">
										<xsl:value-of select="'active'"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:value-of select="@nodeName"/>
							</a>
						</li>
					</xsl:for-each>
				</ul>
			</xsl:if>

			<xsl:variable name="nodes" select="$currentPage/PublicationYearFolder[@nodeName = $currentYearToShow]/PublicationMonthFolder/PublicationDocument" />
			<xsl:if test="count($nodes) &gt; 0">
				<div class="modules-news-year list-item">
					<ul>
						<xsl:for-each select="$nodes">
							<xsl:sort select="date" order="descending" data-type="text"/>
							<xsl:variable name="item" select="umbraco.library:GetXmlNodeById(./@id)" />
							<li>
								<xsl:if test="string($item/image)">
									<div class="list-item-image">
										<xsl:value-of select="upac:GetImageTag($item/image, 220)" disable-output-escaping="yes"/>
									</div>
								</xsl:if>
								<div class="list-item-text">
									<xsl:if test="string($item/image) = false()">
										<xsl:attribute name="class">
											<xsl:value-of select="''"/>
											<xsl:text>list-item-text list-item-text-no-image</xsl:text>
										</xsl:attribute>
									</xsl:if>
									<a href="{upac:UrlViaNodeId($item/@id)}">
										<strong>
											<xsl:value-of select="$item/@nodeName"/>
										</strong>
									</a>
									<p>
										<xsl:value-of select="$item/shortIntro"/>
									</p>
									<span>
										<xsl:value-of select="upac:FormatIsoDateShort($item/date)"/>
									</span>
								</div>
							</li>
						</xsl:for-each>
					</ul>
				</div>
			</xsl:if>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>