Param(
    $reporter,
    $runner,
    $terget,
    $output,
    $type
)

## /ReportType HTML JSON
& $reporter cover /TargetExecutable="$runner" /TargetArguments="$target" /Output="$output" /ReportType="$type"