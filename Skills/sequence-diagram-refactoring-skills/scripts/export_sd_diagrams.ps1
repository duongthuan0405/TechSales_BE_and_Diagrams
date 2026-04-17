$base_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\Diagrams"
$img_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\Skills\img"

# Create output directory if it doesn't exist
if (-not (Test-Path $img_dir)) {
    New-Item -ItemType Directory -Force -Path $img_dir | Out-Null
    Write-Host "Created directory: $img_dir" -ForegroundColor Green
}

$filesProcessed = 0

Write-Host "Starting export of 61 diagrams via Kroki API (SDxx-re.txt -> SDxx-re.png)... This will take a moment." -ForegroundColor Cyan

for ($i=1; $i -le 61; $i++) {
    $folder = "UC{0:D2}" -f $i
    $fileName = "SD{0:D2}-re.txt" -f $i
    $filePath = Join-Path $base_dir $folder | Join-Path -ChildPath $fileName
    
    if (Test-Path $filePath) {
        $content = Get-Content -Path $filePath -Raw -Encoding UTF8
        
        $outFileName = "SD{0:D2}-re.png" -f $i
        $outFilePath = Join-Path $img_dir $outFileName
        
        try {
            # Use Kroki open source API to render the PlantUML code directly to PNG
            Invoke-RestMethod -Uri "https://kroki.io/plantuml/png" -Method Post -Body $content -ContentType "text/plain; charset=utf-8" -OutFile $outFilePath
            
            Write-Host "Exported: $fileName -> $outFileName" -ForegroundColor Green
            $filesProcessed++
        }
        catch {
            Write-Host "Error exporting $fileName : $_" -ForegroundColor Red
        }
    } else {
        Write-Host "File not found: $filePath" -ForegroundColor Yellow
    }
}

Write-Host "`nSuccessfully exported $filesProcessed SD diagrams to $img_dir!" -ForegroundColor Cyan
