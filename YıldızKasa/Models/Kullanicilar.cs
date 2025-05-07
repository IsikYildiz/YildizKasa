using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Kullanicilar
{
    public int KullaniciNo { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Adres1 { get; set; }

    public string? Adres2 { get; set; }

    public DateOnly? SonGirisTarihi { get; set; }

    public uint GirisSayisi { get; set; }

    public decimal Bakiye { get; set; }

    public string Sifre { get; set; } = null!;

    public ulong YetkiSeviyesi { get; set; }

    public virtual ICollection<Kredikartlari> Kredikartlaris { get; set; } = new List<Kredikartlari>();

    public virtual ICollection<Sepet> Sepets { get; set; } = new List<Sepet>();

    public virtual ICollection<Sipari> Siparis { get; set; } = new List<Sipari>();

    public virtual ICollection<Yorum> Yorums { get; set; } = new List<Yorum>();
}
