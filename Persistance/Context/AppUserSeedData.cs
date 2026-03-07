using Domain.Entitiy;
using Domain.Entitiy.Jobs;
using Domain.GlobalEnum;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Context
{
    public class AppUserSeedData
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AppUserSeedData(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            // 1. Rollerin oluşturulması
            string[] roleNames = { "Carpenter", "Assembler", "PanelSawyer" };
            foreach (var roleName in roleNames)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Marangoz (Carpenter) Verileri
            var carpenters = new List<AppUser>
            {
                new AppUser { FullName = "Ahmet Yılmaz", UserName = "ahmet.yilmaz@marangoz.com", Email = "ahmet.yilmaz@marangoz.com", UserCategory = Category.Carpenter, City = "İstanbul", Jobs = new Carpenter { WorkShopName = "Yılmaz Mobilya", AdressDescription = "İkitelli OSB, İstanbul" } },
                new AppUser { FullName = "Mehmet Kaya", UserName = "mehmet.kaya@atolye.com", Email = "mehmet.kaya@atolye.com", UserCategory = Category.Carpenter, City = "Ankara", Jobs = new Carpenter { WorkShopName = "Kaya Ahşap", AdressDescription = "Siteler, Ankara" } },
                new AppUser { FullName = "Mustafa Demir", UserName = "mustafa.demir@ustam.com", Email = "mustafa.demir@ustam.com", UserCategory = Category.Carpenter, City = "İzmir", Jobs = new Carpenter { WorkShopName = "Demir Doğrama", AdressDescription = "Karabağlar, İzmir" } },
                new AppUser { FullName = "Süleyman Çelik", UserName = "suleyman.celik@celikmobilya.com", Email = "suleyman.celik@celikmobilya.com", UserCategory = Category.Carpenter, City = "Bursa", Jobs = new Carpenter { WorkShopName = "Çelik Kardeşler Mobilya", AdressDescription = "İnegöl, Bursa" } },
                new AppUser { FullName = "Osman Nuri", UserName = "osman.nuri@antalyadekor.com", Email = "osman.nuri@antalyadekor.com", UserCategory = Category.Carpenter, City = "Antalya", Jobs = new Carpenter { WorkShopName = "Akdeniz Tasarım", AdressDescription = "Lara, Antalya" } },
                new AppUser { FullName = "Hasan Tahsin", UserName = "hasan.tahsin@karadeniz.com", Email = "hasan.tahsin@karadeniz.com", UserCategory = Category.Carpenter, City = "Samsun", Jobs = new Carpenter { WorkShopName = "Karadeniz Ahşap İşleri", AdressDescription = "Tekkeköy, Samsun" } }
            };

            foreach (var user in carpenters) await CreateUserWithJob(user, "Usta123!", "Carpenter");

            // 3. Montajcı (Assembler) Verileri
            var assemblers = new List<AppUser>
            {
                new AppUser { FullName = "Caner Özkan", UserName = "caner.ozkan@montaj.com", Email = "caner.ozkan@montaj.com", UserCategory = Category.Assembler, City = "Kocaeli", Jobs = new Assembler { WorkShopName = "Hızlı Montaj", AdressDescription = "Gebze, Kocaeli" } },
                new AppUser { FullName = "Emre Bulut", UserName = "emre.bulut@servis.com", Email = "emre.bulut@servis.com", UserCategory = Category.Assembler, City = "İstanbul", Jobs = new Assembler { WorkShopName = "Bulut Teknik", AdressDescription = "Kadıköy, İstanbul" } },
                new AppUser { FullName = "Oğuzhan Koç", UserName = "oguzhan.koc@montajci.com", Email = "oguzhan.koc@montajci.com", UserCategory = Category.Assembler, City = "Adana", Jobs = new Assembler { WorkShopName = "Koç Montaj Grubu", AdressDescription = "Seyhan, Adana" } },
                new AppUser { FullName = "Burak Deniz", UserName = "burak.deniz@uydum.com", Email = "burak.deniz@uydum.com", UserCategory = Category.Assembler, City = "Ankara", Jobs = new Assembler { WorkShopName = "Deniz Kurulum", AdressDescription = "Çankaya, Ankara" } },
                new AppUser { FullName = "Serkan Kurt", UserName = "serkan.kurt@ekip.com", Email = "serkan.kurt@ekip.com", UserCategory = Category.Assembler, City = "Konya", Jobs = new Assembler { WorkShopName = "Kurt Montaj", AdressDescription = "Selçuklu, Konya" } },
                new AppUser { FullName = "Yiğit Aras", UserName = "yigit.aras@aras.com", Email = "yigit.aras@aras.com", UserCategory = Category.Assembler, City = "İzmir", Jobs = new Assembler { WorkShopName = "Aras Servis", AdressDescription = "Bornova, İzmir" } }
            };

            foreach (var user in assemblers) await CreateUserWithJob(user, "Montaj123!", "Assembler");

            // 4. Ebatlamacı (PanelSawyer) Verileri
            var sawyers = new List<AppUser>
            {
                new AppUser { FullName = "Kemal Keskin", UserName = "kemal.keskin@ebatlama.com", Email = "kemal.keskin@ebatlama.com", UserCategory = Category.PanelSawyer, City = "İstanbul", Jobs = new PanelSawyer { WorkShopName = "Hassas Kesim Merkezi", AdressDescription = "Dudullu OSB, İstanbul" } },
                new AppUser { FullName = "Zeki Alasya", UserName = "zeki.alasya@panel.com", Email = "zeki.alasya@panel.com", UserCategory = Category.PanelSawyer, City = "Bursa", Jobs = new PanelSawyer { WorkShopName = "Dostlar Ebatlama", AdressDescription = "İnegöl, Bursa" } },
                new AppUser { FullName = "Metin Akpınar", UserName = "metin.akpinar@panel.com", Email = "metin.akpinar@panel.com", UserCategory = Category.PanelSawyer, City = "Ankara", Jobs = new PanelSawyer { WorkShopName = "Sahne Panel", AdressDescription = "Ostim, Ankara" } },
                new AppUser { FullName = "Halil İbrahim", UserName = "halil.ibrahim@kesimhane.com", Email = "halil.ibrahim@kesimhane.com", UserCategory = Category.PanelSawyer, City = "Gaziantep", Jobs = new PanelSawyer { WorkShopName = "Bereket Kesim", AdressDescription = "Şehitkamil, Gaziantep" } },
                new AppUser { FullName = "Tarık Akan", UserName = "tarik.akan@panelist.com", Email = "tarik.akan@panelist.com", UserCategory = Category.PanelSawyer, City = "İstanbul", Jobs = new PanelSawyer { WorkShopName = "Yeşilçam Ebatlama", AdressDescription = "Modoko, İstanbul" } },
                new AppUser { FullName = "Cüneyt Arkın", UserName = "cuneyt.arkin@mdfci.com", Email = "cuneyt.arkin@mdfci.com", UserCategory = Category.PanelSawyer, City = "Eskişehir", Jobs = new PanelSawyer { WorkShopName = "Savaşçı Kesim Atölyesi", AdressDescription = "Odunpazarı, Eskişehir" } }
            };

            foreach (var user in sawyers) await CreateUserWithJob(user, "Kesim123!", "PanelSawyer");
        }

        private async Task CreateUserWithJob(AppUser user, string password, string role)
        {
            if (await _userManager.FindByEmailAsync(user.Email!) == null)
            {
                user.EmailConfirmed = true;

                // UserManager.CreateAsync hem AppUser'ı hem de bağlı olan Jobs nesnesini tek seferde kaydeder.
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    Console.WriteLine($"Hata: {user.Email} oluşturulamadı. {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}