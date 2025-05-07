using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Marka
{
    public int MarkaId { get; set; }

    public string MarkaAd { get; set; } = null!;

    public string MarkaWeb { get; set; } = null!;

    public virtual ICollection<Urunler> Urunlers { get; set; } = new List<Urunler>();
}
