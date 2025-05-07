using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Yorum
{
    public int YorumId { get; set; }

    public int KullaniciNo { get; set; }

    public int UrunKod { get; set; }

    public byte Puan { get; set; }

    public string? Yorum1 { get; set; }

    public string KullaniciAd { get; set; } = null!;

    public virtual Kullanicilar KullaniciNoNavigation { get; set; } = null!;

    public virtual Urunler UrunKodNavigation { get; set; } = null!;
}
