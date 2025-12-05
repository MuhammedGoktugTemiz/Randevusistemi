# Git commit script for .NET Framework 4.8 port
$ErrorActionPreference = "Stop"

# Proje dizinine geç - encoding sorununu çözmek için
$projectPath = Join-Path $env:USERPROFILE "Desktop\randevu web"
Set-Location $projectPath
Write-Host "Proje dizini: $(Get-Location)"

# Git lock dosyasını temizle
if (Test-Path ".git\index.lock") {
    Remove-Item ".git\index.lock" -Force -ErrorAction SilentlyContinue
}

# Eğer .git yoksa başlat
if (-not (Test-Path ".git")) {
    Write-Host "Git repository başlatılıyor..."
    git init
}

# .NET Framework 4.8 dosyalarını ekle
Write-Host "Dosyalar ekleniyor..."
git add App_Start/
git add Controllers/*.NetFramework.cs
git add Data/*.NetFramework.cs
git add Models/*.NetFramework.cs
git add Services/*.NetFramework.cs
git add Global.asax
git add Global.asax.cs
git add packages.config
git add RandevuWeb.NetFramework.csproj
git add web.config.NetFramework
git add Views/web.config
git add Properties/AssemblyInfo.cs
git add *.md
git add .gitignore

# Views klasörünü ekle (eğer varsa)
if (Test-Path "Views") {
    git add Views/
}

# wwwroot klasörünü ekle (eğer varsa)
if (Test-Path "wwwroot") {
    git add wwwroot/
}

# Commit yap
Write-Host "Commit yapılıyor..."
git commit -m "Port to .NET Framework 4.8 - Complete migration from ASP.NET Core 8.0"

Write-Host "Commit tamamlandı!"
Write-Host ""
Write-Host "GitHub'a push yapmak için:"
Write-Host "git remote add origin <your-repo-url>"
Write-Host "git push -u origin main"
