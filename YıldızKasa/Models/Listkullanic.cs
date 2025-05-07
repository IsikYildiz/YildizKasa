using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Listkullanic
{
    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public int? Email { get; set; }

    public string? Adres1 { get; set; }

    public string? Adres2 { get; set; }

    public decimal Bakiye { get; set; }

    public DateOnly? SonGirisTarihi { get; set; }

    public uint GirisSayisi { get; set; }
}
