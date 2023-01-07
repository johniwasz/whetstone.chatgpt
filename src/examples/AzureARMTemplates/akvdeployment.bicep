
@secure()
param tenantId string

@description('The name of the function app that you wish to create.')
param appName string = 'fn-twitgpt-${uniqueString(resourceGroup().id)}'

@description('Location for all resources.')
param location string = resourceGroup().location

@description('Specifies the object ID of a user, service principal or security group in the Azure Active Directory tenant for the vault. The object ID must be unique for the list of access policies. Get it by using Get-AzADUser or Get-AzADServicePrincipal cmdlets.')
@secure()
param objectId string

@description('Twitter Access Token for invoking user-specific operations.')
@secure()
param twitterAccessToken string

@description('Twitter Access Token Secret for invoking user-specific operations.')
@secure()
param twitterAccessTokenSecret string

@description('Twitter Consumer Key for invoking public operations.')
@secure()
param twitterConsumerKey string

@description('Twitter Consumer Secret for invoking public operations.')
@secure()
param twitterConsumerSecret string

@description('region of the resouce group.')
param twitgptgrouppname string = 'eastus'

param storageAccountType string = 'Standard_LRS'

var hostingPlanName = appName
var applicationInsightsName = appName
var storageAccountName = '${uniqueString(resourceGroup().id)}azfunctions'


resource twitter_chatgpt_funcid 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: 'twitter-chatgpt-funcid'
  location: twitgptgrouppname
}

resource twitterchatgpt_dev 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: 'twitterchatgpt-dev'
  location: twitgptgrouppname
  tags: {
    displayName: 'twitterchatgpt'
  }
  properties: {
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    tenantId: tenantId
    accessPolicies: [
      {
        tenantId: tenantId
        objectId: objectId
        permissions: {
          keys: [
            'all'
          ]
          secrets: [
            'all'
          ]
        }
      }
      {
        tenantId: tenantId
        objectId: twitter_chatgpt_funcid.properties.principalId
        permissions: {
          secrets: [
            'Get'
          ]
        }
      }
    ]
    enableSoftDelete: true
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2018-09-01-preview' = {
  scope: twitterchatgpt_dev
  name: guid(twitterchatgpt_dev.id, twitter_chatgpt_funcid.id, 'Key Vault Secrets User')
  properties: {
    roleDefinitionId: '4633458b-17de-408a-b874-0445c86b69e6'
    principalId: twitter_chatgpt_funcid.properties.principalId
  }
}

resource twitterchatgpt_dev_twittercreds 'Microsoft.KeyVault/vaults/secrets@2016-10-01' = {
  parent: twitterchatgpt_dev
  name: 'twittercreds'
  properties: {
    value: '{ "AccessToken": "${twitterAccessToken}", "AccessTokenSecret": "${twitterAccessTokenSecret}", "ConsumerKey": "${twitterConsumerKey}", "ConsumerSecret": "${twitterConsumerSecret}"}'
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType    
  }
  kind: 'Storage'
  properties: {
    allowedCopyScope: 'AAD'
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: false
    networkAcls: {
      defaultAction: 'Deny'
    }
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource function 'Microsoft.Web/sites@2020-12-01' = {
  name: appName
  location: twitgptgrouppname
  kind: 'functionapp'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${twitter_chatgpt_funcid.id}' : {}
    }
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      ftpsState: 'Disabled'
      minTlsVersion: '1.2'
      http20Enabled: true
      keyVaultReferenceIdentity: twitter_chatgpt_funcid.id
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
          name: 'TWITTER_KEYS'
          value: '@MicrosoftValueSecret(${twitterchatgpt_dev_twittercreds.id})'
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
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
  }
}
