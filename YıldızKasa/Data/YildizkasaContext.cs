using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using YıldızKasa.Models;
using DotNetEnv;

namespace YıldızKasa;

public partial class YildizkasaContext : DbContext
{
    public YildizkasaContext()
    {
    }

    public YildizkasaContext(DbContextOptions<YildizkasaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kredikartlari> Kredikartlaris { get; set; }

    public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }

    public virtual DbSet<Listdizustu> Listdizustus { get; set; }

    public virtual DbSet<Listindirimliurunler> Listindirimliurunlers { get; set; }

    public virtual DbSet<Listkullanic> Listkullanics { get; set; }

    public virtual DbSet<Listmarka> Listmarkas { get; set; }

    public virtual DbSet<Listmasaustu> Listmasaustus { get; set; }

    public virtual DbSet<Listsipari> Listsiparis { get; set; }

    public virtual DbSet<Listurunler> Listurunlers { get; set; }

    public virtual DbSet<Marka> Markas { get; set; }

    public virtual DbSet<Sepet> Sepets { get; set; }

    public virtual DbSet<Sepeturun> Sepeturuns { get; set; }

    public virtual DbSet<Sipari> Siparis { get; set; }

    public virtual DbSet<Urunler> Urunlers { get; set; }

    public virtual DbSet<Yorum> Yorums { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        Env.TraversePath().Load();
        optionsBuilder.UseMySql("server=localhost;database="+Env.GetString("db_name")+";user="+Env.GetString("db_user")+";password="+Env.GetString("db_password")+"", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Kredikartlari>(entity =>
        {
            entity.HasKey(e => e.KartId).HasName("PRIMARY");

            entity.ToTable("kredikartlari");

            entity.HasIndex(e => e.KullaniciNo, "fk_kullanici");

            entity.Property(e => e.KartId).HasColumnName("kartID");
            entity.Property(e => e.KartNo)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("kartNo");
            entity.Property(e => e.KartSahipIsmi)
                .HasMaxLength(80)
                .HasColumnName("kartSahipIsmi");
            entity.Property(e => e.KullaniciNo).HasColumnName("kullaniciNo");
            entity.Property(e => e.SonGecerlilikTarihi).HasColumnName("sonGecerlilikTarihi");

            entity.HasOne(d => d.KullaniciNoNavigation).WithMany(p => p.Kredikartlaris)
                .HasForeignKey(d => d.KullaniciNo)
                .HasConstraintName("fk_kullanici");
        });

        modelBuilder.Entity<Kullanicilar>(entity =>
        {
            entity.HasKey(e => e.KullaniciNo).HasName("PRIMARY");

            entity.ToTable("kullanicilar");

            entity.HasIndex(e => e.Email, "email_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Sifre, "sifre_UNIQUE").IsUnique();

            entity.HasIndex(e => new { e.Email, e.Sifre }, "un_email").IsUnique();

            entity.Property(e => e.KullaniciNo).HasColumnName("kullaniciNo");
            entity.Property(e => e.Ad)
                .HasMaxLength(40)
                .HasColumnName("ad");
            entity.Property(e => e.Adres1)
                .HasMaxLength(70)
                .HasColumnName("adres1");
            entity.Property(e => e.Adres2)
                .HasMaxLength(70)
                .HasColumnName("adres2");
            entity.Property(e => e.Bakiye)
                .HasPrecision(19, 4)
                .HasColumnName("bakiye");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.GirisSayisi).HasColumnName("girisSayisi");
            entity.Property(e => e.Sifre)
                .HasMaxLength(100)
                .HasColumnName("sifre");
            entity.Property(e => e.SonGirisTarihi).HasColumnName("sonGirisTarihi");
            entity.Property(e => e.Soyad)
                .HasMaxLength(40)
                .HasColumnName("soyad");
            entity.Property(e => e.YetkiSeviyesi)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)")
                .HasColumnName("yetkiSeviyesi");
        });

        modelBuilder.Entity<Listdizustu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listdizustu");

            entity.Property(e => e.Cozunurluk)
                .HasMaxLength(15)
                .HasColumnName("cozunurluk");
            entity.Property(e => e.Depolama1)
                .HasMaxLength(10)
                .HasColumnName("depolama1");
            entity.Property(e => e.Depolama2)
                .HasMaxLength(10)
                .HasColumnName("depolama2");
            entity.Property(e => e.Depolama3)
                .HasMaxLength(10)
                .HasColumnName("depolama3");
            entity.Property(e => e.Depolama4)
                .HasMaxLength(10)
                .HasColumnName("depolama4");
            entity.Property(e => e.EkranBoyutu)
                .HasMaxLength(10)
                .HasColumnName("ekranBoyutu");
            entity.Property(e => e.EkranKarti)
                .HasMaxLength(100)
                .HasColumnName("ekranKarti");
            entity.Property(e => e.EkranYenilemeHizi).HasColumnName("ekranYenilemeHizi");
            entity.Property(e => e.Garanti).HasColumnName("garanti");
            entity.Property(e => e.Indirim).HasColumnName("indirim");
            entity.Property(e => e.Islemci)
                .HasMaxLength(100)
                .HasColumnName("islemci");
            entity.Property(e => e.IsletimSistemi)
                .HasMaxLength(15)
                .HasColumnName("isletimSistemi");
            entity.Property(e => e.KasaTipi)
                .HasMaxLength(10)
                .HasColumnName("kasaTipi");
            entity.Property(e => e.Kdvoran)
                .HasDefaultValueSql("'20'")
                .HasColumnName("KDVoran");
            entity.Property(e => e.ListeFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("listeFiyat");
            entity.Property(e => e.MarkaId).HasColumnName("markaId");
            entity.Property(e => e.Ram)
                .HasMaxLength(10)
                .HasColumnName("ram");
            entity.Property(e => e.SatisAdeti).HasColumnName("satisAdeti");
            entity.Property(e => e.StokDurum).HasColumnName("stokDurum");
            entity.Property(e => e.UrunAd)
                .HasMaxLength(100)
                .HasColumnName("urunAd");
            entity.Property(e => e.UrunKod).HasColumnName("urunKod");
            entity.Property(e => e.UrunResim)
                .HasMaxLength(100)
                .HasColumnName("urunResim");
            entity.Property(e => e.UrunTanitim)
                .HasColumnType("text")
                .HasColumnName("urunTanitim");
            entity.Property(e => e.VergisizFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("vergisizFiyat");
        });

        modelBuilder.Entity<Listindirimliurunler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listindirimliurunler");

            entity.Property(e => e.Cozunurluk)
                .HasMaxLength(15)
                .HasColumnName("cozunurluk");
            entity.Property(e => e.Depolama1)
                .HasMaxLength(10)
                .HasColumnName("depolama1");
            entity.Property(e => e.Depolama2)
                .HasMaxLength(10)
                .HasColumnName("depolama2");
            entity.Property(e => e.Depolama3)
                .HasMaxLength(10)
                .HasColumnName("depolama3");
            entity.Property(e => e.Depolama4)
                .HasMaxLength(10)
                .HasColumnName("depolama4");
            entity.Property(e => e.EkranBoyutu)
                .HasMaxLength(10)
                .HasColumnName("ekranBoyutu");
            entity.Property(e => e.EkranKarti)
                .HasMaxLength(100)
                .HasColumnName("ekranKarti");
            entity.Property(e => e.EkranYenilemeHizi).HasColumnName("ekranYenilemeHizi");
            entity.Property(e => e.Garanti).HasColumnName("garanti");
            entity.Property(e => e.Indirim).HasColumnName("indirim");
            entity.Property(e => e.Islemci)
                .HasMaxLength(100)
                .HasColumnName("islemci");
            entity.Property(e => e.IsletimSistemi)
                .HasMaxLength(15)
                .HasColumnName("isletimSistemi");
            entity.Property(e => e.KasaTipi)
                .HasMaxLength(10)
                .HasColumnName("kasaTipi");
            entity.Property(e => e.Kdvoran)
                .HasDefaultValueSql("'20'")
                .HasColumnName("KDVoran");
            entity.Property(e => e.ListeFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("listeFiyat");
            entity.Property(e => e.MarkaId).HasColumnName("markaId");
            entity.Property(e => e.Ram)
                .HasMaxLength(10)
                .HasColumnName("ram");
            entity.Property(e => e.SatisAdeti).HasColumnName("satisAdeti");
            entity.Property(e => e.StokDurum).HasColumnName("stokDurum");
            entity.Property(e => e.UrunAd)
                .HasMaxLength(100)
                .HasColumnName("urunAd");
            entity.Property(e => e.UrunKod).HasColumnName("urunKod");
            entity.Property(e => e.UrunResim)
                .HasMaxLength(100)
                .HasColumnName("urunResim");
            entity.Property(e => e.UrunTanitim)
                .HasColumnType("text")
                .HasColumnName("urunTanitim");
            entity.Property(e => e.VergisizFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("vergisizFiyat");
        });

        modelBuilder.Entity<Listkullanic>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listkullanic");

            entity.Property(e => e.Ad)
                .HasMaxLength(100)
                .HasColumnName("ad");
            entity.Property(e => e.Adres1)
                .HasMaxLength(100)
                .HasColumnName("adres1");
            entity.Property(e => e.Adres2)
                .HasMaxLength(100)
                .HasColumnName("adres2");
            entity.Property(e => e.Bakiye)
                .HasPrecision(19, 4)
                .HasColumnName("bakiye");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.GirisSayisi).HasColumnName("girisSayisi");
            entity.Property(e => e.SonGirisTarihi).HasColumnName("sonGirisTarihi");
            entity.Property(e => e.Soyad)
                .HasMaxLength(100)
                .HasColumnName("soyad");
        });

        modelBuilder.Entity<Listmarka>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listmarka");

            entity.Property(e => e.Marka)
                .HasMaxLength(40)
                .HasColumnName("marka");
            entity.Property(e => e.Web)
                .HasMaxLength(150)
                .HasColumnName("web");
        });

        modelBuilder.Entity<Listmasaustu>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listmasaustu");

            entity.Property(e => e.Cozunurluk)
                .HasMaxLength(15)
                .HasColumnName("cozunurluk");
            entity.Property(e => e.Depolama1)
                .HasMaxLength(10)
                .HasColumnName("depolama1");
            entity.Property(e => e.Depolama2)
                .HasMaxLength(10)
                .HasColumnName("depolama2");
            entity.Property(e => e.Depolama3)
                .HasMaxLength(10)
                .HasColumnName("depolama3");
            entity.Property(e => e.Depolama4)
                .HasMaxLength(10)
                .HasColumnName("depolama4");
            entity.Property(e => e.EkranBoyutu)
                .HasMaxLength(10)
                .HasColumnName("ekranBoyutu");
            entity.Property(e => e.EkranKarti)
                .HasMaxLength(100)
                .HasColumnName("ekranKarti");
            entity.Property(e => e.EkranYenilemeHizi).HasColumnName("ekranYenilemeHizi");
            entity.Property(e => e.Garanti).HasColumnName("garanti");
            entity.Property(e => e.Indirim).HasColumnName("indirim");
            entity.Property(e => e.Islemci)
                .HasMaxLength(100)
                .HasColumnName("islemci");
            entity.Property(e => e.IsletimSistemi)
                .HasMaxLength(15)
                .HasColumnName("isletimSistemi");
            entity.Property(e => e.KasaTipi)
                .HasMaxLength(10)
                .HasColumnName("kasaTipi");
            entity.Property(e => e.Kdvoran)
                .HasDefaultValueSql("'20'")
                .HasColumnName("KDVoran");
            entity.Property(e => e.ListeFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("listeFiyat");
            entity.Property(e => e.MarkaId).HasColumnName("markaId");
            entity.Property(e => e.Ram)
                .HasMaxLength(10)
                .HasColumnName("ram");
            entity.Property(e => e.SatisAdeti).HasColumnName("satisAdeti");
            entity.Property(e => e.StokDurum).HasColumnName("stokDurum");
            entity.Property(e => e.UrunAd)
                .HasMaxLength(100)
                .HasColumnName("urunAd");
            entity.Property(e => e.UrunKod).HasColumnName("urunKod");
            entity.Property(e => e.UrunResim)
                .HasMaxLength(100)
                .HasColumnName("urunResim");
            entity.Property(e => e.UrunTanitim)
                .HasColumnType("text")
                .HasColumnName("urunTanitim");
            entity.Property(e => e.VergisizFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("vergisizFiyat");
        });

        modelBuilder.Entity<Listsipari>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listsiparis");

            entity.Property(e => e.GonderilenAdres)
                .HasMaxLength(70)
                .HasColumnName("gonderilenAdres");
            entity.Property(e => e.KullaniciNo).HasColumnName("kullaniciNo");
            entity.Property(e => e.SiparisDurumu)
                .HasMaxLength(50)
                .HasDefaultValueSql("'kargoya veriliyor'")
                .HasColumnName("siparisDurumu");
            entity.Property(e => e.SiparisId).HasColumnName("siparisId");
            entity.Property(e => e.SiparisTarihi).HasColumnName("siparisTarihi");
            entity.Property(e => e.SonFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("sonFiyat");
        });

        modelBuilder.Entity<Listurunler>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("listurunler");

            entity.Property(e => e.Cozunurluk)
                .HasMaxLength(15)
                .HasColumnName("cozunurluk");
            entity.Property(e => e.Depolama1)
                .HasMaxLength(10)
                .HasColumnName("depolama1");
            entity.Property(e => e.Depolama2)
                .HasMaxLength(10)
                .HasColumnName("depolama2");
            entity.Property(e => e.Depolama3)
                .HasMaxLength(10)
                .HasColumnName("depolama3");
            entity.Property(e => e.Depolama4)
                .HasMaxLength(10)
                .HasColumnName("depolama4");
            entity.Property(e => e.EkranBoyutu)
                .HasMaxLength(10)
                .HasColumnName("ekranBoyutu");
            entity.Property(e => e.EkranKarti)
                .HasMaxLength(100)
                .HasColumnName("ekranKarti");
            entity.Property(e => e.EkranYenilemeHizi).HasColumnName("ekranYenilemeHizi");
            entity.Property(e => e.Garanti).HasColumnName("garanti");
            entity.Property(e => e.Indirim).HasColumnName("indirim");
            entity.Property(e => e.Islemci)
                .HasMaxLength(100)
                .HasColumnName("islemci");
            entity.Property(e => e.IsletimSistemi)
                .HasMaxLength(15)
                .HasColumnName("isletimSistemi");
            entity.Property(e => e.KasaTipi)
                .HasMaxLength(10)
                .HasColumnName("kasaTipi");
            entity.Property(e => e.Kdvoran)
                .HasDefaultValueSql("'20'")
                .HasColumnName("KDVoran");
            entity.Property(e => e.ListeFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("listeFiyat");
            entity.Property(e => e.MarkaId).HasColumnName("markaId");
            entity.Property(e => e.Ram)
                .HasMaxLength(10)
                .HasColumnName("ram");
            entity.Property(e => e.SatisAdeti).HasColumnName("satisAdeti");
            entity.Property(e => e.StokDurum).HasColumnName("stokDurum");
            entity.Property(e => e.UrunAd)
                .HasMaxLength(100)
                .HasColumnName("urunAd");
            entity.Property(e => e.UrunKod).HasColumnName("urunKod");
            entity.Property(e => e.UrunResim)
                .HasMaxLength(100)
                .HasColumnName("urunResim");
            entity.Property(e => e.UrunTanitim)
                .HasColumnType("text")
                .HasColumnName("urunTanitim");
            entity.Property(e => e.VergisizFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("vergisizFiyat");
        });

        modelBuilder.Entity<Marka>(entity =>
        {
            entity.HasKey(e => e.MarkaId).HasName("PRIMARY");

            entity.ToTable("marka");

            entity.HasIndex(e => e.MarkaAd, "markaAd_UNIQUE").IsUnique();

            entity.HasIndex(e => e.MarkaWeb, "markaWeb_UNIQUE").IsUnique();

            entity.Property(e => e.MarkaId).HasColumnName("markaId");
            entity.Property(e => e.MarkaAd)
                .HasMaxLength(40)
                .HasColumnName("markaAd");
            entity.Property(e => e.MarkaWeb)
                .HasMaxLength(150)
                .HasColumnName("markaWeb");
        });

        modelBuilder.Entity<Sepet>(entity =>
        {
            entity.HasKey(e => e.SepetId).HasName("PRIMARY");

            entity.ToTable("sepet");

            entity.HasIndex(e => e.KullaniciNo, "fk_kullaniciSepet");

            entity.Property(e => e.SepetId).HasColumnName("sepetId");
            entity.Property(e => e.KullaniciNo).HasColumnName("kullaniciNo");
            entity.Property(e => e.SepetFİyati)
                .HasPrecision(19, 4)
                .HasDefaultValueSql("'0.0000'")
                .HasColumnName("sepetFİyati");
            entity.Property(e => e.UrunSayisi)
                .HasDefaultValueSql("'0'")
                .HasColumnName("urunSayisi");

            entity.HasOne(d => d.KullaniciNoNavigation).WithMany(p => p.Sepets)
                .HasForeignKey(d => d.KullaniciNo)
                .HasConstraintName("fk_kullaniciSepet");
        });

        modelBuilder.Entity<Sepeturun>(entity =>
        {
            entity.HasKey(e => e.SepetUrunId).HasName("PRIMARY");

            entity.ToTable("sepeturun");

            entity.HasIndex(e => e.SepetId, "fk_sepet");

            entity.HasIndex(e => e.UrunKod, "fk_urun");

            entity.Property(e => e.SepetUrunId).HasColumnName("sepetUrunId");
            entity.Property(e => e.Adet).HasColumnName("adet");
            entity.Property(e => e.SepetId).HasColumnName("sepetId");
            entity.Property(e => e.ToplamFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("toplamFiyat");
            entity.Property(e => e.UrunKod).HasColumnName("urunKod");

            entity.HasOne(d => d.Sepet).WithMany(p => p.Sepeturuns)
                .HasForeignKey(d => d.SepetId)
                .HasConstraintName("fk_sepet");

            entity.HasOne(d => d.UrunKodNavigation).WithMany(p => p.Sepeturuns)
                .HasForeignKey(d => d.UrunKod)
                .HasConstraintName("fk_urun");
        });

        modelBuilder.Entity<Sipari>(entity =>
        {
            entity.HasKey(e => e.SiparisId).HasName("PRIMARY");

            entity.ToTable("siparis");

            entity.HasIndex(e => e.KullaniciNo, "fk_kullaniciSiparis");

            entity.Property(e => e.SiparisId).HasColumnName("siparisId");
            entity.Property(e => e.GonderilenAdres)
                .HasMaxLength(70)
                .HasColumnName("gonderilenAdres");
            entity.Property(e => e.KullaniciNo).HasColumnName("kullaniciNo");
            entity.Property(e => e.SiparisDurumu)
                .HasMaxLength(50)
                .HasDefaultValueSql("'kargoya veriliyor'")
                .HasColumnName("siparisDurumu");
            entity.Property(e => e.SiparisTarihi).HasColumnName("siparisTarihi");
            entity.Property(e => e.SonFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("sonFiyat");

            entity.HasOne(d => d.KullaniciNoNavigation).WithMany(p => p.Siparis)
                .HasForeignKey(d => d.KullaniciNo)
                .HasConstraintName("fk_kullaniciSiparis");
        });

        modelBuilder.Entity<Urunler>(entity =>
        {
            entity.HasKey(e => e.UrunKod).HasName("PRIMARY");

            entity.ToTable("urunler");

            entity.HasIndex(e => e.MarkaId, "fk_marka");

            entity.HasIndex(e => e.UrunAd, "urunAd_UNIQUE").IsUnique();

            entity.Property(e => e.UrunKod).HasColumnName("urunKod");
            entity.Property(e => e.Cozunurluk)
                .HasMaxLength(15)
                .HasColumnName("cozunurluk");
            entity.Property(e => e.Depolama1)
                .HasMaxLength(10)
                .HasColumnName("depolama1");
            entity.Property(e => e.Depolama2)
                .HasMaxLength(10)
                .HasColumnName("depolama2");
            entity.Property(e => e.Depolama3)
                .HasMaxLength(10)
                .HasColumnName("depolama3");
            entity.Property(e => e.Depolama4)
                .HasMaxLength(10)
                .HasColumnName("depolama4");
            entity.Property(e => e.EkranBoyutu)
                .HasMaxLength(10)
                .HasColumnName("ekranBoyutu");
            entity.Property(e => e.EkranKarti)
                .HasMaxLength(100)
                .HasColumnName("ekranKarti");
            entity.Property(e => e.EkranYenilemeHizi).HasColumnName("ekranYenilemeHizi");
            entity.Property(e => e.Garanti).HasColumnName("garanti");
            entity.Property(e => e.Indirim).HasColumnName("indirim");
            entity.Property(e => e.Islemci)
                .HasMaxLength(100)
                .HasColumnName("islemci");
            entity.Property(e => e.IsletimSistemi)
                .HasMaxLength(15)
                .HasColumnName("isletimSistemi");
            entity.Property(e => e.KasaTipi)
                .HasMaxLength(10)
                .HasColumnName("kasaTipi");
            entity.Property(e => e.Kdvoran)
                .HasDefaultValueSql("'20'")
                .HasColumnName("KDVoran");
            entity.Property(e => e.ListeFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("listeFiyat");
            entity.Property(e => e.MarkaId).HasColumnName("markaId");
            entity.Property(e => e.Ram)
                .HasMaxLength(10)
                .HasColumnName("ram");
            entity.Property(e => e.SatisAdeti).HasColumnName("satisAdeti");
            entity.Property(e => e.StokDurum).HasColumnName("stokDurum");
            entity.Property(e => e.UrunAd)
                .HasMaxLength(100)
                .HasColumnName("urunAd");
            entity.Property(e => e.UrunResim)
                .HasMaxLength(100)
                .HasColumnName("urunResim");
            entity.Property(e => e.UrunTanitim)
                .HasColumnType("text")
                .HasColumnName("urunTanitim");
            entity.Property(e => e.VergisizFiyat)
                .HasPrecision(19, 4)
                .HasColumnName("vergisizFiyat");

            entity.HasOne(d => d.Marka).WithMany(p => p.Urunlers)
                .HasForeignKey(d => d.MarkaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_marka");
        });

        modelBuilder.Entity<Yorum>(entity =>
        {
            entity.HasKey(e => e.YorumId).HasName("PRIMARY");

            entity.ToTable("yorum");

            entity.HasIndex(e => e.KullaniciNo, "fk_yorumKullanici");

            entity.HasIndex(e => e.UrunKod, "fk_yorumUrun");

            entity.Property(e => e.YorumId).HasColumnName("yorumId");
            entity.Property(e => e.KullaniciAd)
                .HasMaxLength(45)
                .HasColumnName("kullaniciAd");
            entity.Property(e => e.KullaniciNo).HasColumnName("kullaniciNo");
            entity.Property(e => e.Puan).HasColumnName("puan");
            entity.Property(e => e.UrunKod).HasColumnName("urunKod");
            entity.Property(e => e.Yorum1)
                .HasColumnType("text")
                .HasColumnName("yorum");

            entity.HasOne(d => d.KullaniciNoNavigation).WithMany(p => p.Yorums)
                .HasForeignKey(d => d.KullaniciNo)
                .HasConstraintName("fk_yorumKullanici");

            entity.HasOne(d => d.UrunKodNavigation).WithMany(p => p.Yorums)
                .HasForeignKey(d => d.UrunKod)
                .HasConstraintName("fk_yorumUrun");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
