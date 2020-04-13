# Phanerozoic
Parse, Update and Notify.


## Naming
> [顯生宙](https://zh.wikipedia.org/wiki/%E6%98%BE%E7%94%9F%E5%AE%99)  

## Step
1. Git Pull develop	取得程式碼
2. MSBuild			建置
3. dotCover			計算涵蓋率
4. Phanerozoic		更新涵蓋率

## Coverage Tool
### dotcover
```
../../../../../lib/JetBrains.dotCover.CommandLineTools.2019.2.3/dotcover cover /TargetExecutable="C:\Program Files\dotnet\dotnet.exe" /TargetArguments="test ./Phanerozoic.Core.Test.dll" /Output="AppCoverageReport.html" /ReportType="HTML"
```
- ReportType  
[HTML|JSON|XML|DetailedXML|NDependXML]. A type of the report. XML by default

> [Download dotCover](https://www.jetbrains.com/dotcover/download/#section=commandline)  
> [Coverage Analysis from the Command Line](https://www.jetbrains.com/help/dotcover/Running_Coverage_Analysis_from_the_Command_LIne.html)  
> [report](https://www.jetbrains.com/help/dotcover/dotCover__Console_Runner_Commands.html#report)  

### coverlet
```
# 安裝
dotnet tool install --global coverlet.console

# 執行
coverlet "dotnet" --target "Phanerozoic.Core.Test.dll"
```

> [GitHub](https://github.com/tonerdo/coverlet)  
> [Docker 教學 - .NET Core 測試報告 (Coverlet + ReportGenerator)](https://blog.johnwu.cc/article/docker-dotnet-coverage-report-generator.html)  

#### ReportGenerator
```
# 安裝
dotnet tool install -g dotnet-reportgenerator-globaltool

# 執行
reportgenerator -reports:./output/coverage.opencover.xml -reporttypes:Html -targetdir:./output/report
```
> [GitHub](https://github.com/danielpalme/ReportGenerator)  

## Parse  
涵蓋率格式
- dotCover
- opencover
> [OpenCover 介紹篇](https://ithelp.ithome.com.tw/articles/10187410)

## Update  
### Google Sheet
> [Google Sheets API](https://developers.google.com/sheets/api/quickstart/dotnet)  
> [Google Sheets API v4 - How to get the last row with value?](https://stackoverflow.com/questions/37331756/google-sheets-api-v4-how-to-get-the-last-row-with-value)

## Notify  
### Slack  
> [Sending messages using Incoming Webhooks](https://api.slack.com/messaging/webhooks)  
> [Block Kit Builder](https://api.slack.com/tools/block-kit-builder)  
> [Message Formatting](https://api.slack.com/docs/messages/builder)  
> [Formatting text for app surfaces](https://api.slack.com/reference/surfaces/formatting#linking_to_channels_and_users)

### Email
> [~~SmtpClient~~](https://docs.microsoft.com/zh-tw/dotnet/api/system.net.mail.smtpclient?view=netcore-3.1)  
> [MailKit](https://github.com/jstedfast/MailKit)

#### Gmail
> [Sign in using App Passwords](https://support.google.com/accounts/answer/185833?hl=en&ctx=ch_DisplayUnlockCaptcha)