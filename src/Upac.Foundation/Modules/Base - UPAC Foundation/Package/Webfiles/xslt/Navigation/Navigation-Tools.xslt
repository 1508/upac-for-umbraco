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


    <xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>

    <xsl:param name="currentPage"/>

    <xsl:template match="/">
        <xsl:variable name="useAddThis" select="string($currentPage/ancestor-or-self::*[@level=1]/ConfigurationContainer/ConfigurationAddThis/useAddThis) = '1'" />
        <div class="tools clearfix">
            <hr class="accessibility" />
            <p class="accessibility">
                <strong>
                    <xsl:value-of disable-output-escaping="yes" select="upac:GetDictionaryItem('Accessibility-PageTools')"/>
                </strong>
            </p>
            <ul>
                <xsl:if test="$useAddThis">
                    <li>
                        <xsl:if test="$currentPage/ancestor-or-self::*[@level=1]/ConfigurationContainer/ConfigurationAddThis/useAddThis">
                            <a class="addthis_button share" href="/">
                                <script type="text/javascript">
                                    <xsl:value-of select="$currentPage/ancestor-or-self::*[@level=1]/ConfigurationContainer/ConfigurationAddThis/addthisScript" />
                                </script>
                            </a>
                        </xsl:if>
                    </li>
                </xsl:if>
                <li>
                    <a class="read" href="{umbraco.library:GetDictionaryItem('Global-ReadAloud-Link')}" title="{umbraco.library:GetDictionaryItem('Global-ReadAloud-Text')}">
                        <span class="accessibility">
                            <xsl:value-of disable-output-escaping="yes" select="upac:GetDictionaryItem('Global-ReadAloud-Text')"/>
                        </span>
                    </a>
                </li>
                <xsl:choose>
                    <xsl:when test="$useAddThis">
                        <li>
                            <a class="addthis_button_print print" href="#" title="{umbraco.library:GetDictionaryItem('Global-PrintPage')}">
                                <span class="accessibility">
                                    <xsl:value-of disable-output-escaping="yes" select="upac:GetDictionaryItem('Global-PrintPage')"/>
                                </span>
                            </a>
                        </li>
                    </xsl:when>
                    <xsl:otherwise>
                        <li>
                            <a href="#" onclick="window.print(); return false;" class="print" title="{umbraco.library:GetDictionaryItem('Global-PrintPage')}">
                                <span class="accessibility">
                                    <xsl:value-of disable-output-escaping="yes" select="upac:GetDictionaryItem('Global-PrintPage')"/>
                                </span>
                            </a>
                        </li>
                    </xsl:otherwise>
                </xsl:choose>
            </ul>
        </div>
    </xsl:template>

</xsl:stylesheet>