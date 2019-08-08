using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyVendor.MyApp.Contacts
{
    /// <summary>
    /// A representation of a poke for database storage.
    /// </summary>
    public class PokeEntity
    {
        /// <summary>
        /// The ID of the poke.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The ID of the contact that was poked.
        /// </summary>
        [Required]
        public string ContactId { get; set; }

        /// <summary>
        /// The contact that was poked.
        /// </summary>
        [ForeignKey(nameof(ContactId))]
        public ContactEntity Contact { get; set; }

        /// <summary>
        /// When the poke was performed.
        /// </summary>
        [Required]
        public DateTime Timestamp { get; set; }
    }
}
