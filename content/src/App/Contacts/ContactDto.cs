using System;
using System.ComponentModel.DataAnnotations;

namespace MyVendor.MyApp.Contacts
{
    /// <summary>
    /// A representation of a contact for serialization.
    /// </summary>
    public class ContactDto : IEquatable<ContactDto>
    {
        /// <summary>
        /// The ID of the contact.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The first name of the contact.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the contact.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        public bool Equals(ContactDto other)
        {
            if (other == null) return false;
            return Id == other.Id
                && FirstName == other.FirstName
                && LastName == other.LastName;
        }

        public override bool Equals(object obj)
            => obj is ContactDto other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (FirstName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (LastName?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
