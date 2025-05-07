using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using YıldızKasa;
using YıldızKasa.Models;
using KullaniciYorum;   



namespace DatabaseOperations;
public class Data{

    private YildizkasaContext context=new();

    //Email zaten var mı
    public bool ControlEmail(string email){
        return context.Kullanicilars.Any(Kullanicilar => Kullanicilar.Email == email);
    }

    //Şifre zaten var mı
    public bool ControlSifre(string sifre){
        return context.Kullanicilars.Any(Kullanicilar=>Kullanicilar.Sifre==sifre);
    }

    //Ana sayfa da göstermek için 3 view modeli döndürür.
    public HomePageViewModel GetHomePageViewModel(){
        var model = new HomePageViewModel
            {
                Listindirimliurunlers = context.Listindirimliurunlers.ToList(),
                Listmasaustus = context.Listmasaustus.ToList(),
                Listdizustus = context.Listdizustus.ToList()
            };
        return model;
    }

    //İstenilen ürünün modelini ürün adına göre döndürürü.
    public Urunler GetUrunModel(string product){
        var result=context.Urunlers.SingleOrDefault(c=>c.UrunAd==product);
        return result;
    }

    //Verilen ürün koduna göre o ürün için yapılan yorumları döndürür.
    public List<Yorum> GetYorumlar(int product){
        var result=context.Yorums.Where(a=>a.UrunKod==product).ToList();
        return result;
    }

    //Aktif kullanıcıyı email ile bulur ve döndürür.
    public KullaniciBilgi getKullanici(string email){
        var kullanici=context.Kullanicilars.SingleOrDefault(c=>c.Email==email);
        var model = new KullaniciBilgi
            {
                Kullanicilars = kullanici,
                Kredikartlaris = context.Kredikartlaris.Where(c=>c.KullaniciNo==kullanici.KullaniciNo).ToList()
            };
        return model;
    }

    //Veritabanına kullanıcı ekler.
    public void addUser(string ad, string soyad, string email, string sifre){
        context.Database.ExecuteSqlRaw("CALL addKullanici(@_ad, @_soyad, @_email, @_sifre)",
        new MySqlParameter("@_ad", ad),
        new MySqlParameter("@_soyad", soyad),
        new MySqlParameter("@_email", email),
        new MySqlParameter("@_sifre", sifre)
        );
    }
    
    //Kullanıcı bilgilerini günceller.
    public void updUser(string ad, string soyad, string email, string sifre,string adress1, string adress2,int No){

        var entity = context.Kullanicilars.FirstOrDefault(a => a.KullaniciNo == No);

        if(entity != null){
            entity.Ad=ad;
            entity.Soyad=soyad;
            entity.Email=email;
            entity.Sifre=sifre;
            entity.Adres1=adress1;
            entity.Adres2=adress2;
            context.SaveChanges();
        }
    }

    //Kullanıcıyı siler.
    public void delUser(int No){
        Kullanicilar entity=new Kullanicilar(){KullaniciNo=No};
        if(entity != null){
            context.Kullanicilars.Attach(entity);
            context.Kullanicilars.Remove(entity);
            context.SaveChanges();
        }
    }

    //Kullanıcı giriş yaptığında giriş sayısı ve son giriş tarihini günceller.
    public void updUserSession(string email){
        var entity = context.Kullanicilars.FirstOrDefault(a => a.Email == email);

        if(entity != null){
            entity.GirisSayisi+=1;
            entity.SonGirisTarihi=DateOnly.FromDateTime(DateTime.Now);
            context.SaveChanges();
        }
    }

    //Arama sonuçlarına uyan ürünler gösterilir.
    public List<Urunler> findUrun(string productName){
        List<Urunler> products;
        if(productName==null){
            products=context.Urunlers.ToList();
        }
        else{
            products=context.Urunlers.Where(a=>a.UrunAd.ToLower().Contains(productName.ToLower())).ToList();
        }
        return products;
    }

    //Verilen bilgilerle bir kredi kartı oluşturur.
    public void addCard(string email,string cardNo,DateOnly cardDate, string cardName){
        context.Database.ExecuteSqlRaw("CALL addKrediKarti(@_email, @_kartNo, @_sonGecerlilikTarihi, @_kartSahipIsmi)",
        new MySqlParameter("@_email", email),
        new MySqlParameter("@_kartNo", cardNo),
        new MySqlParameter("@_sonGecerlilikTarihi", cardDate),
        new MySqlParameter("@_kartSahipIsmi", cardName)
        );
    }

