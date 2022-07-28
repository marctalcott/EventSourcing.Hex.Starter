namespace Domain.ValueObjects
{
    public class ContactInformation
    {
        public Address Address { get; }
        public PhoneNumber PrimaryPhoneNumber { get; }
        public PhoneNumber SecondaryPhoneNumber { get; }
        public EmailAddress EmailAddress { get; }
    }
}