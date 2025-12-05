# Git Commit ve Push Script'i
# Kullanım: .\git-commit.ps1 "commit mesajı"

param(
    [Parameter(Mandatory=$false)]
    [string]$Message = "SQL Server migration ve production deployment hazirliklari"
)

Write-Host "Git Commit ve Push Islemleri" -ForegroundColor Green
Write-Host "============================" -ForegroundColor Green
Write-Host ""

# Proje dizinine git
$projectPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $projectPath

# Git durumunu kontrol et
Write-Host "Git durumu kontrol ediliyor..." -ForegroundColor Yellow
$status = git status --short 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "Git repository bulunamadi. Git init yapiliyor..." -ForegroundColor Yellow
    git init
}

# Tüm değişiklikleri ekle (ignore edilenler hariç)
Write-Host "Degisiklikler ekleniyor..." -ForegroundColor Yellow
git add .

# Commit
Write-Host "Commit yapiliyor: $Message" -ForegroundColor Yellow
git commit -m $Message

if ($LASTEXITCODE -eq 0) {
    Write-Host "Commit basarili!" -ForegroundColor Green
    
    # Remote kontrolü
    $remote = git remote -v 2>&1
    if ($remote -match "origin") {
        Write-Host ""
        Write-Host "GitHub'a push yapmak icin:" -ForegroundColor Cyan
        Write-Host "  git push origin main" -ForegroundColor White
        Write-Host "  veya" -ForegroundColor White
        Write-Host "  git push origin master" -ForegroundColor White
    } else {
        Write-Host ""
        Write-Host "GitHub remote eklemek icin:" -ForegroundColor Cyan
        Write-Host "  git remote add origin https://github.com/KULLANICI_ADI/REPO_ADI.git" -ForegroundColor White
        Write-Host "  git branch -M main" -ForegroundColor White
        Write-Host "  git push -u origin main" -ForegroundColor White
    }
} else {
    Write-Host "Commit basarisiz!" -ForegroundColor Red
    exit 1
}

