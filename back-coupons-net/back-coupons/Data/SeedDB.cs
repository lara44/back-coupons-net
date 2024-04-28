using back_coupons.Entities;

namespace back_coupons.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await CheckCategoriesAsync();
            await CheckCompaniesAsync();
            await CheckContactsAsync();
            await CheckCountriesAsync();
            await CheckProductsAsync();
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
                _context.Companies.Add(new Company { 
                    Nit = "123456",
                    Name = "NeoCode",
                    Address = "Cl 22b",
                    Phone = "1234567890",
                    Email = "neocode.gmail.com"
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
                    Barcode = "1030"
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Alitas x 3",
                    Price = 30000,
                    Barcode = "1031"
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Alitas x 4",
                    Price = 40000,
                    Barcode = "1032"
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar S",
                    Price = 60000,
                    Barcode = "1033"
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar M",
                    Price = 70000,
                    Barcode = "1034"
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar L",
                    Price = 80000,
                    Barcode = "1035"
                });
                _context.Products.Add(new Product
                {
                    Name = "Combo Mega Familiar XL",
                    Price = 90000,
                    Barcode = "1036"
                });
                _context.Products.Add(new Product
                {
                    Name = "Sundae Arequipe",
                    Price = 5500,
                    Barcode = "1037"
                });
                _context.Products.Add(new Product
                {
                    Name = "Sundae Vainilla",
                    Price = 5500,
                    Barcode = "1038"
                });
                _context.Products.Add(new Product
                {
                    Name = "Sundae Chocolate",
                    Price = 5500,
                    Barcode = "1039"
                });
                _context.Products.Add(new Product
                {
                    Name = "Agua Cristal",
                    Price = 4000,
                    Barcode = "1040"
                });
                _context.Products.Add(new Product
                {
                    Name = "Mr Tea",
                    Price = 3000,
                    Barcode = "1041"
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
