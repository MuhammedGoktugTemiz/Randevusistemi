# GitHub'a yükleme hazırlık scripti
# Bu script, projeyi GitHub'a yüklemek için hazırlar

Write-Host "=== GitHub Hazırlık Scripti ===" -ForegroundColor Cyan
Write-Host ""

# Git yüklü mü kontrol et
try {
    git --version | Out-Null
    Write-Host "✓ Git yüklü" -ForegroundColor Green
} catch {
    Write-Host "✗ Git yüklü değil! Lütfen Git'i yükleyin: https://git-scm.com/download/win" -ForegroundColor Red
    exit 1
}

Write-Host ""

# .gitignore kontrolü
Write-Host ".gitignore kontrol ediliyor..." -ForegroundColor Cyan
$gitignoreContent = Get-Content ".gitignore" -Raw -ErrorAction SilentlyContinue

if ($gitignoreContent) {
    $requiredIgnores = @("appsettings.json", "Data/", "bin/", "obj/")
    $missingIgnores = @()
    
    foreach ($ignore in $requiredIgnores) {
        if ($gitignoreContent -notmatch [regex]::Escape($ignore)) {
            $missingIgnores += $ignore
        }
    }
    
    if ($missingIgnores.Count -eq 0) {
        Write-Host "✓ .gitignore doğru yapılandırılmış" -ForegroundColor Green
    } else {
        Write-Host "⚠ .gitignore'da eksikler var: $($missingIgnores -join ', ')" -ForegroundColor Yellow
    }
} else {
    Write-Host "✗ .gitignore dosyası bulunamadı!" -ForegroundColor Red
    exit 1
}

Write-Host ""

# Git repository başlat
if (-not (Test-Path ".git")) {
    Write-Host "Git repository başlatılıyor..." -ForegroundColor Cyan
    git init
    Write-Host "✓ Git repository başlatıldı" -ForegroundColor Green
} else {
    Write-Host "✓ Git repository zaten başlatılmış" -ForegroundColor Green
}

Write-Host ""

# Hassas dosyaları Git'ten kaldır
Write-Host "Hassas dosyalar kontrol ediliyor..." -ForegroundColor Cyan
$sensitiveFiles = @("appsettings.json", "Data")

foreach ($file in $sensitiveFiles) {
    $isTracked = git ls-files $file -ErrorAction SilentlyContinue
    
    if ($isTracked) {
        Write-Host "⚠ $file Git tracking'de, kaldırılıyor..." -ForegroundColor Yellow
        if (Test-Path $file) {
            git rm --cached $file -r -f 2>$null
            Write-Host "✓ $file Git'ten kaldırıldı (dosya korundu)" -ForegroundColor Green
        }
    } else {
        Write-Host "✓ $file Git tracking'de değil" -ForegroundColor Green
    }
}

Write-Host ""

# Dosyaları staging area'ya ekle
Write-Host "Dosyalar staging area'ya ekleniyor..." -ForegroundColor Cyan
git add .

Write-Host "✓ Dosyalar eklendi" -ForegroundColor Green

Write-Host ""

# Staged dosyaları göster
Write-Host "Staged dosyalar:" -ForegroundColor Cyan
$stagedFiles = git diff --cached --name-only
if ($stagedFiles) {
    $stagedFiles | ForEach-Object { Write-Host "  + $_" -ForegroundColor Green }
} else {
    Write-Host "  (staged dosya yok)" -ForegroundColor Yellow
}

Write-Host ""

# Hassas dosya kontrolü
$sensitiveInStaged = $stagedFiles | Where-Object { $_ -like "appsettings.json" -or $_ -like "Data/*" }
if ($sensitiveInStaged) {
    Write-Host "✗ UYARI: Hassas dosyalar staged area'da!" -ForegroundColor Red
    $sensitiveInStaged | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
    Write-Host ""
    Write-Host "Lütfen bu dosyaları Git'ten kaldırın:" -ForegroundColor Yellow
    Write-Host "  git rm --cached appsettings.json" -ForegroundColor White
    Write-Host "  git rm -r --cached Data/" -ForegroundColor White
    exit 1
} else {
    Write-Host "✓ Hassas dosyalar staged area'da değil" -ForegroundColor Green
}

Write-Host ""
Write-Host "=== Hazırlık Tamamlandı! ===" -ForegroundColor Green
Write-Host ""
Write-Host "Şimdi şu komutları çalıştırabilirsiniz:" -ForegroundColor Cyan
Write-Host "  git commit -m 'İlk commit: Randevu Web Sistemi'" -ForegroundColor White
Write-Host "  git remote add origin https://github.com/KULLANICI_ADI/REPO_ADI.git" -ForegroundColor White
Write-Host "  git branch -M main" -ForegroundColor White
Write-Host "  git push -u origin main" -ForegroundColor White
Write-Host ""
Write-Host "Detaylı bilgi için GITHUB_YUKLEME_REHBERI.md dosyasına bakın." -ForegroundColor Yellow

