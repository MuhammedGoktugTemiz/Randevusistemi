# Deployment Kontrol Script'i
# Bu script deployment dosyalarının eksiksiz olduğunu kontrol eder

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Deployment Kontrol" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$deployPath = Join-Path (Get-Location) "deploy"
$errors = @()
$warnings = @()

# Deployment klasörü var mı?
if (-not (Test-Path $deployPath)) {
    Write-Host "HATA: Deployment klasörü bulunamadı!" -ForegroundColor Red
    Write-Host "Önce BUILD_AND_DEPLOY.ps1 script'ini çalıştırın." -ForegroundColor Yellow
    exit 1
}

Write-Host "Kontrol ediliyor: $deployPath" -ForegroundColor Yellow
Write-Host ""

# bin klasörü kontrolü
$binPath = Join-Path $deployPath "bin"
if (-not (Test-Path $binPath)) {
    $errors += "bin/ klasörü eksik!"
} else {
    $requiredDlls = @(
        "RandevuWeb.dll",
        "EntityFramework.dll",
        "EntityFramework.SqlServer.dll",
        "BCrypt.Net-Next.dll",
        "System.Web.Mvc.dll",
        "Unity.Container.dll",
        "Unity.Mvc5.dll",
        "Newtonsoft.Json.dll"
    )
    
    foreach ($dll in $requiredDlls) {
        $dllPath = Join-Path $binPath $dll
        if (-not (Test-Path $dllPath)) {
            $warnings += "DLL eksik: $dll"
        }
    }
    Write-Host "✓ bin/ klasörü mevcut" -ForegroundColor Green
}

# Views klasörü kontrolü
$viewsPath = Join-Path $deployPath "Views"
if (-not (Test-Path $viewsPath)) {
    $errors += "Views/ klasörü eksik!"
} else {
    Write-Host "✓ Views/ klasörü mevcut" -ForegroundColor Green
}

# wwwroot klasörü kontrolü
$wwwrootPath = Join-Path $deployPath "wwwroot"
if (-not (Test-Path $wwwrootPath)) {
    $errors += "wwwroot/ klasörü eksik!"
} else {
    Write-Host "✓ wwwroot/ klasörü mevcut" -ForegroundColor Green
}

# App_Start klasörü kontrolü
$appStartPath = Join-Path $deployPath "App_Start"
if (-not (Test-Path $appStartPath)) {
    $warnings += "App_Start/ klasörü eksik (opsiyonel ama önerilir)"
} else {
    Write-Host "✓ App_Start/ klasörü mevcut" -ForegroundColor Green
}

# web.config kontrolü
$webConfigPath = Join-Path $deployPath "web.config"
if (-not (Test-Path $webConfigPath)) {
    $errors += "web.config dosyası eksik!"
} else {
    Write-Host "✓ web.config mevcut" -ForegroundColor Green
    
    # Connection string kontrolü
    $webConfigContent = Get-Content $webConfigPath -Raw
    if ($webConfigContent -match "Server=localhost") {
        $warnings += "web.config'de connection string localhost kullanıyor. Hosting'e göre güncelleyin!"
    }
}

# Global.asax kontrolü
$globalAsaxPath = Join-Path $deployPath "Global.asax"
if (-not (Test-Path $globalAsaxPath)) {
    $errors += "Global.asax dosyası eksik!"
} else {
    Write-Host "✓ Global.asax mevcut" -ForegroundColor Green
}

Write-Host ""

# Sonuçlar
if ($errors.Count -eq 0 -and $warnings.Count -eq 0) {
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "  ✓ TÜM KONTROLLER BAŞARILI!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Deployment dosyaları hazır. Hosting'e yükleyebilirsiniz." -ForegroundColor Yellow
} else {
    if ($errors.Count -gt 0) {
        Write-Host "========================================" -ForegroundColor Red
        Write-Host "  HATALAR:" -ForegroundColor Red
        Write-Host "========================================" -ForegroundColor Red
        foreach ($error in $errors) {
            Write-Host "  ✗ $error" -ForegroundColor Red
        }
        Write-Host ""
    }
    
    if ($warnings.Count -gt 0) {
        Write-Host "========================================" -ForegroundColor Yellow
        Write-Host "  UYARILAR:" -ForegroundColor Yellow
        Write-Host "========================================" -ForegroundColor Yellow
        foreach ($warning in $warnings) {
            Write-Host "  ⚠ $warning" -ForegroundColor Yellow
        }
        Write-Host ""
    }
}

Write-Host ""
Write-Host "Deployment klasörü: $deployPath" -ForegroundColor Cyan

