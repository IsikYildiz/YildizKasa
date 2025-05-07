using YıldızKasa.Models;

public class SiparislerModel
{
    public virtual Kullanicilar Kullanicilar { get; set; }
    public virtual SepetTam SepetTam { get; set; }
    public virtual List<Kredikartlari> Kredikartlari { get; set; }
}