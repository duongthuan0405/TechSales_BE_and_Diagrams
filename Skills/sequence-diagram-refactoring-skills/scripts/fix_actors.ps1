$base_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\Diagrams"
$count = 0

# Use ASCII-safe regex (using dots for Vietnamese characters) to avoid PowerShell encoding errors
$pattern1 = '(?im)^(?:boundary|entity|control|participant|database)\s+("[^"]*(?:Gateway|C.ng thanh to.n|Bank|Ng.n h.ng|API|Email|Mail|Giao h.ng|Shipper|V.n chuy.n|Zalo|Momo|VNPay|B.n th. 3|H. th.ng ngo.i)[^"]*")\s+as\s+([a-zA-Z0-9_]+)'

$pattern2 = '(?im)^(?:boundary|entity|control|participant|database)\s+([a-zA-Z0-9_]*(?:Gateway|Bank|API|Email|Mail|Shipper|Zalo|Momo|VNPay)[a-zA-Z0-9_]*)(?:\s+as\s+([a-zA-Z0-9_]+))?$'

for ($i=1; $i -le 61; $i++) {
    $folder = "UC{0:D2}" -f $i
    $mainFile = "SD{0:D2}.txt" -f $i
    $mainPath = Join-Path $base_dir $folder | Join-Path -ChildPath $mainFile
    
    if (Test-Path $mainPath) {
        $content = Get-Content -Path $mainPath -Raw -Encoding UTF8
        
        # Replace quoted names
        $newContent = [System.Text.RegularExpressions.Regex]::Replace($content, $pattern1, 'actor $1 as $2')
        
        # Replace unquoted names
        $newContent = [System.Text.RegularExpressions.Regex]::Replace($newContent, $pattern2, {
            param($m)
            if ($m.Groups[2].Success) { return "actor $($m.Groups[1].Value) as $($m.Groups[2].Value)" }
            return "actor $($m.Groups[1].Value)"
        })
        
        if ($content -ne $newContent) {
            # Ensure utf-8 without BOM
            $utf8NoBom = New-Object System.Text.UTF8Encoding $False
            [System.IO.File]::WriteAllText($mainPath, $newContent, $utf8NoBom)
            Write-Host "Fixed third-party actors in $mainFile" -ForegroundColor Green
            $count++
        }
    }
}
Write-Host "Successfully fixed $count files!" -ForegroundColor Cyan
