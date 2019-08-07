using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MasterPerform.Contracts.Commands;
using MasterPerform.Contracts.Entities;
using MasterPerform.Tests.Documents.API;
using MasterPerform.Tests.Fixtures;
using Xunit;

namespace MasterPerform.Tests.Documents
{
    [Trait(CollectionName, CollectionDescription)]
    [Collection(MasterPerformCollectionFixture.DEFINITION_NAME)]
    public class DocumentTests
    {
        private const string CollectionName = "Documents";
        private const string CollectionDescription = "Documents tests.";

        private readonly MasterPerformFixture fixture;

        public DocumentTests(MasterPerformFixture fixture)
            => this.fixture = fixture;

        [Fact(DisplayName = "User can create new document.")]
        public async Task CreateDocument_SuccessfullyCreated()
        {
            // Arrange

            var command = GenerateCreateDocument();

            // Act

            await fixture.Client.CreateDocument(command);

            // Assert

            var document = await fixture.Client.GetDocument(command.CreatedId);

            document.Should().NotBeNull();
        }

        private CreateDocument GenerateCreateDocument()
        {
            return new CreateDocument(
                documentDetails: new DocumentDetails(
                    firstName: "John",
                    lastName: "Smith",
                    email: "john.smith@gmail.com",
                    phone: "12345467898"),
                addresses: new List<Address>
                {
                    new Address(addressLine: "Chorzowska 148", city: "Katowice", state: "Śląsk"),
                    new Address(addressLine: "Przewozowa 32", city: "Gliwice", state: "Śląsk")
                });
        }
    }
}
