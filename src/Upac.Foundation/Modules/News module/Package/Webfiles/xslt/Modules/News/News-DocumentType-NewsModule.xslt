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
	<xsl:variable name="currentDateTime" select="umbraco.library:CurrentDate()" />
	<xsl:variable name="currentYear" select="number(umbraco.library:FormatDateTime($currentDateTime, 'yyyy'))" />
	<xsl:variable name="startYear">
		<xsl:choose>
			<xsl:when test="string(umbraco.library:RequestQueryString('year'))">
				<xsl:value-of select="concat(umbraco.library:RequestQueryString('year'), '-01-01T00:00:01')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat($currentYear, '-01-01T00:00:01')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="endYear">
		<xsl:choose>
			<xsl:when test="string(umbraco.library:RequestQueryString('year'))">
				<xsl:value-of select="concat(umbraco.library:RequestQueryString('year'), '-12-31T23:59:59')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat($currentYear, '-12-31T23:59:59')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="currentArchiveYearToShow">
		<xsl:choose>
			<xsl:when test="string(umbraco.library:RequestQueryString('year'))">
				<xsl:value-of select="string(umbraco.library:RequestQueryString('year'))"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$currentYear"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="nodes" select="$currentPage/NewsYearFolder[@nodeName = $currentArchiveYearToShow]/NewsMonthFolder/NewsDocument" />
	<xsl:variable name="nodestoshow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./date, $startYear) and umbraco.library:DateGreaterThanOrEqual($endYear, ./date)]" />
	<xsl:variable name="archiveYears" select="$currentPage/NewsYearFolder[count(./*/*) &gt; 0]" />


	<xsl:template match="/">
		<xsl:variable name="currentYear" select="number(umbraco.library:FormatDateTime(umbraco.library:CurrentDate(), 'yyyy'))" />
		
		<xsl:if test="count($archiveYears) &gt; 1">
			<div class="content grid-six grid-first">
				<ul class="yearlist clearfix">
					<xsl:for-each select="$archiveYears">
						<xsl:sort data-type="number" order="ascending" select="@nodeName"/>
						<li>
							<a href="{upac:UrlViaNodeId($currentPage/@id)}?mode=archive&amp;year={@nodeName}">
								<xsl:if test="@nodeName = $currentArchiveYearToShow">
									<xsl:attribute name="style">
										<xsl:value-of select="'text-decoration: underline;'"/>
									</xsl:attribute>
								</xsl:if>
								<xsl:value-of select="@nodeName"/>
							</a>
						</li>
					</xsl:for-each>
				</ul>
			</div>
		</xsl:if>
		<div class="grid-six event grid-first archive">
			<ul class="clearfix">
				<xsl:call-template name="OutputNodes">
					<xsl:with-param name="nodes" select="$nodestoshow[umbraco.library:DateGreaterThanOrEqual(./date, $startYear) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startYear, 'm', 4), ./date)]" />
					<xsl:with-param name="startDate" select="$startYear" />
					<xsl:with-param name="col" select="1" />
				</xsl:call-template>
			</ul>
			<div class="endlink clearfix ">
				<a href="{upac:UrlViaNodeId($currentPage/@id, 'RSS')}" class="rss">
					<xsl:value-of select="umbraco.library:GetDictionaryItem('Global-RSS')"/>
				</a>
			</div>
		</div>



	</xsl:template>

	<xsl:template name="OutputNodes">
		<xsl:param name="nodes" />
		<xsl:param name="startDate" />
		<xsl:param name="col" />
		<!-- Ensure we have more than 0 years and we have a current year to show -->
		<xsl:variable name="monthOne" select="Exslt.ExsltDatesAndTimes:monthname($startDate)" />
		<xsl:variable name="yearOne" select="Exslt.ExsltDatesAndTimes:year($startDate)" />
		<xsl:variable name="monthTwo" select="Exslt.ExsltDatesAndTimes:monthname(umbraco.library:DateAdd($startDate, 'm', 1))" />
		<xsl:variable name="yearTwo" select="Exslt.ExsltDatesAndTimes:year(umbraco.library:DateAdd($startDate, 'm', 1))" />
		<xsl:variable name="monthThree" select="Exslt.ExsltDatesAndTimes:monthname(umbraco.library:DateAdd($startDate, 'm', 2))" />
		<xsl:variable name="yearThree" select="Exslt.ExsltDatesAndTimes:year(umbraco.library:DateAdd($startDate, 'm', 2))" />
		<xsl:variable name="monthFour" select="Exslt.ExsltDatesAndTimes:monthname(umbraco.library:DateAdd($startDate, 'm', 3))" />
		<xsl:variable name="yearFour" select="Exslt.ExsltDatesAndTimes:year(umbraco.library:DateAdd($startDate, 'm', 3))" />

		<xsl:call-template name="libuilder">
			<xsl:with-param name="month" select="$monthOne"/>
			<xsl:with-param name="year" select="$yearOne"/>
			<xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 1), ./date)]" />
		</xsl:call-template>
		<xsl:call-template name="libuilder">
			<xsl:with-param name="month" select="$monthTwo"/>
			<xsl:with-param name="year" select="$yearTwo"/>
			<xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./date, umbraco.library:DateAdd($startDate, 'm', 1)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 2), ./date)]" />
		</xsl:call-template>
		<xsl:call-template name="libuilder">
			<xsl:with-param name="month" select="$monthThree"/>
			<xsl:with-param name="year" select="$yearThree"/>
			<xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./date, umbraco.library:DateAdd($startDate, 'm', 2)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 3), ./date)]" />
		</xsl:call-template>
		<xsl:call-template name="libuilder">
			<xsl:with-param name="month" select="$monthFour"/>
			<xsl:with-param name="year" select="$yearFour"/>
			<xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./date, umbraco.library:DateAdd($startDate, 'm', 3))]" />
		</xsl:call-template>

		<!-- if archive, output archive year menu -->


		<xsl:if test="$col &lt; 3">
			<xsl:call-template name="OutputNodes">
				<xsl:with-param name="nodes" select="$nodestoshow[umbraco.library:DateGreaterThanOrEqual(./date, umbraco.library:DateAdd($startDate, 'm', 4)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 8), ./date)]" />
				<xsl:with-param name="startDate" select="umbraco.library:DateAdd($startDate, 'm', 4)" />
				<xsl:with-param name="col" select="$col + 1" />
			</xsl:call-template>

		</xsl:if>
	</xsl:template>

	<xsl:template name="libuilder">
		<xsl:param name="month" />
		<xsl:param name="year" />
		<xsl:param name="nodesToShow" />
		<xsl:if test="count($nodesToShow) != 0">
			<li class="event-monthname titleContainerText">
				<xsl:value-of select="$month"/>&#160;<xsl:value-of select="$year"/>
			</li>
			<xsl:if test="count($nodesToShow) = 0">
				<li>
					Ingen nyheder
				</li>
			</xsl:if>

			<xsl:for-each select="$nodesToShow">
				<xsl:sort select="date" order="descending" data-type="text"/>
				<li>
					<xsl:if test="./image != '0'">
						<xsl:variable name="imagetag" select="upac:GetImageTag(./image,190,190,false())" />
						<xsl:value-of select="$imagetag" disable-output-escaping="yes"/>
					</xsl:if>
					<p>
						<a href="{upac:UrlViaNodeId(./@id)}">
							<xsl:value-of select="upac:MenuTitle(.)"/>
						</a>
						<xsl:choose>
							<xsl:when test="string(./listText)">
								<xsl:value-of select="concat(umbraco.library:ReplaceLineBreaks(./listText), ' ')" disable-output-escaping="yes"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(umbraco.library:ReplaceLineBreaks(./shortIntro), ' ')" disable-output-escaping="yes"/>
							</xsl:otherwise>
						</xsl:choose>
						<br/>
						<span>
							<xsl:value-of select="upac:FormatIsoDateShort(./date)"/>
						</span>
					</p>
					<xsl:comment></xsl:comment>
				</li>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>


</xsl:stylesheet>