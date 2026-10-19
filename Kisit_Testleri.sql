USE AkilliSulamaDB;

-- 2) YABANCI ANAHTAR testi: Olmayan bir kullanıcıya parsel eklenemez
INSERT INTO Parseller (KullaniciId, Ad, Konum, Alan, ToprakTuru)
VALUES (999, 'Sahipsiz Parsel', 'Test', 10, 'Kumlu');