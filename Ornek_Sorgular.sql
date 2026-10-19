USE AkilliSulamaDB;

SELECT k.Ad AS Kullanici, p.Ad AS Parsel, b.Ad AS Bitki,
       s.KacGundeBir, s.SuMiktari, s.SulamaSaati
FROM Kullanicilar k
JOIN Parseller p      ON p.KullaniciId = k.KullaniciId
JOIN Bitkiler b       ON b.ParselId = p.ParselId
JOIN SulamaPlanlari s ON s.BitkiId = b.BitkiId;