using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using YıldızKasa;
using YıldızKasa.Models;
using DatabaseOperations;

namespace KullaniciYorum;

public class KullaniciYorumlar:ViewComponent{
    public IViewComponentResult Invoke(int name){
        Data ins=new();
        var model=ins.GetYorumlar(name);
        ins.Dispose();
        return View(model);
    }
}