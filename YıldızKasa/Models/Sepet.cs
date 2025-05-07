using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Sepet
{
    public int SepetId { get; set; }

    public int KullaniciNo { get; set; }

    public decimal? SepetFİyati { get; set; }

    public int? UrunSayisi { get; set; }

    public virtual Kullanicilar KullaniciNoNavigation { get; set; } = null!;

    public virtual ICollection<Sepeturun> Sepeturuns { get; set; } = new List<Sepeturun>();
}
