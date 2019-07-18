namespace MasterPerform.Entities
{
    public class Address
    {
        public Address(string addressLine, string city, string state)
        {
            AddressLine = addressLine;
            City = city;
            State = state;
        }

        public string AddressLine { get; }

        public string City { get; }

        public string State { get; }
    }
}
