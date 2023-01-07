
$resourceGroupName = "twitter-chatgpt"
New-AzResourceGroup `
  -Name $resourceGroupName `
  -Location "East US"

  # $templateFile = ".\akvdeployment.json"
  $templateFile = ".\akvdeployment.bicep"

  $twitterAccessToken = ConvertTo-SecureString $Env:TWITTER_ACCESS_TOKEN -AsPlainText -Force
  $twitterAccessTokenSecret = ConvertTo-SecureString $Env:TWITTER_ACCESS_TOKEN_SECRET -AsPlainText -Force
  $twitterConsumerKey = ConvertTo-SecureString $Env:TWITTER_CONSUMER_KEY -AsPlainText -Force
  $twitterConsumerSecret = ConvertTo-SecureString $Env:TWITTER_CONSUMER_SECRET -AsPlainText -Force
  $tenantId = ConvertTo-SecureString $Env:AZURE_TENANT_ID -AsPlainText -Force
  $objectId = ConvertTo-SecureString $Env:AZURE_OBJECT_ID -AsPlainText -Force

  New-AzResourceGroupDeployment `
    -Name twitter-chatgpt-resources `
    -ResourceGroupName $resourceGroupName `
    -TemplateFile $templateFile `
    -tenantId $tenantId `
    -objectId $objectId `
    -twitterAccessToken $twitterAccessToken `
    -twitterAccessTokenSecret $twitterAccessTokenSecret `
    -twitterConsumerKey $twitterConsumerKey `
    -twitterConsumerSecret $twitterConsumerSecret

   