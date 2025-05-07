using System;
using System.Collections.Generic;

namespace YıldızKasa.Models;

public partial class Sipari
{
    public int SiparisId { get; set; }  

    public int KullaniciNo { get; set; }

    public decimal SonFiyat { get; set; }

    public DateOnly SiparisTarihi { get; set; }

    public string SiparisDurumu { get; set; } = null!;

    public string GonderilenAdres { get; set; } = null!;

    public virtual Kullanicilar KullaniciNoNavigation { get; set; } = null!;
}
