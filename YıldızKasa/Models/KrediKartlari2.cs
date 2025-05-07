using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Kredikartlari2
{
    public string KartNo { get; set; } = null!;

    public string SonGecerlilikTarihi { get; set; }

    public string KartSahipIsmi { get; set; } = null!;
}