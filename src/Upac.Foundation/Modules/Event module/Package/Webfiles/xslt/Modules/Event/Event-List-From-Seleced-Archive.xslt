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
	<xsl:variable name="linkToContainer" select="string(/macro/linkToContainerText)"/>
	<xsl:variable name="linkToContainerText" select="/macro/linkToContainerText"/>
	<xsl:variable name="showRssLink" select="upac:ToBool(/macro/showRssLink)"/>
	<xsl:variable name="category" select="/macro/category"/>
	<xsl:variable name="title" select="/macro/title"/>
	<xsl:variable name="containerNodeId" select="$currentPage/ancestor-or-self::*[@level=1]/ConfigurationContainer/ConfigurationSite/defaultCalenderArchive" />

	<xsl:template match="/">
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
							<div class="grid-one event opacity campaign">
								<xsl:apply-templates select="$container" />
							</div>
						</xsl:otherwise>
					</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="ShowNodes" match="EventModule">
		<xsl:variable name="nodes" select="./*/*/EventDocument[string(./dateFrom) and umbraco.library:DateGreaterThanOrEqualToday(./dateFrom) and ($category = '' or contains(./category, $category))]"/>

		<xsl:choose>
			<xsl:when test="count($nodes) = 0">
				<xsl:call-template name="Error">
					<xsl:with-param name="message" select="concat('Could not find published node with id: ', $containerNodeId)" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="titleTag" select="upac:GetImageTag(/macro/containerImage/Image/@id)" />
				<xsl:choose>
					<xsl:when test="string(titleTag)">
						<div class="titleimage">
							<xsl:value-of select="$titleTag" disable-output-escaping="yes"/>
						</div>
					</xsl:when>
                    <xsl:when test="string($title)">
                        <div class="titleContainerText">
                            <xsl:value-of select="$title"/>
                            <xsl:comment></xsl:comment>
                        </div>
                    </xsl:when>
					<xsl:otherwise>
						<div class="titleContainer titleEvents">
							<span class="accessibility">
								<xsl:value-of select="$title"/>
							</span>
							<xsl:comment></xsl:comment>
						</div>
					</xsl:otherwise>
				</xsl:choose>

				<ul>
					<xsl:for-each select="$nodes">
						<xsl:sort select="./dateFrom" order="ascending" data-type="text"/>
						<xsl:if test="position() &lt;= $maxNodesTotal">
							<li>
								<p>
									<span>
										<xsl:call-template name="WriteEventDate">
											<xsl:with-param name="dateFrom" select="./dateFrom" />
											<xsl:with-param name="dateTo" select="./dateTo" />
										</xsl:call-template>
									</span>
									<br/>
									<a href="{upac:UrlViaNodeId(./@id)}">
										<xsl:value-of select="upac:MenuTitle(.)"/>
									</a>
								</p>
								<xsl:comment></xsl:comment>
							</li>
						</xsl:if>
					</xsl:for-each>
					<xsl:comment></xsl:comment>
				</ul>
				<xsl:if test="$linkToContainer or $showRssLink">
					<div class="endlink">
						<xsl:if test="$linkToContainer or $showRssLink">
							<xsl:if test="$linkToContainer">
								<xsl:variable name="linktext">
									<xsl:choose>
										<xsl:when test="string($linkToContainerText)">
											<xsl:value-of select="$linkToContainerText"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-facts-goToArchive')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<xsl:variable name="linkUrl">
									<xsl:choose>
										<xsl:when test="string($category)">
											<xsl:value-of select="concat(upac:UrlViaNodeId($containerNodeId), '?category=', $category)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="upac:UrlViaNodeId($containerNodeId)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>

								<a href="{$linkUrl}">
									<xsl:value-of select="$linktext"/>
								</a>
							</xsl:if>
							<xsl:if test="$showRssLink">
								<xsl:if test="$linkToContainer">
									<br/>
								</xsl:if>
								<a class="rss" href="{upac:UrlViaNodeId($containerNodeId, 'RSS')}">
									<xsl:value-of select="umbraco.library:GetDictionaryItem('Global-RSS')"/>
									<span class="accessibility">
										&#160;<xsl:value-of select="umbraco.library:GetDictionaryItem('Event-document-name')"/>
									</span>
									<xsl:comment></xsl:comment>
								</a>
							</xsl:if>
						</xsl:if>
						<xsl:comment></xsl:comment>
					</div>
				</xsl:if>
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