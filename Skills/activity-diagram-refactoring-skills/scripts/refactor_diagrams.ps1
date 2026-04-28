$base_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\Diagrams"

$filesProcessed = 0
$filesChanged = 0

for ($i=1; $i -le 60; $i++) {
    $folder = "UC{0:D2}" -f $i
    $fileName = "AD{0:D2}.txt" -f $i
    $filePath = Join-Path $base_dir $folder | Join-Path -ChildPath $fileName
    
    if (Test-Path $filePath) {
        $content = Get-Content -Path $filePath -Raw -Encoding UTF8
        
        # Regex to match: :X. Action text; \s* if (Condition?) then (Branch)
        $pattern = '(?m):(\d+\.\s*[^;]+);\s*if\s*\([^)]+\)\s*then\s*\(([^)]+)\)'
        
        $evaluator = [System.Text.RegularExpressions.MatchEvaluator] {
            param($match)
            $actionText = $match.Groups[1].Value.Trim()
            if (-not $actionText.EndsWith('?')) {
                $actionText += '?'
            }
            $branch = $match.Groups[2].Value
            return "if ($actionText) then ($branch)"
        }
        
        $newContent = [System.Text.RegularExpressions.Regex]::Replace($content, $pattern, $evaluator)
        
        $newFileName = "AD{0:D2}-re.txt" -f $i
        $newFilePath = Join-Path $base_dir $folder | Join-Path -ChildPath $newFileName
        
        # Ensure utf-8 without BOM
        $utf8NoBom = New-Object System.Text.UTF8Encoding $False
        [System.IO.File]::WriteAllText($newFilePath, $newContent, $utf8NoBom)
        
        $filesProcessed++
        if ($newContent -ne $content) {
            $filesChanged++
            Write-Host "Refactored: $folder/$fileName -> $newFileName" -ForegroundColor Green
        } else {
            Write-Host "Copied unchanged: $folder/$fileName -> $newFileName" -ForegroundColor Gray
        }
    }
}

Write-Host "`nTotal files processed: $filesProcessed" -ForegroundColor Cyan
Write-Host "Total files refactored: $filesChanged" -ForegroundColor Cyan
