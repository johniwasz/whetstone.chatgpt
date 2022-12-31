
$resourceGroupName = "twitter-chatgpt"
New-AzResourceGroup `
  -Name $resourceGroupName `
  -Location "East US"

  $templateFile = ".\akvdeployment.json"

  $twitterAccessToken = ConvertTo-SecureString $Env:TWITTER_ACCESS_TOKEN -AsPlainText -Force
  $twitterAccessTokenSecret = ConvertTo-SecureString $Env:TWITTER_ACCESS_TOKEN_SECRET -AsPlainText -Force
  $twitterConsumerKey = ConvertTo-SecureString $Env:TWITTER_CONSUMER_KEY -AsPlainText -Force
  $twitterConsumerSecret = ConvertTo-SecureString $Env:TWITTER_CONSUMER_SECRET -AsPlainText -Force

  New-AzResourceGroupDeployment `
    -Name twitter-chatgpt-resources `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile $templateFile `
    -tenantId $Env:AZURE_TENANT_ID `
    -objectId $Env:AZURE_OBJECT_ID `
    -twitterAccessToken $twitterAccessToken `
    -twitterAccessTokenSecret $twitterAccessTokenSecret `
    -twitterConsumerKey $twitterConsumerKey `
    -twitterConsumerSecret $twitterConsumerSecret

   