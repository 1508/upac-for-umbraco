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

	<xsl:variable name="containerNodeId" select="/macro/container"/>
	<xsl:variable name="nodesInFocus" select="number(/macro/nodesInFocus)"/>
	<xsl:variable name="maxNodesTotal" select="number(/macro/maxNodesTotal)"/>
	<xsl:variable name="linkToContainer" select="string(/macro/linkToContainer) = '1'"/>
	<xsl:variable name="linkToContainerText" select="/macro/linkToContainerText"/>
	<xsl:variable name="showRssLink" select="string(/macro/showRssLink) = '1'"/>
	<xsl:variable name="title" select="/macro/title"/>
	
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="string($containerNodeId) = false()">
				<xsl:call-template name="Error">
					<xsl:with-param name="message" select="'Please select a node'" />
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
						<xsl:variable name="nodes" select="$container/*/*/PublicationDocument" />
						<div class="item">
							<div class="grid_six grid_first">
								<xsl:if test="string($title)">
									<h1>
										<xsl:value-of select="$title"/>
									</h1>
								</xsl:if>
								<div class="grid_three grid_first">
									<xsl:if test="count($nodes) &gt;= $nodesInFocus">
										<ul>
											<xsl:for-each select="$nodes">
												<xsl:sort select="date" order="descending" data-type="text"/>
												<xsl:if test="position() &lt;= $nodesInFocus">
													<li>
														<a href="{upac:UrlViaNodeId(./@id)}">
															<xsl:value-of select="upac:MenuTitle(.)"/>
														</a>
														<br />
														<span class="shortintrolist">
															<xsl:value-of select="concat(umbraco.library:ReplaceLineBreaks(./shortIntro), ' ')"/>
														</span>
														<span class="date">
															<xsl:value-of select="upac:FormatIsoDateShort(./date)"/>
														</span>
													</li>
												</xsl:if>
											</xsl:for-each>
										</ul>
									</xsl:if>
								</div>
								<div class="grid_three">
									<xsl:if test="count($nodes) &gt; $nodesInFocus">
										<ul>
											<xsl:for-each select="$nodes">
												<xsl:sort select="date" order="descending" data-type="text"/>
												<xsl:if test="position() &gt; $nodesInFocus and position() &lt;= $maxNodesTotal">
													<li>
														<a href="{upac:UrlViaNodeId(./@id)}">
															<xsl:value-of select="upac:MenuTitle(.)"/>
														</a>
														<span>
															<xsl:value-of select="upac:FormatIsoDateShort(./date)"/>
														</span>
													</li>
												</xsl:if>
											</xsl:for-each>
										</ul>
									</xsl:if>
								</div>
								<div class="more_rss">
									<xsl:if test="$linkToContainer">
										<a href="{upac:UrlViaNodeId($containerNodeId, 'RSS')}" class="rss">
											<xsl:comment></xsl:comment>
										</a>
										<a href="{upac:UrlViaNodeId($containerNodeId)}">
											<xsl:value-of select="$linkToContainerText"/>
										</a>
									</xsl:if>
								</div>
							</div>
						</div>
					</xsl:otherwise>
				</xsl:choose>
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