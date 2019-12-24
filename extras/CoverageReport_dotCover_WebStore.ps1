$output = ".\output\dotCover.html"
$dll = "D:\Code\Taiwan\NineYi.WebStore.MobileWebMall\WebStore\Frontend\BLV2.Test.Xunit\bin\Debug\NineYi.WebStore.Frontend.BLV2.Test.Xunit.dll"
$runner = "D:\Code\Taiwan\NineYi.WebStore.MobileWebMall\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe"
$report = "..\lib\JetBrains.dotCover.CommandLineTools.2019.2.3\dotcover"
$type = "HTML"


## /ReportType HTML JSON
& $report cover /TargetExecutable="$runner" /TargetArguments="$dll" /Output="$output" /ReportType="$type"