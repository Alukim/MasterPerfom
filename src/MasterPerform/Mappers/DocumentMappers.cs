using MasterPerform.Contracts.Responses;
using MasterPerform.Entities;
using System.Linq;
using AddressContract = MasterPerform.Contracts.Entities.Address;
using DocumentDetailsContract = MasterPerform.Contracts.Entities.DocumentDetails;

namespace MasterPerform.Mappers
{
    internal static class DocumentMappers
    {
        internal static DocumentResponse BuildResponse(this Document document)
        {
            if (document is null)
                return null;

            return new DocumentResponse(
                documentId: document.Id,
                documentDetails: document.Details.BuildDetails(),
                addresses: document.Addresses?.Select(x => x.BuildAddress()).ToList(),
                similarDocument: document.SimilarDocument);
        }

        private static DocumentDetailsContract BuildDetails(this DocumentDetails details)
        {
            if (details is null)
                return null;

            return new DocumentDetailsContract(
                firstName: details.FirstName,
                lastName: details.LastName,
                email: details.Email,
                phone: details.Phone);
        }

        private static AddressContract BuildAddress(this Address address)
        {
            if (address is null)
                return null;

            return new AddressContract(
                addressLine: address.AddressLine,
                city: address.City,
                state: address.State);
        }
    }
}
