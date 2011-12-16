Der skal indsættes to appSettings i web.config
Det er API key og Client ID: http://www.campaignmonitor.com/api/required/
<configuration>
	<appSettings>
		<add key="Upac.CampaignMonitor.ApiKey" value="API KEY"/>
		<add key="Upac.CampaignMonitor.ApiClientId" value="CLIENT ID"/>
	</appSettings>
</configuration>