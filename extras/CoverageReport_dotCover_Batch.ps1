$targetArray = @(
    # WebStore
    #"D:\Code\Taiwan\NineYi.WebStore.MobileWebMall\WebStore\Frontend\BLV2.Test.Xunit\bin\Debug\NineYi.WebStore.Frontend.BLV2.Test.Xunit.dll"
    # ApiV2
    #"D:\Code\Taiwan\NineYi.Scm.ApiV2\Test\BL.Service.Test.Xunit\NineYi.Scm.ApiV2.BL.Services.Test.Xunit\bin\Debug\NineYi.Scm.ApiV2.BL.Services.Test.Xunit.dll"
    # NMQ
    #"D:\Code\Taiwan\NineYi.Scm.NMQV2\Test\ERP\Backend\BLV2.Test.Xunit\bin\Debug\NineYi.ERP.Backend.BLV2.Test.Xunit.dll"
    #"D:\Code\Taiwan\NineYi.ERP.NMQV2\Test\ERP\Backend\BLV2.Test.Xunit\bin\Debug\NineYi.ERP.Backend.NMQV2.Test.Xunit.dll"
    # LoyaltyPoint
    "D:\Code\Service\NineYi.LoyaltyPoint\Test\BL.Services.Test\bin\Debug\NineYi.LoyaltyPoint.BL.Services.Test.dll"
    # OSMPlus
    #"D:\Code\Service\NineYi.OSMPlus.Console\NineYi.OsmPlus.BL.Service.Test\bin\Debug\NineYi.OsmPlus.BL.Service.Test.dll"
    #"D:\Code\Service\NineYi.OSMPlus.Console\NineYi.OsmPlus.Console.BL.Service.Test\bin\Debug\NineYi.OsmPlus.Console.BL.Service.Test.dll"
    # MemberTier
    #"test D:\Code\Service\NineYi.MemberTier\Test\NineYi.MemberTier.Service.Test\bin\Debug\netcoreapp3.0\NineYi.MemberTier.Service.Test.dll"
)     

$reporter = "..\lib\JetBrains.dotCover.CommandLineTools.2019.2.3\dotcover"
$runner = "D:\Code\Taiwan\NineYi.WebStore.MobileWebMall\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe"
#$runner = "C:\Program Files\dotnet\dotnet.exe"
$type = "html" # HTML JSON

$date = Get-Date -Format yyyyMMddHHmm

function dotCover 
{
    param(        
        $reporter,        
        $runner,        
        $target,        
        $output,        
        $type    
    )

    & $reporter cover /TargetExecutable="$runner" /TargetArguments="$target" /Output="$output" /ReportType="$type"
}

ForEach ($target in $targetArray) {
    $file = [System.IO.Path]::GetFileNameWithoutExtension($target)
    Write-Host "Process $file" -ForegroundColor Green

    $output = ".\output\$($date)_$file.$type"
    #Write-Host $output

    dotCover $reporter $runner $target $output $type
}


