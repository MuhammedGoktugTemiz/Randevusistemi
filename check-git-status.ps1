# Git durumunu kontrol etme scripti
# Bu script, hassas dosyaların Git'e eklenip eklenmediğini kontrol eder

Write-Host "=== Git Durum Kontrolü ===" -ForegroundColor Cyan
Write-Host ""

# Git yüklü mü kontrol et
try {
    $gitVersion = git --version
    Write-Host "✓ Git yüklü: $gitVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ Git yüklü değil! Lütfen Git'i yükleyin: https://git-scm.com/download/win" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Git repository var mı kontrol et
if (Test-Path ".git") {
    Write-Host "✓ Git repository bulundu" -ForegroundColor Green
} else {
    Write-Host "⚠ Git repository bulunamadı. 'git init' komutunu çalıştırın." -ForegroundColor Yellow
    Write-Host ""
    exit 0
}

Write-Host ""

# Staged dosyaları kontrol et
Write-Host "Staged dosyalar kontrol ediliyor..." -ForegroundColor Cyan
$stagedFiles = git diff --cached --name-only

if ($stagedFiles) {
    Write-Host ""
    Write-Host "Staged dosyalar:" -ForegroundColor Yellow
    $stagedFiles | ForEach-Object { Write-Host "  - $_" }
} else {
    Write-Host "✓ Staged dosya yok" -ForegroundColor Green
}

Write-Host ""

# Hassas dosyaları kontrol et
Write-Host "Hassas dosyalar kontrol ediliyor..." -ForegroundColor Cyan
$sensitiveFiles = @("appsettings.json", "Data/doctors.json", "Data/patients.json", "Data/appointments.json")
$foundSensitive = $false

foreach ($file in $sensitiveFiles) {
    if ($stagedFiles -contains $file) {
        Write-Host "✗ UYARI: $file Git'e eklenmiş!" -ForegroundColor Red
        $foundSensitive = $true
    }
}

# Git tracking'de hassas dosyalar var mı kontrol et
Write-Host ""
Write-Host "Git tracking'deki hassas dosyalar kontrol ediliyor..." -ForegroundColor Cyan
$trackedFiles = git ls-files

foreach ($file in $sensitiveFiles) {
    if ($trackedFiles -contains $file) {
        Write-Host "✗ UYARI: $file Git tracking'de!" -ForegroundColor Red
        Write-Host "  Çözüm: git rm --cached $file" -ForegroundColor Yellow
        $foundSensitive = $true
    }
}

Write-Host ""

if (-not $foundSensitive) {
    Write-Host "✓ Tüm hassas dosyalar güvende!" -ForegroundColor Green
} else {
    Write-Host "⚠ HASSAS DOSYALAR BULUNDU! Lütfen bunları Git'ten kaldırın!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Komutlar:" -ForegroundColor Yellow
    Write-Host "  git rm --cached appsettings.json" -ForegroundColor White
    Write-Host "  git rm -r --cached Data/" -ForegroundColor White
    Write-Host "  git commit -m 'Hassas dosyaları kaldır'" -ForegroundColor White
}

Write-Host ""
Write-Host "=== Kontrol Tamamlandı ===" -ForegroundColor Cyan

