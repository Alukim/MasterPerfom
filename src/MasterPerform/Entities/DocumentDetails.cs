namespace MasterPerform.Entities
{
    public class DocumentDetails
    {
        public DocumentDetails()
        {
        }

        public DocumentDetails(string firstName, string lastName, string email, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
