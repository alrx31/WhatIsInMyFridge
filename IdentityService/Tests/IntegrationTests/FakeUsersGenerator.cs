using Bogus;
using Domain.Entities;
using Infastructure.Persistanse;

namespace Tests.IntegrationTests
{

    public class FakeUsersGenerator
    {
        public readonly List<User> Users = new();

        private readonly int _amountOfUsers = 2;
        private int _counter = 0;

        public void InitializeData()
        {
            var userGenerator = GetUserGenerator();
            var fakeUsers = userGenerator.Generate(_amountOfUsers);
            Users.AddRange(fakeUsers);
        }

        private Faker<User> GetUserGenerator()
        {
            return new Faker<User>()
                .RuleFor(user => user.id, _ => ++_counter)
                .RuleFor(user => user.email, faker => faker.Internet.Email())
                .RuleFor(user => user.login, faker => faker.Internet.UserName())
                .RuleFor(user => user.name, faker => faker.Name.FirstName())
                .RuleFor(user => user.password, faker => Scripts.GetHash(faker.Internet.Password())
                );
        }
    }
}
