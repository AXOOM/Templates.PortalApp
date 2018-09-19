using System;
using System.ComponentModel.DataAnnotations;

namespace MyVendor.MyApp.Contacts
{
    /// <summary>
    /// A representation of a note for serialization.
    /// </summary>
    public class NoteDto : IEquatable<NoteDto>
    {
        /// <summary>
        /// The content of the note
        /// </summary>
        [Required]
        public string Content { get; set; }

        public bool Equals(NoteDto other)
        {
            if (other == null) return false;
            return Content == other.Content;
        }

        public override bool Equals(object obj)
            => obj is NoteDto other && Equals(other);

        public override int GetHashCode()
            => Content?.GetHashCode() ?? 0;
    }
}
