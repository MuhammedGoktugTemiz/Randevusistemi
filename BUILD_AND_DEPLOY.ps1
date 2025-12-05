# Production Build ve Deployment Script
# Bu script projeyi build eder ve deployment için hazırlar

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  .NET Framework 4.8 Build & Deploy" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Proje dizini
$projectPath = Get-Location
$solutionPath = Join-Path $projectPath "randevu web.sln"
$projectFile = Join-Path $projectPath "RandevuWeb.csproj"
$deployPath = Join-Path $projectPath "deploy"

Write-Host "[1/5] Proje dosyaları kontrol ediliyor..." -ForegroundColor Yellow

# Solution veya proje dosyası kontrolü
if (-not (Test-Path $solutionPath) -and -not (Test-Path $projectFile)) {
    Write-Host "HATA: Solution veya proje dosyası bulunamadı!" -ForegroundColor Red
    Write-Host "  Aranan: $solutionPath" -ForegroundColor Yellow
    Write-Host "  Aranan: $projectFile" -ForegroundColor Yellow
    exit 1
}

# Hangi dosyayı kullanacağız?
if (Test-Path $solutionPath) {
    $buildTarget = $solutionPath
    Write-Host "✓ Solution dosyası bulundu: $solutionPath" -ForegroundColor Green
} elseif (Test-Path $projectFile) {
    $buildTarget = $projectFile
    Write-Host "✓ Proje dosyası bulundu: $projectFile" -ForegroundColor Green
}
Write-Host ""

# Eski deployment klasörünü temizle
Write-Host "[2/5] Eski deployment klasörü temizleniyor..." -ForegroundColor Yellow
if (Test-Path $deployPath) {
    Remove-Item $deployPath -Recurse -Force
}
New-Item -ItemType Directory -Path $deployPath -Force | Out-Null
Write-Host "✓ Deployment klasörü hazır" -ForegroundColor Green
Write-Host ""

# MSBuild ile build et
Write-Host "[3/5] Proje build ediliyor (Release)..." -ForegroundColor Yellow
$msbuildPath = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if (-not (Test-Path $msbuildPath)) {
    $msbuildPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe"
}
if (-not (Test-Path $msbuildPath)) {
    $msbuildPath = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
}
if (-not (Test-Path $msbuildPath)) {
    Write-Host "HATA: MSBuild bulunamadı. Visual Studio yüklü olmalı." -ForegroundColor Red
    Write-Host "Lütfen Visual Studio'da manuel olarak Build → Rebuild Solution yapın." -ForegroundColor Yellow
    exit 1
}

$buildArgs = @(
    $buildTarget,
    "/p:Configuration=Release",
    "/p:Platform=`"Any CPU`"",
    "/t:Rebuild",
    "/v:minimal",
    "/nologo"
)

$buildResult = & $msbuildPath $buildArgs 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "HATA: Build başarısız oldu!" -ForegroundColor Red
    Write-Host $buildResult
    exit 1
}
Write-Host "✓ Build başarılı" -ForegroundColor Green
Write-Host ""

# Deployment dosyalarını kopyala
Write-Host "[4/5] Deployment dosyaları hazırlanıyor..." -ForegroundColor Yellow

# bin/Release klasöründeki DLL'leri kopyala
$binReleasePath = Join-Path $projectPath "bin\Release"
if (Test-Path $binReleasePath) {
    $deployBinPath = Join-Path $deployPath "bin"
    New-Item -ItemType Directory -Path $deployBinPath -Force | Out-Null
    Copy-Item "$binReleasePath\*" -Destination $deployBinPath -Recurse -Force
    Write-Host "✓ DLL'ler kopyalandı" -ForegroundColor Green
} else {
    Write-Host "UYARI: bin\Release klasörü bulunamadı. Build başarısız olmuş olabilir." -ForegroundColor Yellow
}

# Views klasörünü kopyala
if (Test-Path "Views") {
    Copy-Item "Views" -Destination $deployPath -Recurse -Force
    Write-Host "✓ Views klasörü kopyalandı" -ForegroundColor Green
}

# wwwroot klasörünü kopyala
if (Test-Path "wwwroot") {
    Copy-Item "wwwroot" -Destination $deployPath -Recurse -Force
    Write-Host "✓ wwwroot klasörü kopyalandı" -ForegroundColor Green
}

# App_Start klasörünü kopyala
if (Test-Path "App_Start") {
    Copy-Item "App_Start" -Destination $deployPath -Recurse -Force
    Write-Host "✓ App_Start klasörü kopyalandı" -ForegroundColor Green
}

# web.config kopyala (production için optimize edilmiş)
if (Test-Path "web.config") {
    Copy-Item "web.config" -Destination $deployPath -Force
    Write-Host "✓ web.config kopyalandı" -ForegroundColor Green
}

# Global.asax kopyala
if (Test-Path "Global.asax") {
    Copy-Item "Global.asax" -Destination $deployPath -Force
    Write-Host "✓ Global.asax kopyalandı" -ForegroundColor Green
}

# packages.config kopyala (opsiyonel)
if (Test-Path "packages.config") {
    Copy-Item "packages.config" -Destination $deployPath -Force
    Write-Host "✓ packages.config kopyalandı" -ForegroundColor Green
}

Write-Host ""

# Deployment bilgileri
Write-Host "[5/5] Deployment hazır!" -ForegroundColor Green
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  DEPLOYMENT DOSYALARI HAZIR" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Deployment klasörü: $deployPath" -ForegroundColor Yellow
Write-Host ""
Write-Host "SONRAKI ADIMLAR:" -ForegroundColor Cyan
Write-Host "1. '$deployPath' klasöründeki TÜM dosyaları hosting'e yükleyin" -ForegroundColor White
Write-Host "2. web.config dosyasındaki connection string'i hosting'e göre güncelleyin" -ForegroundColor White
Write-Host "3. IIS Application Pool'u .NET Framework 4.8'e ayarlayın" -ForegroundColor White
Write-Host "4. Veritabanını oluşturun (database-script.sql)" -ForegroundColor White
Write-Host ""
Write-Host "Detaylı rehber: PRODUCTION_DEPLOYMENT_FINAL.md" -ForegroundColor Yellow

