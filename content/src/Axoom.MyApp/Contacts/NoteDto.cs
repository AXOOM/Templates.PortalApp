namespace Axoom.MyApp.Contacts
{
    /// <summary>
    /// A representation of a note for serialization.
    /// </summary>
    public class NoteDto
    {
        /// <summary>
        /// The content of the note
        /// </summary>
        public string Content { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return (obj is NoteDto other) && Content == other.Content;
        }

        public override int GetHashCode() => Content?.GetHashCode() ?? 0;
    }
}
