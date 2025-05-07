using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Sepeturun
{
    public int SepetUrunId { get; set; }

    public int SepetId { get; set; }

    public int UrunKod { get; set; }

    public short Adet { get; set; }

    public decimal? ToplamFiyat { get; set; }

    public virtual Sepet Sepet { get; set; } = null!;

    public virtual Urunler UrunKodNavigation { get; set; } = null!;
}
