using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Kredikartlari
{
    public int KartId { get; set; }

    public int KullaniciNo { get; set; }

    public string KartNo { get; set; } = null!;

    public DateOnly SonGecerlilikTarihi { get; set; }

    public string KartSahipIsmi { get; set; } = null!;

    public virtual Kullanicilar KullaniciNoNavigation { get; set; } = null!;
}
