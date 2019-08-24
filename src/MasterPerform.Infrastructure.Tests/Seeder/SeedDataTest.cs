using Bogus;
using Bogus.Extensions;
using MasterPerform.Contracts.Commands;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Repositories;
using MasterPerform.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ContractAddress = MasterPerform.Contracts.Entities.Address;
using Document = MasterPerform.Entities.Document;

namespace MasterPerform.Tests.Seeder
{
    [Collection(MasterPerformCollectionFixture.DEFINITION_NAME)]
    public class SeedDataTest
    {
        private readonly MasterPerformFixture fixture;
        private static readonly Random rnd = new Random();

        private const int DataCounts = 100;
        private const int DataPackage = 100;

        private ConcurrentBag<CreateDocument> Commands { get; }

        public SeedDataTest(MasterPerformFixture fixture)
        {
            this.fixture = fixture;
            Commands = new ConcurrentBag<CreateDocument>();
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

            const int count = (DataCounts * DataPackage) / 4;

            var list = Commands.OrderBy(z => z.DocumentDetails?.Email)
                .Take(count)
                .ToList();

            for (var i = 0; i < count; i += 10)
            {
                var faker = GetFaker();
                var documents = faker.Generate(10);
                foreach (var document in documents)
                    list.Add(MapToCommand(document));
            }

            SaveCommandsToJson(list);

            Assert.True(true);
        }

        private async Task SaveDocuments(IReadOnlyCollection<Document> documents)
        {
            using (var scope = fixture.ServiceProvider.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<IEntityRepository<Document>>();
                var tasks = documents.Select(async document =>
                    await Task.Run(async () =>
                    {
                        await repository.AddAsync(document);
                        Commands.Add(MapToCommand(document));
                    }));
                await Task.WhenAll(tasks);
            }
        }

        private void SaveCommandsToJson(IReadOnlyCollection<CreateDocument> documents)
        {
            using (var file = File.CreateText("./test-data.json"))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, documents);
            }
        }

        private CreateDocument MapToCommand(Document document)
        {
            return new CreateDocument(
                documentDetails: new Contracts.Entities.DocumentDetails(
                    firstName: document.Details.FirstName,
                    lastName: document.Details.LastName,
                    email: document.Details.Email,
                    phone: document.Details.Phone),
                addresses: document.Addresses?.Select(z => new ContractAddress(
                    addressLine: z?.AddressLine,
                    city: z?.City,
                    state: z?.State))?.ToList(),
                findSimilar: true
            );
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
