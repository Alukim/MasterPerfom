namespace MasterPerform.Contracts.Entities
{
    /// <summary>
    /// Representation of address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="addressLine">Address line.</param>
        /// <param name="city">City.</param>
        /// <param name="state">State.</param>
        public Address(string addressLine, string city, string state)
        {
            AddressLine = addressLine;
            City = city;
            State = state;
        }

        /// <summary>
        /// Address line.
        /// </summary>
        public string AddressLine { get; }

        /// <summary>
        /// City.
        /// </summary>
        public string City { get; }

        /// <summary>
        /// State.
        /// </summary>
        public string State { get; }
    }
}
