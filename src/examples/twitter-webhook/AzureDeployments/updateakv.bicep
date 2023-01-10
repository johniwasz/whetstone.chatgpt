
@description('Location for all resources.')
param funcName string

@description('Location for all resources.')
param location string = resourceGroup().location

@minLength(3)
@maxLength(24)
@description('Name of the keyvault that stores Twitter and OpenAI credentials.')
param keyVaultName string 

var tenantId = tenant().tenantId


resource function 'Microsoft.Web/sites@2020-12-01' existing = {
  name: funcName
}

resource twitterchatgpt_dev_kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName
  location: location
  tags: {
    displayName: 'twitterchatgpt'
  }
  properties: {
    tenantId: tenantId
    accessPolicies: [
      {
        tenantId: tenantId
        objectId: function.identity.principalId
        permissions: {
          secrets: [
            'get'
          ]
        }
      }
    ]
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}
