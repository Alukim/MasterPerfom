using Bogus;
using Bogus.Extensions;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Document = MasterPerform.Entities.Document;

namespace MasterPerform.Tests.Seeder
{
    [Collection(MasterPerformCollectionFixture.DEFINITION_NAME)]
    public class SeedDataTest
    {
        private readonly MasterPerformFixture fixture;
        private static readonly Random rnd = new Random();

        private const long DataCounts = 500;
        private const int DataPackage = 1000;

        public SeedDataTest(MasterPerformFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact(DisplayName = "Seed data", Skip = "Only for seed.")]
        public async Task SeedData()
        {
            for (long i = 0; i < DataCounts; ++i)
            {
                var faker = GetFaker();
                var documents = faker.GenerateBetween(DataPackage, DataPackage);
                await SaveDocuments(documents);
            }

            Assert.True(true);
        }

        private async Task SaveDocuments(IReadOnlyCollection<Document> documents)
        {
            using (var scope = fixture.ServiceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IEntityRepository<Document>>();
                var tasks = documents.Select(async document =>
                    await Task.Run(async () => await repository.AddAsync(document)));
                await Task.WhenAll(tasks);
            }
        }

        private Faker<Document> GetFaker() 
        {
            var addressCount = rnd.Next(0, 3);

            return new Faker<Document>()
                .RuleFor(z => z.Id, r => r.Random.Guid())
                .RuleFor(z => z.Details, r => GetDocumentDetailsFaker.Generate())
                .RuleFor(z => z.Addresses, r => GetAddressFaker.Generate(addressCount).ToList())
                .RuleFor(z => z.SimilarDocument, r => null);
        }


        private static Faker<DocumentDetails> GetDocumentDetailsFaker
            => new Faker<DocumentDetails>()
                .RuleFor(z => z.FirstName, r => r.Person.FirstName)
                .RuleFor(z => z.LastName, r => r.Person.LastName)
                .RuleFor(z => z.Email, r => r.Person.Email)
                .RuleFor(z => z.Phone, r => r.Person.Phone);

        private static Faker<Address> GetAddressFaker
            => new Faker<Address>()
                .RuleFor(z => z.City, r => r.Address.City())
                .RuleFor(z => z.State, r => r.Address.State())
                .RuleFor(z => z.AddressLine, r => r.Address.StreetAddress());
    }
}
