using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YıldızKasa.Models;
using DatabaseOperations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace YıldızKasa.Controllers;

/// Ana sayfa ve ürün işlemlerini yöneten controller sınıfı
public class HomePageController : Controller
{
    /// Ana sayfayı gösterir ve gerekli verileri yükler
    public IActionResult Index()
    {
        // Kullanıcı oturumu kontrolü
        if(string.IsNullOrEmpty(HttpContext.Session.GetString("email"))){
            HttpContext.Session.SetString("email","");
        }
        Data ins=new();
        // Ana sayfa için gerekli verileri getirir
        var model=ins.GetHomePageViewModel();
        ins.Dispose();
        return View("AnaSayfa",model);
    }

    /// Kullanıcı oturumunu sonlandırır
    public IActionResult endSession(){
        HttpContext.Session.SetString("email","");
        return RedirectToAction("Index","HomePage");
    }

    /// Kullanıcı oturumunu başlatır ve session bilgisini günceller
    public IActionResult signed(string email){
        // Session'a email bilgisini kaydeder
        HttpContext.Session.SetString("email",email);
        Data ins=new();
        // Kullanıcı oturum bilgisini veritabanında günceller
        ins.updUserSession(email);
        ins.Dispose();
        return RedirectToAction("Index","HomePage");
    }

    /// Belirli bir ürünün detay sayfasını gösterir.
    public IActionResult getUrun(string product){
        Data ins=new();
        // Ürün detaylarını getirir
        var model=ins.GetUrunModel(product);
        // Kullanıcının ürün hakkında yorum yapıp yapmadığını kontrol eder
        ViewData["Yorum"]=thereYorum(model.UrunKod);
        return View("UrunSayfa",model);
    }

    /// Ürün araması yapar ve sonuçları gösterir
    public IActionResult findUrun(string productName){
        Data ins=new();
        // Ürün araması yapar
        var model=ins.findUrun(productName);
        ins.Dispose();
        // Arama sorgusunu ViewData'ya ekler
        ViewData["Arama"]=productName;
        return View("UrunlerSayfa",model);
    }

    /// Kullanıcının belirli bir ürün hakkında yorum yapıp yapmadığını kontrol eder
    public string thereYorum(int urunKod){
        Data ins=new();
        // Kullanıcının ürün hakkında yorum yapıp yapmadığını kontrol eder
        var result=ins.thereYorum(urunKod,HttpContext.Session.GetString("email"));
        ins.Dispose();
        return result;
    }

    /// Kullanıcının ürün hakkında yorum yapmasını sağlar
    /// <param name="UrunKod">Ürün kodu</param>
    /// <param name="yorum">Yorum metni</param>
    /// <param name="puan">Ürün puanı</param>
    public IActionResult sendYorum(int UrunKod,string yorum,int puan){
        Data ins=new();
        byte puanByte=(byte)puan;
        // Kullanıcının yorumunu veritabanına ekler
        ins.addYorum(UrunKod,yorum,puanByte,HttpContext.Session.GetString("email"));
        ins.Dispose();
        return Ok("Başarılı");
    }

    /// Kullanıcının ürün hakkında yaptığı yorumu siler
    /// <param name="UrunKod">Ürün kodu</param>
    public IActionResult delYorum(int UrunKod){
        Data ins=new();
        // Kullanıcının yorumunu veritabanından siler
        ins.delYorum(UrunKod,HttpContext.Session.GetString("email"));
        ins.Dispose();
        return Ok("Başarılı");
    }

    /// Hata sayfasını gösterir
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
