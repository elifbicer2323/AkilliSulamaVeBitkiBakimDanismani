# Güvenlik Notu – Hafta 5

- **Parola saklama:** Parolalar düz metin saklanmaz. PBKDF2-SHA256, 16 bayt rastgele tuz (salt) ve 100.000 tekrar ile hash'lenir.
- **Oturum:** Cookie tabanlı oturum. HttpOnly, Secure, SameSite=Strict; 30 dk işlem yoksa oturum biter.
- **Rol tabanlı erişim:** Çiftçi ve Yönetici rolleri. /Yonetim sayfası sunucu tarafında [Authorize(Roles="Yonetici")] ile korunur; yetkisiz kullanıcı "Erişim engellendi" sayfasına yönlendirilir.
- **Kaba kuvvet koruması:** 5 hatalı girişten sonra hesap 15 dakika kilitlenir.
- **Kullanıcı numaralandırma koruması:** E-posta yanlış da olsa parola yanlış da olsa aynı hata mesajı gösterilir.
- **CSRF koruması:** Tüm formlarda [ValidateAntiForgeryToken].
- **Açık yönlendirme koruması:** Girişten sonra sadece site içi adreslere dönülür (Url.IsLocalUrl).