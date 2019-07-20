namespace MasterPerform.Contracts.Entities
{
    /// <summary>
    /// Representation of Document details.
    /// </summary>
    public class DocumentDetails
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="email">Email.</param>
        /// <param name="phone">Phone number.</param>
        public DocumentDetails(string firstName, string lastName, string email, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone { get; }
    }
}
