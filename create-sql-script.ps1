# SQL Script Olusturma Script'i
# Migration'lardan SQL script'i olusturur

Write-Host "SQL Script Olusturuluyor..." -ForegroundColor Green
Write-Host "=========================" -ForegroundColor Green
Write-Host ""

# Proje dosyasi
$projectFile = "RandevuWeb.csproj"
$outputFile = "database-script.sql"

# Migration'lardan SQL script olustur
Write-Host "Migration'lardan SQL script olusturuluyor..." -ForegroundColor Yellow
dotnet ef migrations script --project $projectFile -o $outputFile --idempotent

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "SQL script basariyla olusturuldu: $outputFile" -ForegroundColor Green
    Write-Host ""
    Write-Host "Bu dosyayi SQL Server Management Studio'da calistirabilirsiniz." -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Kullanim:" -ForegroundColor Yellow
    Write-Host "1. SQL Server Management Studio'yu acin" -ForegroundColor White
    Write-Host "2. Production SQL Server'a baglanin" -ForegroundColor White
    Write-Host "3. $outputFile dosyasini acin" -ForegroundColor White
    Write-Host "4. Script'i calistirin (F5)" -ForegroundColor White
} else {
    Write-Host ""
    Write-Host "SQL script olusturulurken hata olustu!" -ForegroundColor Red
    exit 1
}

