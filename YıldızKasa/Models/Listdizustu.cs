using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Listdizustu
{
    public int UrunKod { get; set; }

    public int MarkaId { get; set; }

    public string UrunAd { get; set; } = null!;

    public decimal VergisizFiyat { get; set; }

    public int Kdvoran { get; set; }

    public decimal ListeFiyat { get; set; }

    public int Indirim { get; set; }

    public string UrunResim { get; set; } = null!;

    public string? UrunTanitim { get; set; }

    public int StokDurum { get; set; }

    public int Garanti { get; set; }

    public string EkranKarti { get; set; } = null!;

    public string Islemci { get; set; } = null!;

    public string Ram { get; set; } = null!;

    public string Depolama1 { get; set; } = null!;

    public string Depolama2 { get; set; } = null!;

    public string Depolama3 { get; set; } = null!;

    public string Depolama4 { get; set; } = null!;

    public string IsletimSistemi { get; set; } = null!;

    public string? EkranBoyutu { get; set; }

    public string? Cozunurluk { get; set; }

    public ushort? EkranYenilemeHizi { get; set; }

    public string? KasaTipi { get; set; }

    public int SatisAdeti { get; set; }
}