    //Kredi kartı verilen id ye göre silinir.
    public void delCard(int cardId){
        Kredikartlari entity=new Kredikartlari(){KartId=cardId};
        if(entity != null){
            context.Kredikartlaris.Attach(entity);
            context.Kredikartlaris.Remove(entity);
            context.SaveChanges();
        }
    }

    //Yorum var mı
    public string thereYorum(int urunKod,string email){
        if(email==""){
            return "noGiriş";
        }
        else{
            var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
            if(context.Yorums.Any(a=>a.UrunKod==urunKod && a.KullaniciNo==entity.KullaniciNo)){
            return "var";
            } 
            else{
                return "yok";
            }
        }
    }

    //Bakılan ürüne yorum eklenir.
    public void addYorum(int urunKod,string yorum,byte puan,string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var newYorum=new Yorum{KullaniciNo=entity.KullaniciNo,UrunKod=urunKod,Puan=puan,Yorum1=yorum,KullaniciAd=entity.Ad};
        context.Yorums.Add(newYorum);
        context.SaveChanges();
    }

    //Yorum gelen ürün kodu ve emaile göre silinir.
    public void delYorum(int urunKod,string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var yorumEntity=context.Yorums.FirstOrDefault(a=>a.UrunKod==urunKod && a.KullaniciNo==entity.KullaniciNo);
        if(yorumEntity!= null){
            context.Yorums.Attach(yorumEntity);
            context.Yorums.Remove(yorumEntity);
            context.SaveChanges();
        }
    }

    //Kullanıcının sepetini ve ürünleri geri döndürür.
    public SepetTam getSepet(string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=> a.KullaniciNo == entity.KullaniciNo);
        var sepeturuns=context.Sepeturuns.Where(a=>a.SepetId==sepet.SepetId).ToList();
        List<Sepeturun2> sepeturunTam= sepeturuns.Select(sm=> new Sepeturun2{
            SepetUrunId=sm.SepetUrunId,
            SepetId=sm.SepetId,
            UrunKod=sm.UrunKod,
            Adet=sm.Adet,
            ToplamFiyat=sm.ToplamFiyat,
            urunAd=context.Urunlers.Where(a=>a.UrunKod==sm.UrunKod).Select(a=>a.UrunAd).FirstOrDefault()
        }).ToList();

