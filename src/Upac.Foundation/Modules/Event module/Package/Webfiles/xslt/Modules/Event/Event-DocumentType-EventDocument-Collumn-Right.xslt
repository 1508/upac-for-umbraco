<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
    <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:upac="urn:upac" xmlns:kmh="urn:kmh"
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets upac kmh">


    <xsl:output method="xml" omit-xml-declaration="yes"/>

    <xsl:param name="currentPage"/>

    <xsl:template match="/">
        <xsl:if test="string($currentPage/@id)">                   
            <div class="grid-three grid-first shadow3">
                <div class="grid-content">
                    <div class="event-module box">
                        <div class="headerbox">
                            <h2>
                                <xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-facts')"/>
                            </h2>
                        </div>
                        <div class="contentbox">

                            <xsl:if test="string($currentPage/dateTo)">
                                <span>
                                    <xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-facts-date')"/>
                                </span>
                                <br /> 
                                <xsl:choose>
                                    <xsl:when test="substring($currentPage/dateFrom, 1, 10) = substring($currentPage/dateTo, 1, 10)">
                                        <xsl:value-of select="upac:FormatIsoDateShort($currentPage/dateFrom)"/>
                                        <xsl:text> - </xsl:text>
                                        <xsl:value-of select="upac:FormatIsoTime($currentPage/dateFrom)"/> - <xsl:value-of select="upac:FormatIsoTime($currentPage/dateTo)"/>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <xsl:value-of select="upac:FormatIsoDateShort($currentPage/dateFrom)"/>
                                        <xsl:text> - </xsl:text>
                                        <xsl:value-of select="upac:FormatIsoTime($currentPage/dateFrom)"/> -
                                        <br/>
                                        <xsl:value-of select="upac:FormatIsoDateShort($currentPage/dateTo)"/>
                                        <xsl:text> - </xsl:text>
                                        <xsl:value-of select="upac:FormatIsoTime($currentPage/dateTo)"/>
                                    </xsl:otherwise>
                                </xsl:choose>
                                <br />
                                <br />
                            </xsl:if>
                           
                            <xsl:if test="string($currentPage/place)">
                                <span>
                                    <xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-facts-place')"/>
                                </span>
                                <br />
                                <xsl:value-of select="umbraco.library:ReplaceLineBreaks($currentPage/place)" disable-output-escaping="yes" />
                                <br />
                                <br />
                            </xsl:if>

                            <xsl:if test="string($currentPage/contact)">
                                <span>
                                    <xsl:value-of select="umbraco.library:GetDictionaryItem('EventModule-Property-contact')"/>
                                    <xsl:text>: </xsl:text>
                                </span>
                                <br />                                
                                <xsl:value-of select="umbraco.library:ReplaceLineBreaks($currentPage/contact)" disable-output-escaping="yes" />
                                <br />
                                <br />
                            </xsl:if>
                            
                        </div>
                    </div>
                </div>
            </div>

        </xsl:if>
    </xsl:template>

</xsl:stylesheet>