using AutoMapper.Configuration.Annotations;
using Bogus;
using Bogus.DataSets;
using DAL.Entities;
using DAL.Persistanse;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Repositories
{
    public class FridgeRepositoryTests
    {
        private ApplicationDbContext _context;

        private FridgeRepository _repository;

        public FridgeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new FridgeRepository(_context);
        }

        [Fact]
        public async Task AddFridge_Success_ShouldAddFridge()
        {
            var faker = new Faker();

            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            // Act
            
            await _repository.AddFridge(fridge);

            await _context.SaveChangesAsync();

            // Assert

            var result = await _context.fridges.FirstOrDefaultAsync(f=> f.serial == fridge.serial);

            result.Should().NotBeNull();
            result.name.Should().Be(fridge.name);
            result.serial.Should().Be(fridge.serial);
            result.boxNumber.Should().Be(fridge.boxNumber);
            result.model.Should().Be(fridge.model);
        }

        [Fact]
        public async Task GetFridge_Success_ShouldReturnFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            await _context.fridges.AddAsync(fridge);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetFridge(fridge.id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fridge);
        }

        [Fact]
        public async Task GetFridgeByUserId_Success_ShouldReturnFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            
            var fridges = new List<Fridge> { fridge };

            var user = new User
            {
                id = faker.Random.Number(1, 1000),
                name = faker.Random.String(10),
                login = faker.Person.FirstName,
                email = faker.Internet.Email(),
                password = faker.Random.String(10),
                isAdmin = false,
            };

            var model = new UserFridge
            {
                id = faker.Random.Number(1, 100),
                userId = user.id,
                fridgeId = fridge.id,
                fridge=  fridge,
                LinkTime = DateTime.UtcNow,
            };

            await _context.fridges.AddAsync(fridge);

            await _context.userFridges.AddAsync(model);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetFridgeByUserId(user.id);

            // Assert

            result.Should().BeEquivalentTo(fridges);
        }

        [Fact]
        public async Task RemoveFridge_Success_ShouldRemoveFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            await _context.fridges.AddAsync(fridge);

            await _context.SaveChangesAsync();

            // Act

            await _repository.RemoveFridge(fridge.id);

            await _context.SaveChangesAsync();

            // Asser

            var res = await _context.fridges.FirstOrDefaultAsync(f => f.id == fridge.id);
            
            res.Should().BeNull();
        }

        [Fact]
        public async Task AddUserToFridge_Success_ShouldAddUserToFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            var user = new User
            {
                id = faker.Random.Number(1, 1000),
                name = faker.Random.String(10),
                login = faker.Person.FirstName,
                email = faker.Internet.Email(),
                password = faker.Random.String(10),
                isAdmin = false,
            };
            
            await _context.fridges.AddAsync(fridge);

            await _context.SaveChangesAsync();

            var model = new UserFridge
            {
                id = faker.Random.Number(1, 100),
                userId = user.id,
                fridgeId = fridge.id,
                fridge = fridge,
                LinkTime = DateTime.UtcNow,
            };

            // Act

            await _repository.AddUserToFridge(model);

            await _context.SaveChangesAsync();

            // Assert

            var result = await _context.userFridges.FirstOrDefaultAsync(m => m.fridgeId == fridge.id);

            result.Should().NotBeNull();
            result.userId.Should().Be(user.id);
        }

        [Fact]
        public async Task RemoveUserFromFridge_Success_ShouldRemoveUser()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            var user = new User
            {
                id = faker.Random.Number(1, 1000),
                name = faker.Random.String(10),
                login = faker.Person.FirstName,
                email = faker.Internet.Email(),
                password = faker.Random.String(10),
                isAdmin = false,
            };

            var model = new UserFridge
            {
                id = faker.Random.Number(1, 100),
                userId = user.id,
                fridgeId = fridge.id,
                fridge = fridge,
                LinkTime = DateTime.UtcNow,
            };

            await _context.fridges.AddAsync(fridge);
        
            await _context.userFridges.AddAsync(model);

            await _context.SaveChangesAsync();

            // Act

            await _repository.RemoveUserFromFridge(fridge.id, user.id);

            // Assert

            var result = await _context.userFridges.FirstOrDefaultAsync(user => user.id == fridge.id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUsersFromFridge_Success_ShouldReturnUsers()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            var model = new UserFridge
            {
                id = faker.Random.Number(1, 100),
                userId = faker.Random.Number(1,100),
                fridgeId = fridge.id,
                fridge = fridge,
                LinkTime = DateTime.UtcNow,
            };

            var users = new List<int>
            {
                model.userId
            };

            await _context.fridges.AddAsync(fridge);

            await _context.userFridges.AddAsync(model);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetUsersFromFridge(fridge.id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task UpdateFridge_Success_ShouldUpdateFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            var updtFridge = new Fridge
            {
                id = fridge.id,
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            await _context.fridges.AddAsync(fridge);

            await _context.SaveChangesAsync();

            // Act

            fridge.name = updtFridge.name;
            fridge.model = updtFridge.model;
            fridge.serial = updtFridge.serial;
            fridge.boughtDate = updtFridge.boughtDate;
            fridge.boxNumber = updtFridge.boxNumber;

            var result = await _repository.UpdateFridge(fridge);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(updtFridge);
        }

        [Fact]
        public async Task AddProductToFridge_Success_ShouldAddProduct()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            var model = new ProductFridgeModel
            {
                id = faker.Random.Number(1, 100),
                productId = faker.Random.Number(0, 100).ToString(),
                count = faker.Random.Number(1, 10),
                fridgeId = fridge.id,
                Fridge = fridge,
                addTime = DateTime.UtcNow,
            };

            // Act

            await _repository.AddProductToFridge(model);

            await _context.SaveChangesAsync();

            // Assert

            var result = await  _context.productFridgeModels.FirstOrDefaultAsync(m => m.fridgeId == fridge.id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(model);
        }

        [Fact]
        public async Task AddProductsToFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };
            var model = new List<ProductFridgeModel>
            {
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100).ToString(),
                    count = faker.Random.Number(1, 10),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow,
                },
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100).ToString(),
                    count = faker.Random.Number(1, 10),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow,
                }
            };

            // Act

            await _repository.AddProductsToFridge(model);

            await _context.SaveChangesAsync();

            // Assert

            var result = _context.productFridgeModels.Where(m => m.fridgeId == fridge.id);
            
            result.Should().BeEquivalentTo(model);
        }

        [Fact]
        public async Task RemoveProductFromFridge_Success_ShouldRemoveProduct()
        {
            var faker = new Faker();

            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            var model = new ProductFridgeModel
            {
                id = faker.Random.Number(1, 100),
                productId = faker.Random.Number(0, 100).ToString(),
                count = faker.Random.Number(1, 10),
                fridgeId = fridge.id,
                Fridge = fridge,
                addTime = DateTime.UtcNow,
            };

            await _context.fridges.AddAsync(fridge);

            await _context.productFridgeModels.AddAsync(model);

            await _context.SaveChangesAsync();

            // Act

            await _repository.RemoveProductFromFridge(fridge.id, model.productId);

            // Assert

            var result = await _context.productFridgeModels.ToListAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProductFromFridge_Success_ShouldReturnProducts()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            var models = new List<ProductFridgeModel>
            {
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100).ToString(),
                    count = faker.Random.Number(1, 10),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow,
                },
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100).ToString(),
                    count = faker.Random.Number(1, 10),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow,
                }
            };

            await _context.fridges.AddAsync(fridge);

            await _context.productFridgeModels.AddRangeAsync(models);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetProductFromFridge(fridge.id, models[0].productId);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(models[0]);
        }

        [Fact]
        public async Task GetProductsFromFridge_Success_ShouldReturnProducts()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            var models = new List<ProductFridgeModel>
            {
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100).ToString(),
                    count = faker.Random.Number(1, 10),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow,
                },
                new ProductFridgeModel
                {
                    id = faker.Random.Number(1, 100),
                    productId = faker.Random.Number(0, 100).ToString(),
                    count = faker.Random.Number(1, 10),
                    fridgeId = fridge.id,
                    Fridge = fridge,
                    addTime = DateTime.UtcNow,
                }
            };

            await _context.fridges.AddAsync(fridge);

            await _context.productFridgeModels.AddRangeAsync(models);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetProductsFromFridge(fridge.id);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(models);
        }

        [Fact]
        public async Task GetAllFridges_Success_ShouldReturnFridges()
        {
            var faker = new Faker();
            var fridges = new List<Fridge>
            {
                new Fridge
                {
                    id = faker.Random.Number(1, 100),
                    name = faker.Person.FirstName,
                    model = faker.Random.Number(0, 1000000000).ToString(),
                    serial = faker.Random.Number(0, 100000000).ToString(),
                    boughtDate = DateTime.UtcNow,
                    boxNumber = faker.Random.Number(0, 10)
                },
                new Fridge
                {
                    id = faker.Random.Number(1, 100),
                    name = faker.Person.FirstName,
                    model = faker.Random.Number(0, 1000000000).ToString(),
                    serial = faker.Random.Number(0, 100000000).ToString(),
                    boughtDate = DateTime.UtcNow,
                    boxNumber = faker.Random.Number(0, 10)
                }
            };

            await _context.fridges.AddRangeAsync(fridges);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetAllFridges();

            // Assert

            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(fridges);
        }

        [Fact]
        public async Task GetFridgeBySerialAndBoxNumber_Success_ShouldReturnFridge()
        {
            var faker = new Faker();
            var fridge = new Fridge
            {
                id = faker.Random.Number(1, 100),
                name = faker.Person.FirstName,
                model = faker.Random.Number(0, 1000000000).ToString(),
                serial = faker.Random.Number(0, 100000000).ToString(),
                boughtDate = DateTime.UtcNow,
                boxNumber = faker.Random.Number(0, 10)
            };

            await _context.fridges.AddAsync(fridge);

            await _context.SaveChangesAsync();

            // Act

            var result = await _repository.GetFridgeBySerialAndBoxNumber(fridge.serial, fridge.boxNumber);

            // Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(fridge);
        }

        [Fact]
        public async Task updateProductInFridge_Success_ShouldUpdateProduct()
        {
            var faker = new Faker();
            var model = new ProductFridgeModel
            {
                id = faker.Random.Number(1, 100),
                productId = faker.Random.Number(1, 100).ToString(),
                count = faker.Random.Number(1, 10),
                fridgeId = faker.Random.Number(1, 10),
                addTime = DateTime.UtcNow.AddDays(1),
            };


            await _context.AddAsync(model);

            await _context.SaveChangesAsync();

            model.count = faker.Random.Number(1, 1000);

            // Act

            await _repository.UpdateProductInFridge(model);

            await _context.SaveChangesAsync();

            // Assert

            var result = await _context.productFridgeModels.FirstOrDefaultAsync(m => m.id == model.id);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(model);
        }
    }
}
