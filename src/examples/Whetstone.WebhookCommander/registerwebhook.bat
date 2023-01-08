  REM ngrok http http://localhost:7070

REM bin\Debug\net6.0\twithookconfig removewebhook --e devchatgpt --wi 1611802887591546883
bin\Debug\net6.0\twithookconfig addwebhook --e devchatgpt --webhookurl https://fn-twitgpt-uqvq5cmh43iws.azurewebsites.net/api/chatgptdm
bin\Debug\net6.0\twithookconfig subscribe --e devchatgpt