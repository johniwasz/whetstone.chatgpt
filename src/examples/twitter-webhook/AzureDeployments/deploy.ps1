
$resourceGroupName = "twitter-chatgpt"

New-AzResourceGroup `
  -Name $resourceGroupName `
  -Location "East US"

$templateFile = ".\twitterfunc.bicep"

$twitterCredsJson = ConvertTo-SecureString $Env:TWITTER_CREDS -AsPlainText -Force
$OpenAIAPICreds = ConvertTo-SecureString $Env:OPENAI_API_CREDS -AsPlainText -Force

New-AzResourceGroupDeployment `
  -Name twitter-chatgpt-resources `
  -ResourceGroupName $resourceGroupName `
  -TemplateFile $templateFile `
  -twitterCredsJson $twitterCredsJson `
  -openAIAPICredsJson $OpenAIAPICreds `
  -overwriteFuncSettings $true