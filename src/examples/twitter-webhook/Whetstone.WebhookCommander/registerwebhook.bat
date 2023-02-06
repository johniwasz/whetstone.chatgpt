  REM ngrok http http://localhost:7070

REM bin\Debug\net6.0\twithookconfig list webhooks --e devchatgpt
REM bin\Debug\net6.0\twithookconfig removewebhook --e devchatgpt --wi 1000000000000
bin\Debug\net6.0\twithookconfig addwebhook --e devchatgpt --webhookurl https://fn-myfunc.azurewebsites.net/api/chatgptdm
bin\Debug\net6.0\twithookconfig subscribe --e devchatgpt