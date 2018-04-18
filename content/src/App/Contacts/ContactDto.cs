using System.ComponentModel.DataAnnotations;

namespace Axoom.MyApp.Contacts
{
    /// <summary>
    /// A representation of a contact for serialization.
    /// </summary>
    public class ContactDto
    {
        /// <summary>
        /// The ID of the contact.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The first name of the contact.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the contact.
        /// </summary>
        public string LastName { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ContactDto other
                && Id == other.Id
                && FirstName == other.FirstName
                && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Id != null ? Id.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
