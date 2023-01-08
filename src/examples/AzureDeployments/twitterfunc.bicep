
@description('The name of the function app that you wish to create.')
param appName string = 'fn-twitgpt-${uniqueString(resourceGroup().id)}'

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Twitter credentials in JSON.')
@secure()
param twitterCredsJson string

@description('OpenAI credentials in JSON.')
@secure()
param openAIAPICredsJson string

@description('region of the resouce group.')
param twitgptgrouppname string = 'eastus'

param storageAccountType string = 'Standard_LRS'


var keyVaultName = 'kvgpt-dev-${uniqueString(resourceGroup().id)}'

var tenantId = tenant().tenantId
var hostingPlanName = appName
var applicationInsightsName = appName
var storageAccountName = '${uniqueString(resourceGroup().id)}azfunctions'

resource twitterchatgpt_dev_kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName
  location: twitgptgrouppname
  tags: {
    displayName: 'twitterchatgpt'
  }
  properties: {
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    tenantId: tenantId
    enableSoftDelete: true
    accessPolicies: []
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}

resource twitterchatgpt_dev_kv_twittercreds 'Microsoft.KeyVault/vaults/secrets@2016-10-01' = {
  parent: twitterchatgpt_dev_kv
  name: 'twittercreds'
  tags: {
    displayName: 'twitterchatgpt'
  }
  properties: {
    value: twitterCredsJson
  }
}

resource twitterchatgpt_dev_kv_openaiapicreds 'Microsoft.KeyVault/vaults/secrets@2016-10-01' = {
  parent: twitterchatgpt_dev_kv
  name: 'openaicreds'
  tags: {
    displayName: 'twitterchatgpt'
  }
  properties: {
    value: openAIAPICredsJson
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: storageAccountName
  location: location
  tags: {
    displayName: 'twitterchatgpt'
  }
  sku: {
    name: storageAccountType    
  }
  kind: 'Storage'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  tags: {
    displayName: 'twitterchatgpt'
  }
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource function 'Microsoft.Web/sites@2020-12-01' = {
  name: appName
  location: twitgptgrouppname
  kind: 'functionapp'
  tags: {
    displayName: 'twitterchatgpt'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true      
      appSettings: [
        {
          name: 'AzureWebJobsDashboard'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccount.listKeys().keys[0].value}'          
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(appName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'TWITTER_CREDS'
          value: '@Microsoft.KeyVault(VaultName=${twitterchatgpt_dev_kv.name};SecretName=${twitterchatgpt_dev_kv_twittercreds.name})'
        }
        {
          name: 'OPENAI_API_CREDS'
          value: '@Microsoft.KeyVault(VaultName=${twitterchatgpt_dev_kv.name};SecretName=${twitterchatgpt_dev_kv_openaiapicreds.name})'
        }
      ]
    }
    httpsOnly: true
  }
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  tags: {
    displayName: 'twitterchatgpt'
  }
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}

module appService 'updateakv.bicep' = {
  name: 'appService'
  params: {
    funcName: function.name
    location: location
    keyVaultName: keyVaultName
  }
}

output createdFunction string = function.name
