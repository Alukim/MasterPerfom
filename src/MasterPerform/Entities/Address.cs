namespace MasterPerform.Entities
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string addressLine, string city, string state)
        {
            AddressLine = addressLine;
            City = city;
            State = state;
        }

        public string AddressLine { get; set; }

        public string City { get; set; }

        public string State { get; set; }
    }
}
