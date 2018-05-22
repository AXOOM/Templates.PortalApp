using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyVendor.MyApp.Contacts
{
    /// <summary>
    /// A representation of a contact for database storage.
    /// </summary>
    public class ContactEntity
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

        /// <summary>
        /// A note about a contact.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// A list of pokes performed on this contact.
        /// </summary>
        public ICollection<PokeEntity> Pokes { get; set; } = new List<PokeEntity>();
    }
}
