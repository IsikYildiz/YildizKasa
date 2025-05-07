using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YıldızKasa.Models;
using DatabaseOperations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Iana;
using SoapClientApp;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace YıldızKasa.Controllers;

/// Alışveriş sepeti ve sipariş işlemlerini yöneten controller sınıfı
public class TransactionPageController : Controller{
    /// Kullanıcının alışveriş sepetini gösterir
    public IActionResult Cart(){
        var sifre = HttpContext.Session.GetString("email");
        // Kullanıcı giriş yapmamışsa giriş sayfasına yönlendirir
        if(sifre==""){
            return RedirectToAction("SignIn", "LoginPage", new { area = "" });
        }
        Data ins=new();
        // Kullanıcının sepet bilgilerini getirir
        var model=ins.getSepet(HttpContext.Session.GetString("email"));
        return View("AlisverisSepeti",model);
    }

    /// Sepete ürün ekler veya sepetteki ürün miktarını günceller
    public IActionResult updSepet(int urunKod){
        Data ins=new();
        // Kullanıcı giriş yapmamışsa sepet sayfasına yönlendirir
        if(HttpContext.Session.GetString("email")==""){
            return RedirectToAction("Cart","TransactionPage");
        }
        // Ürün sepette varsa miktarını artırır, yoksa sepete ekler
        else if(ins.getSepetUrun(HttpContext.Session.GetString("email"),urunKod)){
            ins.updSepetUrun(HttpContext.Session.GetString("email"),urunKod);
            ins.Dispose();
        }
        else{
            ins.addSepetUrun(HttpContext.Session.GetString("email"),urunKod);
            ins.Dispose();
        }
        return RedirectToAction("Cart","TransactionPage");
    }

    /// Sepetten belirli bir ürünü siler
    public IActionResult dellSepetUrun(int urunKod){
        Data ins=new();
        // Sepetten ürünü siler
        ins.dellSepetUrun(HttpContext.Session.GetString("email"),urunKod);
        ins.Dispose();
        return RedirectToAction("Cart","TransactionPage");
    }

    /// Sepeti tamamen temizler
    public IActionResult delSepet(){
        Data ins=new();
        // Sepeti tamamen temizler
        ins.delSepet(HttpContext.Session.GetString("email"));
        ins.Dispose();
        return RedirectToAction("Cart","TransactionPage");
    }

    /// Ödeme işlemini gerçekleştirir
    public bool payment(){
        Data ins=new();
        // Ödeme işlemini gerçekleştirir
        if(ins.payment(HttpContext.Session.GetString("email"))){
            ins.Dispose();
            return false;
        }
        else{
            ins.Dispose();
            return true;
        }
    }

    /// Sipariş sayfasını gösterir
    public IActionResult Siparis(){
        Data ins=new();
        // Sipariş bilgilerini getirir
        var model=ins.getSiparisModel(HttpContext.Session.GetString("email"));
        ins.Dispose();
        return View("Siparis",model);
    }

    /// Kullanıcının tüm siparişlerini gösterir
    public IActionResult Siparisler(){
        Data ins=new();
        // Kullanıcının tüm siparişlerini getirir
        var model=ins.GetSiparis(HttpContext.Session.GetString("email"));
        ins.Dispose();
        return View("Siparisler",model);
    }

    /// Sipariş tamamlama ve ödeme işlemini gerçekleştirir
    [HttpPost]
    public async Task<IActionResult> finishSiparis([FromBody] PaymentRequest request)
    {
    if (!ModelState.IsValid)
    {
        return BadRequest(new { success = false, message = "Invalid request data" });
    }

    try
    {
        Data ins = new();
        // SOAP servisi ile ödeme işlemini gerçekleştirir
        string soapResponse = await Soap.ProcessPayment(
            request.cardNo,
            request.secCode,
            ins.findCardDate(HttpContext.Session.GetString("email")),
            request.adres,
            request.fiyat,
            request.urunAdet
        );

        // SOAP yanıtını JSON formatına dönüştürür
        var result = JsonConvert.DeserializeObject<PaymentResponse>(soapResponse);
        // KDV ekleyerek toplam fiyatı hesaplar (%10)
        var fiyat=request.fiyat*1.1;
        // Siparişi tamamlar
        bool success = ins.finishSiparis(HttpContext.Session.GetString("email"), (decimal)fiyat, request.adres);
        ins.Dispose();

        return Json(new { success = success, message = success ? "success" : "failure" });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = "System error occurred" });
    }
    }

    /// Ödeme isteği için kullanılan model sınıfı
    public class PaymentRequest
    {
    public string cardNo { get; set; }
    public int secCode { get; set; }
    public string adres { get; set; }
    public int fiyat { get; set; }
    public int urunAdet { get; set; }
    }
    public class PaymentResponse
    {
    public bool Success { get; set; }
    public string Message { get; set; }
    public double ShippingFee { get; set; }
    public double TotalAmount { get; set; }
    }
}
