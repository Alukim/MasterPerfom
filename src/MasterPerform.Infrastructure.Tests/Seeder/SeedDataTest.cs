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
using CsvHelper;
using Microsoft.AspNetCore.Mvc.Formatters;
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

        private ConcurrentBag<CreateDocument> Commands { get; }
        private ConcurrentBag<CreateDocument> DataToTest { get; }

        public SeedDataTest(MasterPerformFixture fixture)
        {
            this.fixture = fixture;
            Commands = new ConcurrentBag<CreateDocument>();
            DataToTest = new ConcurrentBag<CreateDocument>();
        }

        // Seed 1000000 documents, 10 Tasks - save document, 1 task to generate next random data
        [Fact(DisplayName = "Seed data", Skip = "Only for seed.")]
        public async Task SeedData()
        {
            var tasks = new Task[20];

            for (long i = 0; i < 10; ++i)
                tasks[i] = Seed();

            for (var i = 10; i < 20; ++i)
                tasks[i] = Task.Run(() => GenerateDataForTest());

            await Task.WhenAll(tasks);

            const int count = 300000;

            Commands.OrderBy(z => z.DocumentDetails?.Email)
                .Take(count)
                .AsParallel()
                .ForAll(DataToTest.Add);

            await SaveCommandsToJson(DataToTest);

            Assert.True(true);
        }

        /// Seed 100000 documents
        private async Task Seed()
        {
            for (long i = 0; i < 1000; ++i)
            {
                var faker = GetFaker();
                var documents = faker.GenerateBetween(100, 100);
                await SaveDocuments(documents);
            }
        }

        private void GenerateDataForTest()
        {
            for (var i = 0; i < 30000; i += 100)
            {
                var faker = GetFaker();
                var documents = faker.GenerateBetween(100, 100).Select(MapToCommand);
                documents.AsParallel().ForAll(DataToTest.Add);
            }
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

        private async Task SaveCommandsToJson(IReadOnlyCollection<CreateDocument> documents)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var testFolder = $"test-data-{DateTime.Now:yyyyMMdd_HHmmss}";

            var jsonDataFileNameTemplate = "test-data-{0}.txt";
            var documentsCount = documents.Count;
            var documentsToSerialize = documents.ToArray();
            var records = Enumerable.Range(1, documentsCount).ToList();

            using(var writer = new StreamWriter(Path.Combine(desktopPath, testFolder, "index.csv")))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(records);
            }

            var settings = JsonSerializerSettingsProvider.CreateSerializerSettings();

            for (var i = 1; i <= documentsCount; ++i)
            {
                var fileName = string.Format(jsonDataFileNameTemplate, i);
                using (var file = File.CreateText(Path.Combine(desktopPath, testFolder, fileName)))
                {
                    var doc = JsonConvert.SerializeObject(documentsToSerialize[i - 1], settings);
                    await file.WriteAsync(doc);
                }
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
