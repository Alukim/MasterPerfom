using FluentAssertions;
using MasterPerform.Entities;
using MasterPerform.Infrastructure.Exceptions;
using MasterPerform.Infrastructure.Tests;
using MasterPerform.Tests.Documents.API;
using MasterPerform.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Address = MasterPerform.Contracts.Entities.Address;
using DocumentDetails = MasterPerform.Contracts.Entities.DocumentDetails;

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

            var command = fixture.DocumentFactory.GenerateSimpleCreateDocument();

            // Act

            await fixture.Client.CreateDocument(command);

            // Assert

            var document = await fixture.Client.GetDocument(command.CreatedId);

            document.Should().NotBeNull();
            document.DocumentDetails.Should().BeEquivalentTo(command.DocumentDetails);
            document.Addresses.Should().BeEquivalentTo(command.Addresses);
        }
         
        [Fact(DisplayName = "User can update document details.")]
        public async Task UpdateDocumentDetails_SuccessfullyUpdated()
        {
            // Arrange

            var command = fixture.DocumentFactory.GenerateSimpleCreateDocument();
            await fixture.Client.CreateDocument(command);

            var updateCommand = fixture.DocumentFactory.GenerateUpdateDocumentDetails(command.CreatedId,
                new DocumentDetails(
                    firstName: "Jan",
                    lastName: "Nowak",
                    email: "jan.nowak@gmail.com",
                    phone: null));

            // Act

            await fixture.Client.UpdateDocumentDetails(updateCommand);

            // Assert

            var document = await fixture.Client.GetDocument(updateCommand.DocumentId);

            document.Should().NotBeNull();
            document.DocumentDetails.Should().BeEquivalentTo(updateCommand.Details);
            document.Addresses.Should().BeEquivalentTo(command.Addresses);
        }

        [Fact(DisplayName = "User can't update not existing document details.")]
        public async Task UpdateDocumentDetails_ThrowException_EntityNotFound()
        {
            // Arrange

            var notExistingDocumentId = Guid.NewGuid();    

            var updateCommand = fixture.DocumentFactory.GenerateUpdateDocumentDetails(notExistingDocumentId,
                new DocumentDetails(
                    firstName: "Jan",
                    lastName: "Nowak",
                    email: "jan.nowak@gmail.com",
                    phone: null));

            // Act, Assert

            var exception = await Assert.ThrowsAsync<TestRequestFailed>(async () => await fixture.Client.UpdateDocumentDetails(updateCommand));
            exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            exception.Report.Details.First().Code.Should().Be(typeof(EntityNotFound).Name);
        }

        [Fact(DisplayName = "User can update document addresses.")]
        public async Task UpdateDocumentAddresses_SuccessfullyUpdated()
        {
            // Arrange

            var command = fixture.DocumentFactory.GenerateSimpleCreateDocument();
            await fixture.Client.CreateDocument(command);

            var updateCommand = fixture.DocumentFactory.GenerateUpdateDocumentAddresses(command.CreatedId,
                new List<Address>
                {
                    new Address(addressLine: "New Address 14", city: "New city", state: "New state")
                });

            // Act

            await fixture.Client.UpdateDocumentAddresses(updateCommand);

            // Assert

            var document = await fixture.Client.GetDocument(updateCommand.DocumentId);

            document.Should().NotBeNull();
            document.DocumentDetails.Should().BeEquivalentTo(command.DocumentDetails);
            document.Addresses.Should().BeEquivalentTo(updateCommand.Addresses);
        }

        [Fact(DisplayName = "User can't update not existing document addresses.")]
        public async Task UpdateDocumentAddresses_ThrowException_EntityNotFound()
        {
            // Arrange

            var notExistingDocumentId = Guid.NewGuid();

            var updateCommand = fixture.DocumentFactory.GenerateUpdateDocumentAddresses(notExistingDocumentId,
                new List<Address>
                {
                    new Address(addressLine: "New Address 14", city: "New city", state: "New state")
                });

            // Act, Assert

            var exception = await Assert.ThrowsAsync<TestRequestFailed>(async () => await fixture.Client.UpdateDocumentAddresses(updateCommand));
            exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            exception.Report.Details.First().Code.Should().Be(typeof(EntityNotFound).Name);
        }

        [Fact(DisplayName = "User can't get not existing document.")]
        public async Task GetDocument_ThrowException_EntityNotFound()
        {
            // Arrange

            var notExistingDocumentId = Guid.NewGuid();

            // Act, Assert

            var exception = await Assert.ThrowsAsync<TestRequestFailed>(async () => await fixture.Client.GetDocument(notExistingDocumentId));
            exception.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            exception.Report.Details.First().Code.Should().Be(typeof(EntityNotFound).Name);
        }

        [Fact(DisplayName = "User can get list of documents.")]
        public async Task GetDocuments_SuccessfullyGet()
        {
            // Arrange

            var command = fixture.DocumentFactory.GenerateSimpleCreateDocument();
            await fixture.Client.CreateDocument(command);

            var command2 = fixture.DocumentFactory.GenerateCreateDocument(new DocumentDetails(
                firstName: "Bartosz",
                lastName: "Kowalski",
                email: "b.k@gmail.com",
                phone: "14785236998"));
            await fixture.Client.CreateDocument(command2);

            var command3 = fixture.DocumentFactory.GenerateCreateDocument(new DocumentDetails(
                firstName: "Jan",
                lastName: "Nowak",
                email: "j.nowak@o2.com",
                phone: "5478632"));
            await fixture.Client.CreateDocument(command3);

            fixture.RefreshIndexOfType<Document>();

            // Act

            var collection = await fixture.Client.GetDocuments(pageSize: 50, pageNumber: 1);

            // Assert

            collection.Should().Contain(z => z.DocumentId == command.CreatedId);
            collection.Should().Contain(z => z.DocumentId == command2.CreatedId);
            collection.Should().Contain(z => z.DocumentId == command3.CreatedId);
        }

        [Fact(DisplayName = "User can FTS to get list of documents.")]
        public async Task GetDocuments_FTS_SuccessfullyGet()
        {
            // Arrange

            var command = fixture.DocumentFactory.GenerateSimpleCreateDocument();
            await fixture.Client.CreateDocument(command);

            var command2 = fixture.DocumentFactory.GenerateCreateDocument(new DocumentDetails(
                firstName: "Grzegorz",
                lastName: "Hellmans",
                email: "g.hellmans@email.net",
                phone: "555888222"));
            await fixture.Client.CreateDocument(command2);

            fixture.RefreshIndexOfType<Document>();

            // Act

            var collectionExactMatch = await fixture.Client.GetDocuments(query: "Grzegorz", pageSize: 50, pageNumber: 1);
            var collectionContains = await fixture.Client.GetDocuments(query: "llman", pageSize: 50, pageNumber: 1);
            var collectionStartsWith = await fixture.Client.GetDocuments(query: "g.hellmans", pageSize: 50, pageNumber: 1);

            // Assert

            collectionExactMatch.Should().HaveCount(1).And.ContainSingle(z => z.DocumentId == command2.CreatedId);
            collectionContains.Should().HaveCount(1).And.ContainSingle(z => z.DocumentId == command2.CreatedId);
            collectionStartsWith.Should().HaveCount(1).And.ContainSingle(z => z.DocumentId == command2.CreatedId);
        }

        [Fact(DisplayName = "User can create new document and find similar document.")]
        public async Task CreateDocument_SuccessfullyCreated_SimilarDocumentFounded()
        {
            // Arrange

            var documentDetails = new DocumentDetails(
                firstName: "Founded",
                lastName: "Similar",
                email: "Document@outlook.com",
                phone: "# 48 [123 456 789]");

            var command = fixture.DocumentFactory.GenerateCreateDocument(
                details: documentDetails,
                addresses: null,
                findSimilar: false);
            await fixture.Client.CreateDocument(command);

            var command2 = fixture.DocumentFactory.GenerateCreateDocument(
                details: new DocumentDetails(
                    firstName: "Found",
                    lastName: "Similar",
                    email: "Document@out",
                    phone: "48123456789"),
                addresses: null,
                findSimilar: true);

            // Act

            await fixture.Client.CreateDocument(command2);

            // Assert

            var document = await fixture.Client.GetDocument(command2.CreatedId);

            document.Should().NotBeNull();
            document.DocumentDetails.Should().BeEquivalentTo(command2.DocumentDetails);
            document.Addresses.Should().BeEquivalentTo(command2.Addresses);
            document.SimilarDocument.Should().Be(command.CreatedId);
        }
    }
}
