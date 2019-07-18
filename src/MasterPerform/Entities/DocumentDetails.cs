namespace MasterPerform.Entities
{
    public class DocumentDetails
    {
        public DocumentDetails(string firstName, string lastName, string email, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Phone { get; }
    }
}