        var sepettam= new SepetTam{
            Sepets=sepet,
            Sepeturuns=sepeturunTam
        };
        return sepettam;
    }

    //Sepette ürün var mı kontrol eder
    public bool getSepetUrun(string email,int urunKod){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=>a.KullaniciNo==entity.KullaniciNo);
        return context.Sepeturuns.Any(a=>a.UrunKod==urunKod && a.SepetId==sepet.SepetId);
    }

    //Sepet ürününü arttırır.
    public void updSepetUrun(string email,int urunKod){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=>a.KullaniciNo==entity.KullaniciNo);
        var sepeturun=context.Sepeturuns.FirstOrDefault(a=>a.SepetId==sepet.SepetId && a.UrunKod==urunKod);
        context.Database.ExecuteSqlRaw("CALL updSepetUrun(@_sepetUrunId)",
        new MySqlParameter("@_sepetUrunId", sepeturun.SepetUrunId)
        );
    }

    //Sepet urunü ekler.
    public void addSepetUrun(string email,int urunKod){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=>a.KullaniciNo==entity.KullaniciNo);
        context.Database.ExecuteSqlRaw("CALL addSepetUrun(@_sepetId, @_urunKod)",
        new MySqlParameter("@_sepetId", sepet.SepetId),
        new MySqlParameter("@_urunKod", urunKod)
        );
    }

    //Sepet ürünü siler.
    public void dellSepetUrun(string email,int urunKod){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=>a.KullaniciNo==entity.KullaniciNo);
        var sepeturun=context.Sepeturuns.FirstOrDefault(a=>a.SepetId==sepet.SepetId && a.UrunKod==urunKod);
        sepet.UrunSayisi-=sepeturun.Adet;
        sepet.SepetFİyati-=sepeturun.ToplamFiyat;
        context.Sepeturuns.Attach(sepeturun);
        context.Sepeturuns.Remove(sepeturun);
        context.SaveChanges();
    }

    //Sepetteki tüm ürünleri siler.
    public void delSepet(string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=>a.KullaniciNo==entity.KullaniciNo);
        var sepeturun=context.Sepeturuns.Where(a=>a.SepetId==sepet.SepetId).ToList();
        sepet.UrunSayisi=0;
        sepet.SepetFİyati=0;
        context.Sepeturuns.RemoveRange(sepeturun);
        context.SaveChanges();
    }

    //Kullanıcının adresi ve kredi kartı var mı kontrol eder.
    public bool payment(string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        if((entity.Adres1!=null || entity.Adres2!=null)&& (entity.Adres1!="" || entity.Adres2!="") && context.Kredikartlaris.Any(a=>a.KullaniciNo==entity.KullaniciNo)){
            return true;
        }
        return false;
    }

    //Sipariş için gerekli modeli oluşturur.
    public SiparislerModel getSiparisModel(string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var kredikartlari=context.Kredikartlaris.Where(a=>a.KullaniciNo==entity.KullaniciNo).ToList();
        var sepet=context.Sepets.FirstOrDefault(a=> a.KullaniciNo == entity.KullaniciNo);
        var sepeturuns=context.Sepeturuns.Where(a=>a.SepetId==sepet.SepetId).ToList();
        List<Sepeturun2> sepeturunTam= sepeturuns.Select(sm=> new Sepeturun2{
            SepetUrunId=sm.SepetUrunId,
            SepetId=sm.SepetId,
            UrunKod=sm.UrunKod,
            Adet=sm.Adet,
            ToplamFiyat=sm.ToplamFiyat,
            urunAd=context.Urunlers.Where(a=>a.UrunKod==sm.UrunKod).Select(a=>a.UrunAd).FirstOrDefault()
        }).ToList();

        var sepettam= new SepetTam{
            Sepets=sepet,
            Sepeturuns=sepeturunTam
        };

        var siparisModel=new SiparislerModel{
            Kullanicilar=entity,
            SepetTam=sepettam,
            Kredikartlari=kredikartlari
        };
        return siparisModel;

    }

    //Sipariş oluştur, sepeti sıfırla, stokları güncelle.
    public bool finishSiparis(string email,decimal sonFiyat,string adres){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var sepet=context.Sepets.FirstOrDefault(a=> a.KullaniciNo == entity.KullaniciNo);
        var sepeturuns=context.Sepeturuns.Where(a=>a.SepetId==sepet.SepetId).ToList();
        foreach (var sepeturun in sepeturuns){
            var urun=context.Urunlers.FirstOrDefault(a=>a.UrunKod==sepeturun.UrunKod);
            if(urun.StokDurum<sepeturun.Adet){
                return false;
            }
        }
        foreach (var sepeturun in sepeturuns){
            var urun=context.Urunlers.FirstOrDefault(a=>a.UrunKod==sepeturun.UrunKod);
            urun.SatisAdeti+=sepeturun.Adet;
            urun.StokDurum-=sepeturun.Adet;
            context.SaveChanges();
        }
        context.Database.ExecuteSqlRaw("CALL addSiparis(@_email, @_adres,@_sonFiyat)",
        new MySqlParameter("@_email", email),
        new MySqlParameter("@_adres", adres),
        new MySqlParameter("@_sonFiyat", sonFiyat)
        );
        return true;
    }

    //Kullanicinin siparişlerini döndürür.
    public List<Sipari> GetSiparis(string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var siparis=context.Siparis.Where(a=> a.KullaniciNo == entity.KullaniciNo).ToList();
        return siparis;
    }

    //Kredi kartı no sundan tarih geri döndür.
    public string findCardDate(string email){
        var entity=context.Kullanicilars.FirstOrDefault(a => a.Email == email);
        var creditCart=context.Kredikartlaris.FirstOrDefault(a=>a.KullaniciNo==entity.KullaniciNo);
        string tarih=creditCart.SonGecerlilikTarihi.ToString();
        return tarih;
    }
    //Veritabanı erişimini bitirir.
    public void Dispose()
    {
        context.Dispose();
    }
}