using System;
using FluentValidation;

namespace Domain.ValueObjects
{
    public class Address : IEquatable<Address>
    {
        public string Line1 { get; }
        public string Line2 { get; }
        public string Line3 { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string PostalCode { get; }

        public Address(
            string line1, string line2, string line3, string city, string state, string country, string postalCode)
        {
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
        }


        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Line1 == other.Line1
                   && Line2 == other.Line2
                   && Line3 == other.Line3
                   && City == other.City
                   && State == other.State
                   && Country == other.Country
                   && PostalCode == other.PostalCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line1, Line2, Line3, City, State, Country, PostalCode);
        }

        public static bool operator ==(Address left, Address right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !Equals(left, right);
        }
    }

    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Line1).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.State).NotEmpty();
            RuleFor(x => x.Country).NotEmpty();
            RuleFor(x => x.PostalCode)
                .Must(BeAValidPostalCode)
                .WithMessage("A valid postal code will have only numbers and " +
                             "will be 5 or 9 digits.");
        }

        private bool BeAValidPostalCode(string postalCode)
        {
            return (postalCode.Length == 5 || postalCode.Length == 9)
                   && int.TryParse(postalCode, out int result);
        }
    }
}