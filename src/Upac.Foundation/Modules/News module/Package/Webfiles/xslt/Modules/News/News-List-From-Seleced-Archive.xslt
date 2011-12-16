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

    <xsl:include href="../../include.xslt"/>

    <xsl:variable name="maxNodesTotal" select="upac:ToNumber(/macro/maxNodesTotal, 5)"/>
    <xsl:variable name="nodesInFocus" select="upac:ToNumber(/macro/nodesInFocus, 0)" />
    <xsl:variable name="linkToContainer" select="string(/macro/linkToContainerText)"/>
    <xsl:variable name="linkToContainerText" select="/macro/linkToContainerText"/>
    <xsl:variable name="showRssLink" select="upac:ToBool(/macro/showRssLink)"/>
    <xsl:variable name="title" select="/macro/title"/>
    <xsl:variable name="containerNodeId" select="$currentPage/ancestor-or-self::*[@level=1]/ConfigurationContainer/ConfigurationSite/defaultNewsArchive" />

    <xsl:template match="/">
        <div class="grid-one news opacity campaign">
            <xsl:choose>
                <xsl:when test="string($containerNodeId) = false()">
                    <xsl:call-template name="Error">
                        <xsl:with-param name="message" select="concat('Could not find published node with id: ', $containerNodeId)" />
                    </xsl:call-template>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:variable name="container" select="umbraco.library:GetXmlNodeById($containerNodeId)" />
                    <xsl:choose>
                        <xsl:when test="string($container/@id) = false()">
                            <xsl:call-template name="Error">
                                <xsl:with-param name="message" select="concat('Could not find published node with id: ', $containerNodeId)" />
                            </xsl:call-template>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:apply-templates select="$container" />
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:otherwise>
            </xsl:choose>
        </div>
    </xsl:template>

    <xsl:template name="ShowNodes" match="NewsModule">
        <xsl:variable name="nodes" select="./*/*/NewsDocument"/>

        <xsl:choose>
            <xsl:when test="string($nodes) = false()">
                <xsl:call-template name="Error">
                    <xsl:with-param name="message" select="'Please select a node'" />
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:if test="string($title)">
                    <div class="titleContainerText">
                        <xsl:value-of select="$title"/>
                        <xsl:comment></xsl:comment>
                    </div>
                </xsl:if>
                <xsl:if test="count($nodes)">
                    <ul class="clearfix">
                        <xsl:if test="$nodesInFocus &gt;0">
                            <xsl:for-each select="$nodes">
                                <xsl:sort select="date" order="descending" data-type="text"/>
                                <xsl:if test="position() &lt;= $nodesInFocus">
                                    <li>
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
                                </xsl:if>
                            </xsl:for-each>
                            <xsl:comment></xsl:comment>
                        </xsl:if>
                        <xsl:for-each select="$nodes">
                            <xsl:sort select="date" order="descending" data-type="text"/>
                            <xsl:if test="(position() &gt; $nodesInFocus) and (position() &lt;= $maxNodesTotal)">
                                <li>
                                    <p>
                                        <a href="{upac:UrlViaNodeId(./@id)}">
                                            <xsl:value-of select="upac:MenuTitle(.)"/>
                                        </a>

                                        <span>
                                            <xsl:value-of select="upac:FormatIsoDateShort(./date)"/>
                                        </span>
                                    </p>
                                    <xsl:comment></xsl:comment>
                                </li>
                            </xsl:if>
                        </xsl:for-each>
                    </ul>
                </xsl:if>
                <div class="endlink">
                    <xsl:if test="$linkToContainer or $showRssLink">
                        <xsl:if test="$linkToContainer">
                            <xsl:variable name="linktext">
                                <xsl:choose>
                                    <xsl:when test="string($linkToContainerText)">
                                        <xsl:value-of select="$linkToContainerText"/>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:value-of select="umbraco.library:GetDictionaryItem('NewsModule-GoTo Archive')"/>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>

                            <xsl:variable name="linkUrl">
                                <xsl:value-of select="upac:UrlViaNodeId($containerNodeId)"/>
                            </xsl:variable>

                            <a href="{$linkUrl}">
                                <xsl:value-of select="$linktext"/>
                            </a>
                        </xsl:if>
                        <xsl:if test="$showRssLink">
                            <a class="rss" href="{upac:UrlViaNodeId($containerNodeId, 'RSS')}">
                                <xsl:value-of select="umbraco.library:GetDictionaryItem('Global-RSS')"/>
                                <span class="accessibility">
                                    &#160;<xsl:value-of select="umbraco.library:GetDictionaryItem('NewsModule-document-name')"/>
                                </span>
                                <xsl:comment></xsl:comment>
                            </a>
                        </xsl:if>
                    </xsl:if>
                    <xsl:comment></xsl:comment>
                </div>
                <xsl:comment></xsl:comment>
            </xsl:otherwise>
        </xsl:choose>

    </xsl:template>

    <xsl:template name="Error">
        <xsl:param name="message" />
        <div style="border: 3px solid red;">
            <xsl:value-of select="$message"/>
        </div>
    </xsl:template>

</xsl:stylesheet>