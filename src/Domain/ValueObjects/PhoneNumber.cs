using System;
using FluentValidation;

namespace Domain.ValueObjects
{
    public enum PhoneType
    {
        Mobile,
        Home,
        Work,
        Other
    }
    public class PhoneNumber:IEquatable<PhoneNumber>
    {
        public string Number { get; }
        public PhoneType PhoneType { get; }

        public PhoneNumber(string number, PhoneType phoneType)
        {
            Number = number;
            PhoneType = phoneType;
        }


        public bool Equals(PhoneNumber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Number == other.Number && PhoneType == other.PhoneType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PhoneNumber)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Number, (int)PhoneType);
        }
    }
    public class PhoneNumberValidator : AbstractValidator<PhoneNumber> {
        public PhoneNumberValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(20)
                .WithMessage("Invalid phone number.");
        }
    }
}