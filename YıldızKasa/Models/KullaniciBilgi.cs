using YıldızKasa.Models;

public class KullaniciBilgi
{
    public virtual Kullanicilar Kullanicilars { get; set; }
    public virtual List< Kredikartlari> Kredikartlaris { get; set; }
}
