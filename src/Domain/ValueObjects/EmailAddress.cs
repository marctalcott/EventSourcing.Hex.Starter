using System;
using FluentValidation;

namespace Domain.ValueObjects
{
    public class EmailAddress: IEquatable<EmailAddress>
    {
      public string Address { get; }
      
      public EmailAddress(string address)
      {
          Address = address;
      }

      public bool Equals(EmailAddress other)
      {
          if (ReferenceEquals(null, other)) return false;
          if (ReferenceEquals(this, other)) return true;
          return Address == other.Address;
      }

      public override bool Equals(object obj)
      {
          if (ReferenceEquals(null, obj)) return false;
          if (ReferenceEquals(this, obj)) return true;
          if (obj.GetType() != this.GetType()) return false;
          return Equals((EmailAddress)obj);
      }

      public override int GetHashCode()
      {
          return (Address != null ? Address.GetHashCode() : 0);
      }
    }
    
    public class EmailAddressValidator : AbstractValidator<EmailAddress> {
        public EmailAddressValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
}