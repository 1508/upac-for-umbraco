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
    <xsl:variable name="debug" select="false()" />
    <xsl:variable name="mode">
        <xsl:choose>
            <xsl:when test="string(umbraco.library:RequestQueryString('mode')) = 'archive'">
                <xsl:value-of select="'archive'"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="'upcomming'"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:variable>
    <xsl:variable name="headline">
        <xsl:choose>
            <xsl:when test="$mode = 'upcomming'" >
                <xsl:value-of select="umbraco.library:GetDictionaryItem('Event-Upcomming')" />
            </xsl:when>
            <xsl:when test="string($currentPage/ArchiveHeadline)">
                <xsl:value-of select="$currentPage/ArchiveHeadline" />
            </xsl:when>
        </xsl:choose>
    </xsl:variable>
    <xsl:variable name="currentDateTime" select="umbraco.library:CurrentDate()" />
    <xsl:variable name="currentDate" select="concat(substring(umbraco.library:CurrentDate(), 1, 10), 'T23:59:59')" />
    <xsl:variable name="currentMonth" select="number(umbraco.library:FormatDateTime($currentDate, 'MM'))" />
    <xsl:variable name="currentYear" select="number(umbraco.library:FormatDateTime($currentDate, 'yyyy'))" />
    <xsl:variable name="startDate" select="concat($currentYear, '-', $currentMonth, '-', '1T00:00:01')" />
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
    <xsl:variable name="allEventNodes" select="$currentPage/EventYearFolder/EventMonthFolder/EventDocument[string(./dateFrom) and string(./dateTo)]" />
    <xsl:variable name="upcommingNodes" select="$allEventNodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, $currentDate) or umbraco.library:DateGreaterThanOrEqual(./dateTo, $currentDate)]" />
    <xsl:variable name="archivedNodes" select="$allEventNodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, $startYear) and umbraco.library:DateGreaterThanOrEqual($endYear, ./dateFrom)]" />
    <xsl:variable name="archiveYears" select="$currentPage/EventYearFolder[count(./*/*) &gt; 0]" />
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


    <xsl:template match="/">
        <xsl:variable name="currentYear" select="number(umbraco.library:FormatDateTime(umbraco.library:CurrentDate(), 'yyyy'))" />
        <xsl:if test="$debug">
            <p>
                currentDate: <xsl:value-of select="$currentDate"/>
                <br />
                currentDateTime: <xsl:value-of select="$currentDateTime"/>
                <br />
                startDate: <xsl:value-of select="$startDate"/>
                <br/>
                currentMonth: <xsl:value-of select="$currentMonth"/>
                <br/>
                mode: <xsl:value-of select="$mode"/>
                <br />
                allEventNodes: <xsl:value-of select="count($allEventNodes)"/>
                <br />
                upcommingNodes: <xsl:value-of select="count($upcommingNodes)"/>
                <br />
                archivedNodes: <xsl:value-of select="count($archivedNodes)"/>
                <br />
                currentArchiveYearToShow: <xsl:value-of select="$currentArchiveYearToShow"/>
                <br/>
                Date + 3: <xsl:value-of select="umbraco.library:DateAdd($startDate, 'm', 3)" />
            </p>
        </xsl:if>
        <xsl:comment></xsl:comment>
        <xsl:if test="$mode = 'archive' and count($archiveYears) &gt; 1">
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
            <ul>
                <xsl:choose>
                    <xsl:when test="$mode = 'archive'">
                        <xsl:call-template name="OutputNodes">
                            <xsl:with-param name="nodes" select="$archivedNodes[../../@nodeName = $currentArchiveYearToShow and umbraco.library:DateGreaterThanOrEqual(./dateFrom, $startYear) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startYear, 'm', 4), ./dateFrom)]" />
                            <xsl:with-param name="mode" select="'archive'" />
                            <xsl:with-param name="startDate" select="$startYear" />
                            <xsl:with-param name="col" select="1" />
                        </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:call-template name="OutputNodes">
                            <xsl:with-param name="nodes" select="$upcommingNodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, $currentDateTime) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 4), ./dateFrom)]" />
                            <xsl:with-param name="startDate" select="$startDate" />
                            <xsl:with-param name="col" select="1" />
                        </xsl:call-template>
                    </xsl:otherwise>
                </xsl:choose>
            </ul>

            <div class="endlink clearfix">
                <xsl:choose>
                    <xsl:when test="$mode != 'archive'">
                        <a href="{upac:UrlViaNodeId($currentPage/@id)}?mode=archive">
                            <xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-facts-goToArchive')"/>
                        </a>
                    </xsl:when>
                    <xsl:otherwise>
                        <a href="{upac:UrlViaNodeId($currentPage/@id)}?mode=upcomming">
                            <xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-facts-goToUpComming')"/>
                        </a>
                    </xsl:otherwise>
                </xsl:choose>
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
        <xsl:param name="mode" />
        <xsl:variable name="today" select="umbraco.library:CurrentDate()"/>
        <xsl:variable name="monthOne" select="Exslt.ExsltDatesAndTimes:monthname($startDate)" />
        <xsl:variable name="yearOne" select="Exslt.ExsltDatesAndTimes:year($startDate)" />
        <xsl:variable name="monthTwo" select="Exslt.ExsltDatesAndTimes:monthname(umbraco.library:DateAdd($startDate, 'm', 1))" />
        <xsl:variable name="yearTwo" select="Exslt.ExsltDatesAndTimes:year(umbraco.library:DateAdd($startDate, 'm', 1))" />
        <xsl:variable name="monthThree" select="Exslt.ExsltDatesAndTimes:monthname(umbraco.library:DateAdd($startDate, 'm', 2))" />
        <xsl:variable name="yearThree" select="Exslt.ExsltDatesAndTimes:year(umbraco.library:DateAdd($startDate, 'm', 2))" />
        <xsl:variable name="monthFour" select="Exslt.ExsltDatesAndTimes:monthname(umbraco.library:DateAdd($startDate, 'm', 3))" />
        <xsl:variable name="yearFour" select="Exslt.ExsltDatesAndTimes:year(umbraco.library:DateAdd($startDate, 'm', 3))" />

        <xsl:if test="$debug">
            <li>
                start: <xsl:value-of select="$startDate"/><br/>
                end: <xsl:value-of select="umbraco.library:DateAdd($startDate, 'm', 3)"/>
            </li>
        </xsl:if>
        <xsl:call-template name="buildLi">
            <xsl:with-param name="month" select="$monthOne" />
            <xsl:with-param name="year" select="$yearOne" />
            <xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 1), ./dateFrom)]" />
        </xsl:call-template>
        <xsl:call-template name="buildLi">
            <xsl:with-param name="month" select="$monthTwo" />
            <xsl:with-param name="year" select="$yearTwo" />
            <xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, umbraco.library:DateAdd($startDate, 'm', 1)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 2), ./dateFrom)]" />
        </xsl:call-template>
        <xsl:call-template name="buildLi">
            <xsl:with-param name="month" select="$monthThree" />
            <xsl:with-param name="year" select="$yearThree" />
            <xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, umbraco.library:DateAdd($startDate, 'm', 2)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 3), ./dateFrom)]" />
        </xsl:call-template>
        <xsl:call-template name="buildLi">
            <xsl:with-param name="month" select="$monthFour" />
            <xsl:with-param name="year" select="$yearFour" />
            <xsl:with-param name="nodesToShow" select="$nodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, umbraco.library:DateAdd($startDate, 'm', 3))]" />
        </xsl:call-template>

        <!-- if archive, output archive year menu -->

        <xsl:if test="$col &lt; 3">
            <xsl:choose>
                <xsl:when test="$mode = 'archive'">
                    <xsl:call-template name="OutputNodes">
                        <xsl:with-param name="nodes" select="$archivedNodes[../../@nodeName = $currentArchiveYearToShow and umbraco.library:DateGreaterThanOrEqual(./dateFrom, umbraco.library:DateAdd($startDate, 'm', 4)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 8), ./dateFrom)]" />
                        <xsl:with-param name="startDate" select="umbraco.library:DateAdd($startDate, 'm', 4)" />
                        <xsl:with-param name="mode" select="$mode" />
                        <xsl:with-param name="col" select="$col + 1" />
                    </xsl:call-template>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:call-template name="OutputNodes">
                        <xsl:with-param name="nodes" select="$upcommingNodes[umbraco.library:DateGreaterThanOrEqual(./dateFrom, umbraco.library:DateAdd($startDate, 'm', 4)) and umbraco.library:DateGreaterThanOrEqual(umbraco.library:DateAdd($startDate, 'm', 7), ./dateFrom)]" />
                        <xsl:with-param name="startDate" select="umbraco.library:DateAdd($startDate, 'm', 4)" />
                        <xsl:with-param name="col" select="$col + 1" />
                    </xsl:call-template>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:if>
    </xsl:template>


    <xsl:template name="buildLi">
        <xsl:param name="month" />
        <xsl:param name="year" />
        <xsl:param name="nodesToShow" />

        <li class="event-monthname titleContainerText">

            <xsl:value-of select="$month"/>&#160;<xsl:value-of select="$year"/>

        </li>
        <xsl:if test="count($nodesToShow) = 0">
            <li>
                <p class="noEvent">
                    <span>Ingen begivenheder</span>
                </p>
            </li>
        </xsl:if>
        <xsl:for-each select="$nodesToShow">
            <xsl:sort select="dateFrom" order="ascending"/>
            <li>
                <p>
                    <span>
                        <xsl:choose>
                            <xsl:when test="upac:FormatIsoDateShort(./dateFrom) = upac:FormatIsoDateShort(./dateTo)">
                                <xsl:value-of select="upac:FormatIsoDateShort(./dateFrom)"/>
                                <xsl:text> - </xsl:text>
                                <xsl:value-of select="upac:FormatIsoTime(./dateFrom)"/>
                                <xsl:text> - </xsl:text>
                                <xsl:value-of select="upac:FormatIsoTime(./dateTo)"/>
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="upac:FormatIsoDateShort(./dateFrom)"/>
                                <xsl:text> - </xsl:text>
                                <xsl:value-of select="upac:FormatIsoDateShort(./dateTo)"/>
                            </xsl:otherwise>
                        </xsl:choose>
                    </span>
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
                </p>
                <xsl:comment></xsl:comment>
            </li>
        </xsl:for-each>
    </xsl:template>
</xsl:stylesheet>