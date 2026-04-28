$base_dir = "d:\KTPM2023\2025_2026_HK2\SE356_KienTrucPhanMem\DoAn\Project\TechSale_BE_and_Diagrams\Diagrams"
$count = 0

Write-Host "Đang ghi đè file SD chính thức và dọn dẹp các file -re.txt..." -ForegroundColor Cyan

for ($i=1; $i -le 61; $i++) {
    $folder = "UC{0:D2}" -f $i
    $reFile = "SD{0:D2}-re.txt" -f $i
    $mainFile = "SD{0:D2}.txt" -f $i
    
    $rePath = Join-Path $base_dir $folder | Join-Path -ChildPath $reFile
    $mainPath = Join-Path $base_dir $folder | Join-Path -ChildPath $mainFile
    
    if (Test-Path $rePath) {
        # Copy content to overwrite main file
        Copy-Item -Path $rePath -Destination $mainPath -Force
        
        # Delete -re file
        Remove-Item -Path $rePath -Force
        
        $count++
    }
}

Write-Host "Thành công! Đã áp dụng và xóa $count file SDxx-re.txt" -ForegroundColor Green
