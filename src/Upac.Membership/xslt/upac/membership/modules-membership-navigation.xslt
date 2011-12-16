<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
	<!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:upac="urn:upac"  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets upac ">
	<xsl:output method="xml" omit-xml-declaration="yes" indent="yes"/>

	<xsl:param name="currentPage"/>
	<!--
		CPA SAYS 2010-03-10 : OBS hvis der laves ændringer/regler i denne fil, så skal man stærkt overveje om disse ændringer skal med i UPac!
	-->

	<xsl:template match="/">
		<xsl:variable name="membershipEnabled" select="string(upac:Setting('Membership/Enabled')) = '1'" />
		<xsl:if test="$membershipEnabled">
			<div class="MembershipNavigation">
				<ul>
					<xsl:choose>
						<xsl:when test="umbraco.library:IsLoggedOn()">
							<xsl:variable name="member" select="umbraco.library:GetCurrentMember()" />
							<li class="MembershipNavigationUser">
								<xsl:value-of select="upac:Setting('Membership/Navigation/YouAreLoggedInAsTextPrefix')"/>
								<xsl:value-of select="' '"/>
								<a href="{upac:Setting('Membership/YourProfileUrl')}">
									<xsl:choose>
										<xsl:when test="string($member/data[@alias='nickname'])">
											<xsl:value-of select="$member/data[@alias='nickname']"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$member/@nodeName"/>
										</xsl:otherwise>
									</xsl:choose>
								</a>
								<xsl:value-of select="' '"/>
								<xsl:value-of select="upac:Setting('Membership/Navigation/YouAreLoggedInAsTextSuffix')"/>
							</li>
							<li class="MembershipNavigationLogout">
								<a href="{upac:Setting('Membership/LogoutUrl')}">
									<xsl:value-of select="upac:Setting('Membership/Navigation/LogoutText')"/>
								</a>
							</li>
						</xsl:when>
						<xsl:otherwise>
							<li class="MembershipNavigationLogin">
								<a href="{upac:Setting('Membership/LoginUrl')}">
									<xsl:value-of select="upac:Setting('Membership/Navigation/LoginText')"/>								
								</a>
							</li>
							<li class="MembershipNavigationCreateUser">
								<a href="{upac:Setting('Membership/CreateUserUrl')}">
									<xsl:value-of select="upac:Setting('Membership/Navigation/CreateUserText')"/>
								</a>
							</li>
						</xsl:otherwise>
					</xsl:choose>
				</ul>
			</div>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>