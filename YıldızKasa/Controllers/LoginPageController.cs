using Microsoft.AspNetCore.Mvc;
using DatabaseOperations;
using YıldızKasa.Models;

namespace YıldızKasa.Controllers;

/// Kullanıcı giriş, kayıt ve hesap işlemlerini yöneten controller sınıfı
[ApiController]
[Route("[controller]")]
public class LoginPageController:Controller{
    /// Kullanıcı giriş sayfasını gösterir
    [HttpGet("signin")]
    public IActionResult SignIn(){
        return View("GirisYap");
    }

    /// Başarılı üyelik sonrası giriş sayfasını gösterir ve bilgi mesajı ekler
    [HttpGet("signinfirst")]
    public IActionResult SignInFirst(){
        ViewData["Uye"]="Başarıyla üye olundu!";
        return View("GirisYap");
    }

    /// Üye olma sayfasını gösterir
    [HttpGet("signup")]
    public IActionResult SignUp(){
        return View("UyeOl");
    }

    /// Kullanıcı hesap bilgilerini gösterir
    [HttpGet("account")]
    public IActionResult Account(){
        Data ins=new();
        // Session'da saklanan email ile kullanıcı bilgilerini getirir
        var model=ins.getKullanici(HttpContext.Session.GetString("email"));
        ins.Dispose();
        return View("User",model);
    }

    /// Email veya şifre kontrolü yapar
    [HttpGet("control")]
    public bool Control(string toControl,string which){
        Data ins=new();
        if(which.Equals("email")){
            // Email kontrolü yapar
            var response=ins.ControlEmail(toControl);
            Console.WriteLine(response);
            ins.Dispose();
            return response;
        }
        else{
            // Şifre kontrolü yapar
            var response=ins.ControlSifre(toControl);
            ins.Dispose();
            return response;
        }
    }

    /// Yeni kullanıcı ekler
    [HttpPost("adduser")]
    public IActionResult addUser([FromBody] Kullanicilar kullanici){
        string ad=kullanici.Ad;
        string soyad=kullanici.Soyad;
        string email=kullanici.Email;
        string sifre=kullanici.Sifre;
        Data ins=new();
        ins.addUser(ad,soyad,email,sifre);
        ins.Dispose();
        return Ok(new { success = true, message = "success" });
    }

    /// Kullanıcı bilgilerini günceller
    [HttpPost("upduser")]
    public IActionResult updUser([FromBody] Kullanicilar kullanici){
        string ad=kullanici.Ad;
        string soyad=kullanici.Soyad;
        string email=kullanici.Email;
        string sifre=kullanici.Sifre;
        string adres1=kullanici.Adres1;
        string adres2=kullanici.Adres2;
        int No=kullanici.KullaniciNo;
        Data ins=new();
        ins.updUser(ad,soyad,email,sifre,adres1,adres2,No);
        ins.Dispose();
        return Ok(new { success = true, message = "success" });
    }

    /// Kullanıcı hesabını siler
    [HttpGet("deluser")]
    public IActionResult delUser(int No){
        Data ins=new();
        ins.delUser(No);
        ins.Dispose();
        // Kullanıcı oturumunu sonlandırır
        HttpContext.Session.SetString("email","");
        return Ok(new { success = true, message = "success" });
    }

    /// Kullanıcıya kredi kartı ekler
    [HttpPost("addcard")]
    public IActionResult addCard([FromBody] Kredikartlari2 kredikartlari){
        string KartNo=kredikartlari.KartNo;
        string SonGecerlilikTarihi=kredikartlari.SonGecerlilikTarihi;
        string KartSahipIsmi=kredikartlari.KartSahipIsmi;
        DateOnly Tarih=DateOnly.MinValue;
        try{
            // Tarih formatını dönüştürür
            Tarih = DateOnly.Parse(SonGecerlilikTarihi);
        }
        catch (FormatException ex){
            Console.WriteLine("Hata: Geçersiz tarih formatı. Lütfen 'yyyy-MM-dd' formatında bir tarih girin.");
        }
        Data ins=new();
        ins.addCard(HttpContext.Session.GetString("email"),KartNo,Tarih,KartSahipIsmi);
        ins.Dispose();
        return Ok(new { success = true, message = "success" });
    }

    /// Kullanıcının kredi kartını siler
    [HttpGet("delcard")]
    public IActionResult delCard(int cardId){
        Data ins=new();
        ins.delCard(cardId);
        ins.Dispose();
        return RedirectToAction("index","HomePage");
    }

    /// Email adresinin geçerliliğini kontrol eder
    [HttpGet("emailvalid")]
    public async Task<bool> emailValid(string email){
        var emailValidator = new EmailValidationService();
        bool isValid = await emailValidator.ValidateEmailAsync(email);
        return isValid;
    }
}