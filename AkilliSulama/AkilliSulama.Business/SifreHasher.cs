using System.Security.Cryptography;

namespace AkilliSulama.Business;

// Parolaları PBKDF2 (SHA-256) ile tuzlayıp hash'ler.
// Saklanan biçim: "tekrarSayisi.tuz.hash"  (tuz ve hash Base64)
public static class SifreHasher
{
    private const int TuzBoyutu = 16;      // 128 bit rastgele tuz
    private const int HashBoyutu = 32;     // 256 bit hash
    private const int TekrarSayisi = 100_000; // Yavaşlatma: kaba kuvvet saldırısını zorlaştırır

    public static string Hashle(string sifre)
    {
        byte[] tuz = RandomNumberGenerator.GetBytes(TuzBoyutu); // her parola için farklı tuz
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, TekrarSayisi, HashAlgorithmName.SHA256, HashBoyutu);
        return $"{TekrarSayisi}.{Convert.ToBase64String(tuz)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Dogrula(string sifre, string saklananHash)
    {
        var parcalar = saklananHash.Split('.');
        if (parcalar.Length != 3) return false;

        int tekrar = int.Parse(parcalar[0]);
        byte[] tuz = Convert.FromBase64String(parcalar[1]);
        byte[] beklenen = Convert.FromBase64String(parcalar[2]);

        // Girilen parolayı AYNI tuzla tekrar hash'le ve karşılaştır
        byte[] hesaplanan = Rfc2898DeriveBytes.Pbkdf2(sifre, tuz, tekrar, HashAlgorithmName.SHA256, beklenen.Length);

        // Sabit süreli karşılaştırma: zamanlama saldırısını önler
        return CryptographicOperations.FixedTimeEquals(hesaplanan, beklenen);
    }
}