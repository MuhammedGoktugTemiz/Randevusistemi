# Git commit script for .NET Framework 4.8 port
$ErrorActionPreference = "Stop"

# Proje dizinine geç
$projectPath = Join-Path $env:USERPROFILE "Desktop\randevu web"
Set-Location $projectPath

Write-Host "Proje dizini: $projectPath"

# Git repository başlat (eğer yoksa)
if (-not (Test-Path ".git")) {
    Write-Host "Git repository başlatılıyor..."
    git init
}

# Sadece proje dosyalarını ekle
Write-Host "Dosyalar ekleniyor..."
git add App_Start/
git add Controllers/*.cs
git add Data/*.cs
git add Models/*.cs
git add Services/*.cs
git add Global.asax
git add Global.asax.cs
git add packages.config
git add RandevuWeb.csproj
git add web.config
git add Views/web.config
git add Properties/AssemblyInfo.cs
git add Views/
git add wwwroot/
git add *.md
git add .gitignore

# Commit yap
Write-Host "Commit yapılıyor..."
git commit -m "Port to .NET Framework 4.8 - Complete migration from ASP.NET Core 8.0"

Write-Host ""
Write-Host "✅ Commit tamamlandı!"
Write-Host ""
Write-Host "GitHub'a push yapmak için:"
Write-Host "  git remote add origin <your-repo-url>"
Write-Host "  git push -u origin main"

