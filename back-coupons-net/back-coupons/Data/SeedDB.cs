using back_coupons.Entities;
using back_coupons.Enums;
using back_coupons.UnitsOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;

namespace back_coupons.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserUnitOfWork _userUnitOfWork;

        public SeedDB(DataContext context, IUserUnitOfWork userUnitOfWork)
        {
            _context = context;
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckCategoriesAsync();
            await CheckCompaniesAsync();
            await CheckContactsAsync();
            await CheckCountriesAsync();
            await CheckProductsAsync();
            await CheckRolesAsync();
            await CheckCouponsAsync();
            await CheckUserAsync("1010", "Luis Alberto", "Rojas Adames", "adames.lancero@gmail.com", "3214451040", "Cll 24", UserType.Admin, 1);
            await CheckUserAsync("1010", "Carlos Andres", "Cuellar Roso", "carlos.andres@gmail.com", "3214451040", "Cll 25", UserType.User, 1);
            await CheckUserAsync("1010", "Daniel", "Espinoza", "daniel.espinoza@gmail.com", "3214451040", "Cll 26", UserType.User, 2);
        }

        private async Task CheckRolesAsync()
        {
            await _userUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _userUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType, int companyId)
        {
            var user = await _userUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                    CompanyId = companyId
                };

                await _userUnitOfWork.AddUserAsync(user, "123456");
                await _userUnitOfWork.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
                await _userUnitOfWork.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Cosmeticos" });
                _context.Categories.Add(new Category { Name = "Ferreteria" });
                _context.Categories.Add(new Category { Name = "Gamer" });
                _context.Categories.Add(new Category { Name = "Hogar" });
                _context.Categories.Add(new Category { Name = "Jardín" });
                _context.Categories.Add(new Category { Name = "Jugetes" });
                _context.Categories.Add(new Category { Name = "Lenceria" });
                _context.Categories.Add(new Category { Name = "Mascotas" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Tecnología" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCompaniesAsync()
        {
            if (!_context.Companies.Any())
            {
                _context.Companies.Add(new Company
                {
                    Nit = "500001",
                    Name = "NeoCode",
                    Address = "Cl 22b",
                    Phone = "1234567890",
                    Email = "neocode.gmail.com"
                });

                _context.Companies.Add(new Company
                {
                    Nit = "500002",
                    Name = "La Granja",
                    Address = "Cl 23b",
                    Phone = "1234567890",
                    Email = "lagranja.gmail.com"
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckContactsAsync()
        {
            if (!_context.Contacts.Any())
            {
                _context.Contacts.Add(new Contact
                {
                    Name = "Carlos Andres Cuellar",
                    Phone = "123456789",
                    Address = "Call 1",
                    Email = "carlos.cuella@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Mauricio Andres Peréz",
                    Phone = "123456789",
                    Address = "Call 2",
                    Email = "mauricio.andres@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Andres Moreno Collazos",
                    Phone = "123456789",
                    Address = "Call 3",
                    Email = "andres.moreno@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Luis Daniel Espinoza",
                    Phone = "123456789",
                    Address = "Call 4",
                    Email = "luis.daniel@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Karen Escobar",
                    Phone = "123456789",
                    Address = "Call 5",
                    Email = "karen.escobar",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Cristian Castillo",
                    Phone = "123456789",
                    Address = "Call 6",
                    Email = "cristian.castillo@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Ana Catalina Barbosa",
                    Phone = "123456789",
                    Address = "Call 7",
                    Email = "ana.catalina@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Mariana Pereira",
                    Phone = "123456789",
                    Address = "Call 8",
                    Email = "mariana.pereira@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Carlos Andres Parra",
                    Phone = "123456789",
                    Address = "Call 9",
                    Email = "carlos.parra@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Brayan Medina",
                    Phone = "123456789",
                    Address = "Call 10",
                    Email = "brayan.medina@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Alfredo Rojas Cuevas",
                    Phone = "123456789",
                    Address = "Call 11",
                    Email = "alfredo.rojas@gmail.com",
                    CompanyId = 1
                });
                _context.Contacts.Add(new Contact
                {
                    Name = "Jhon Freddy Burgos",
                    Phone = "123456789",
                    Address = "Call 12",
                    Email = "jhon.burgos@gmail.com",
                    CompanyId = 1
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _ = _context.Countries.Add(new Entities.Country
                {
                    Name = "Colombia",
                    States =
                    [
                        new Entities.State()
                        {
                            Name = "Antioquia",
                            Cities = [
                                new() { Name = "Medellín" },
                                new() { Name = "Bello" },
                                new() { Name = "Itagüí" },
                                new() { Name = "Envigado" },
                                new() { Name = "Apartado" },
                                new() { Name = "Turbo" },
                                new() { Name = "Rionegro" },
                                new() { Name = "Sabaneta" },
                                new() { Name = "Caucasia" },
                                new() { Name = "Caldas" },
                                new() { Name = "Copacabana" },
                                new() { Name = "Chigorodó" }
                            ]
                        },
                        new Entities.State()
                        {
                            Name = "Caquetá",
                            Cities = [
                                new() { Name = "Puerto Rico" },
                                new() { Name = "Doncello" },
                                new() { Name = "San Vicente" },
                                new() { Name = "Paujil" },
                                new() { Name = "Montañita" },
                                new() { Name = "Florencia" },
                                new() { Name = "Belén de los Andaquies" },
                                new() { Name = "Milan" },
                                new() { Name = "Curillo" },
                                new() { Name = "Cartagena del Chaira" },
                                new() { Name = "Solano" },
                                new() { Name = "Valparaiso" }
                            ]
                        },
                        new Entities.State()
                        {
                            Name = "Huila",
                            Cities = [
                                new() { Name = "Neiva" },
                                new() { Name = "Campo alegre" },
                                new() { Name = "Hobo" },
                                new() { Name = "Gigante" },
                                new() { Name = "Garzón" },
                                new() { Name = "Altamira" },
                                new() { Name = "Pitalito" },
                                new() { Name = "San Augustin" },
                                new() { Name = "Yaguará" },
                                new() { Name = "Tello" },
                                new() { Name = "Palermo" },
                                new() { Name = "Baraya" },
                            ]
                        },
                    ]
                });
                _context.Countries.Add(new Entities.Country
                {
                    Name = "Estados Unidos",
                    States =
                    [
                        new Entities.State()
                        {
                            Name = "Florida",
                            Cities = [
                                new() { Name = "Orlando" },
                                new() { Name = "Miami" },
                                new() { Name = "Tampa" },
                                new() { Name = "Fort Lauderdale" },
                                new() { Name = "Key West" },
                            ]
                        },
                        new Entities.State()
                        {
                            Name = "Texas",
                            Cities = [
                                new() { Name = "Houston" },
                                new() { Name = "San Antonio" },
                                new() { Name = "Dallas" },
                                new() { Name = "Austin" },
                                new() { Name = "El Paso" },
                            ]
                        },
                    ]
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                _context.Products.Add(new Product
                {
                    Name = "Combo Alitas x 2",
                    Price = 30000,
                    Barcode = "1030",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Alitas x 3",
                    Price = 30000,
                    Barcode = "1031",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Alitas x 4",
                    Price = 40000,
                    Barcode = "1032",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar S",
                    Price = 60000,
                    Barcode = "1033",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar M",
                    Price = 70000,
                    Barcode = "1034",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar L",
                    Price = 80000,
                    Barcode = "1035",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar XL",
                    Price = 90000,
                    Barcode = "1036",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Sundae Arequipe",
                    Price = 5500,
                    Barcode = "1037",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Sundae Vainilla",
                    Price = 5500,
                    Barcode = "1038",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Sundae Chocolate",
                    Price = 5500,
                    Barcode = "1039",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Agua Cristal",
                    Price = 4000,
                    Barcode = "1040",
                    Company = _context.Companies.FirstOrDefault()
                });
                _context.Products.Add(new Product
                {
                    Name = "Mr Tea",
                    Price = 3000,
                    Barcode = "1041",
                    Company = _context.Companies.FirstOrDefault()
                });
                await _context.SaveChangesAsync();
            }
        }
        private async Task CheckCouponsAsync()
        {
            if (!_context.Coupons.Any())
            {
                await AddProductAsync(
                   "CUPON MEMBRESIA 31",
                   new DateTime(2024, 06, 01),
                   new DateTime(2024, 06, 30),
                   100,
                   100,
                   1,
                   "2031",
                   "https://creatuaplicacion.com/wp-content/uploads/2019/02/crear-cupones.png",
                   new List<string>() { "Sundae Arequipe" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 32",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2032",
                    "https://tecnosoluciones.com/wp-content/uploads/2023/03/cupones-de-descuento-en-el-comercio-electronico.png",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 33",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2033",
                    "https://www.actualidadecommerce.com/wp-content/uploads/2016/10/cupon-descuento-830x536.jpg",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 34",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2034",
                    "https://connectif.ai/wp-content/uploads/2021/12/Imagen-destacada-cupon-1-1.png",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 35",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2035",
                    "https://img.freepik.com/vector-premium/conjunto-ilustracion-centro-comercial-descuento-mitad-precio-brillante-cupon-moneda-oro-como-regalo_632180-345.jpg",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 36",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2036",
                    "https://static.vecteezy.com/system/resources/thumbnails/000/271/115/small_2x/Coupon_Vector.jpg",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 37",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2037",
                    "https://img.freepik.com/vector-premium/cupon-descuento-90-ciento-descuento-vale-regalo-marcas-porcentaje-codigo-qr-codigos-promocion_662353-962.jpg",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 38",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2038",
                    "https://img.freepik.com/vector-premium/concepto-plantilla-moderna-cupon-o-vale_23-2147942782.jpg",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 39",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2039",
                    "https://png.pngtree.com/png-vector/20211007/ourlarge/pngtree-vector-illustration-of-discount-coupon-save-10-png-image_3973391.png",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await AddProductAsync(
                    "CUPON MEMBRESIA 40",
                    new DateTime(2024, 06, 01),
                    new DateTime(2024, 06, 15),
                    50,
                    50,
                    1,
                    "2040",
                    "https://img.freepik.com/vector-premium/conjunto-cupon-descuento-codigo-promocion-cupon-descuento-cupon-devolucion-efectivo_529344-1935.jpg",
                    new List<string>() { "Combo Alitas x 2", "Mr Tea" }
                );

                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, DateTime startDate, DateTime expiryDate, int quantityInitial, int quantityActual, int companyId, string couponCode, string photo, List<string> products)
        {
            Coupon coupon = new()
            {
                Name = name,
                StartDate = startDate,
                ExpiryDate = expiryDate,
                QuantityInitial = quantityInitial,
                QuantityActual = quantityActual,
                CompanyId = companyId,
                CouponCode = couponCode,
                Photo = photo,
                DetailCoupons = new List<DetailCoupon>(),
            };

            foreach (var productName in products)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Name == productName);
                if (product != null)
                {
                    coupon.DetailCoupons.Add(new DetailCoupon { Product = product });
                }
            }

            _context.Coupons.Add(coupon);
        }
    }
}
