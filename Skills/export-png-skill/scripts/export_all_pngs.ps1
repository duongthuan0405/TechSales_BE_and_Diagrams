$base_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\Diagrams"
$ad_img_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\DiagramsPNG\ActivityDiagrams"
$sd_img_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\DiagramsPNG\SequenceDiagrams"

# Create output directories if they don't exist
if (-not (Test-Path $ad_img_dir)) {
    New-Item -ItemType Directory -Force -Path $ad_img_dir | Out-Null
    Write-Host "Created directory: $ad_img_dir" -ForegroundColor Green
}
if (-not (Test-Path $sd_img_dir)) {
    New-Item -ItemType Directory -Force -Path $sd_img_dir | Out-Null
    Write-Host "Created directory: $sd_img_dir" -ForegroundColor Green
}

$adProcessed = 0
$sdProcessed = 0

Write-Host "Starting export of all AD and SD diagrams via Kroki API... This will take a moment." -ForegroundColor Cyan

for ($i=1; $i -le 61; $i++) {
    $folder = "UC{0:D2}" -f $i
    
    # Process AD
    $adFileName = "AD{0:D2}.txt" -f $i
    $adFilePath = Join-Path $base_dir $folder | Join-Path -ChildPath $adFileName
    if (Test-Path $adFilePath) {
        $content = Get-Content -Path $adFilePath -Raw -Encoding UTF8
        $outFileName = "AD{0:D2}.png" -f $i
        $outFilePath = Join-Path $ad_img_dir $outFileName
        try {
            Invoke-RestMethod -Uri "https://kroki.io/plantuml/png" -Method Post -Body $content -ContentType "text/plain; charset=utf-8" -OutFile $outFilePath
            Write-Host "Exported: $adFileName -> $outFileName" -ForegroundColor Green
            $adProcessed++
        }
        catch {
            Write-Host "Error exporting $adFileName : $_" -ForegroundColor Red
        }
    }
    
    # Process SD
    $sdFileName = "SD{0:D2}.txt" -f $i
    $sdFilePath = Join-Path $base_dir $folder | Join-Path -ChildPath $sdFileName
    if (Test-Path $sdFilePath) {
        $content = Get-Content -Path $sdFilePath -Raw -Encoding UTF8
        $outFileName = "SD{0:D2}.png" -f $i
        $outFilePath = Join-Path $sd_img_dir $outFileName
        try {
            Invoke-RestMethod -Uri "https://kroki.io/plantuml/png" -Method Post -Body $content -ContentType "text/plain; charset=utf-8" -OutFile $outFilePath
            Write-Host "Exported: $sdFileName -> $outFileName" -ForegroundColor Green
            $sdProcessed++
        }
        catch {
            Write-Host "Error exporting $sdFileName : $_" -ForegroundColor Red
        }
    }
}

Write-Host "`nSuccessfully exported $adProcessed ADs and $sdProcessed SDs!" -ForegroundColor Cyan
