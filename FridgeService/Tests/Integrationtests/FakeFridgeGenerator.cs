using Bogus;
using DAL.Entities;

namespace Tests.IntegrationTests
{

    public class FakeFridgeGenerator
    {
        public readonly List<Fridge> Fridges = new();

        private readonly int _amountOffridges = 2;
        private int _counter = 0;

        public void InitializeData()
        {
            var fridgeGenerator = GetFridgeGenerator();
            var fakefridges = fridgeGenerator.Generate(_amountOffridges);
            Fridges.AddRange(fakefridges);
        }

        private Faker<Fridge> GetFridgeGenerator()
        {
            return new Faker<Fridge>()
                .RuleFor(fridge => fridge.id, _ => ++_counter)
                .RuleFor(fridge => fridge.name, faker => faker.Internet.UserName())
                .RuleFor(fridge => fridge.model, faker => faker.Internet.UserName())
                .RuleFor(fridge => fridge.serial, faker => faker.Internet.UserName())
                .RuleFor(fridge => fridge.boughtDate, faker => DateTime.UtcNow)
                .RuleFor(fridge => fridge.boxNumber, faker => faker.Random.Number(1, 10));
        }
    }
}
