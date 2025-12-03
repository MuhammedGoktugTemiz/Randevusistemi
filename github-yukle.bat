@echo off
chcp 65001 >nul
echo ========================================
echo GitHub'a Yükleme Scripti
echo ========================================
echo.

REM Git yüklü mü kontrol et
where git >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [HATA] Git yüklü değil!
    echo.
    echo Lütfen önce Git'i yükleyin:
    echo https://git-scm.com/download/win
    echo.
    pause
    exit /b 1
)

echo [OK] Git yüklü
echo.

REM Git repository başlat
if not exist ".git" (
    echo [1/8] Git repository başlatılıyor...
    git init
    if %ERRORLEVEL% NEQ 0 (
        echo [HATA] Git repository başlatılamadı!
        pause
        exit /b 1
    )
    echo [OK] Git repository başlatıldı
) else (
    echo [OK] Git repository zaten başlatılmış
)
echo.

REM Hassas dosyaları Git'ten kaldır (eğer varsa)
echo [2/8] Hassas dosyalar kontrol ediliyor...
git rm --cached appsettings.json >nul 2>&1
git rm -r --cached Data/ >nul 2>&1
git rm -r --cached bin/ >nul 2>&1
git rm -r --cached obj/ >nul 2>&1
echo [OK] Hassas dosyalar kontrol edildi
echo.

REM Dosyaları staging area'ya ekle
echo [3/8] Dosyalar staging area'ya ekleniyor...
git add .
if %ERRORLEVEL% NEQ 0 (
    echo [HATA] Dosyalar eklenemedi!
    pause
    exit /b 1
)
echo [OK] Dosyalar eklendi
echo.

REM Hassas dosya kontrolü
echo [4/8] Hassas dosyalar kontrol ediliyor...
git diff --cached --name-only | findstr /C:"appsettings.json" >nul
if %ERRORLEVEL% EQU 0 (
    echo [UYARI] appsettings.json staged area'da! Git'ten kaldırılıyor...
    git rm --cached appsettings.json
)

git diff --cached --name-only | findstr /C:"Data" >nul
if %ERRORLEVEL% EQU 0 (
    echo [UYARI] Data/ klasörü staged area'da! Git'ten kaldırılıyor...
    git rm -r --cached Data/
)
echo [OK] Hassas dosya kontrolü tamamlandı
echo.

REM İlk commit
echo [5/8] İlk commit yapılıyor...
git commit -m "İlk commit: Randevu Web Sistemi - Güvenlik özellikleri eklendi"
if %ERRORLEVEL% NEQ 0 (
    echo [UYARI] Commit yapılamadı veya zaten commit edilmiş
) else (
    echo [OK] Commit başarılı
)
echo.

REM GitHub repository bilgisi
echo [6/8] GitHub Repository Bilgisi
echo.
echo Lütfen GitHub'da repository oluşturun:
echo 1. https://github.com adresine gidin
echo 2. Sağ üstteki + butonuna tıklayın
echo 3. "New repository" seçin
echo 4. Repository adını girin
echo 5. PRIVATE seçin (önerilen)
echo 6. "Initialize with README" seçeneğini İŞARETLEMEYİN
echo 7. "Create repository" tıklayın
echo.
set /p GITHUB_USER="GitHub kullanıcı adınızı girin: "
set /p REPO_NAME="Repository adını girin: "
echo.

REM Remote ekle
echo [7/8] GitHub remote ekleniyor...
git remote remove origin >nul 2>&1
git remote add origin https://github.com/%GITHUB_USER%/%REPO_NAME%.git
if %ERRORLEVEL% NEQ 0 (
    echo [HATA] Remote eklenemedi!
    pause
    exit /b 1
)
echo [OK] Remote eklendi: https://github.com/%GITHUB_USER%/%REPO_NAME%.git
echo.

REM Branch'i main olarak ayarla
echo [8/8] Branch ayarlanıyor...
git branch -M main
echo [OK] Branch main olarak ayarlandı
echo.

echo ========================================
echo Hazırlık Tamamlandı!
echo ========================================
echo.
echo Şimdi GitHub'a push etmek için şu komutu çalıştırın:
echo   git push -u origin main
echo.
echo VEYA bu scripti tekrar çalıştırın ve "E" seçeneğini seçin.
echo.
set /p PUSH_NOW="Şimdi push etmek istiyor musunuz? (E/H): "
if /i "%PUSH_NOW%"=="E" (
    echo.
    echo GitHub'a push ediliyor...
    git push -u origin main
    if %ERRORLEVEL% EQU 0 (
        echo.
        echo [BAŞARILI] Proje GitHub'a yüklendi!
    ) else (
        echo.
        echo [HATA] Push başarısız oldu. Lütfen manuel olarak deneyin:
        echo   git push -u origin main
    )
) else (
    echo.
    echo Push işlemi atlandı. Daha sonra şu komutu çalıştırabilirsiniz:
    echo   git push -u origin main
)

echo.
pause

