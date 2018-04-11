using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axoom.MyApp.Contacts
{
    /// <summary>
    /// Manages contacts in an address book.
    /// </summary>
    public interface IContactService : ICrudService<ContactDto>
    {
        /// <summary>
        /// Returns the note for a specific contact.
        /// </summary>
        /// <param name="id">The ID of the contact to get the note for.</param>
        /// <exception cref="KeyNotFoundException">Specified contact not found.</exception>
        Task<NoteDto> ReadNoteAsync(string id);

        /// <summary>
        /// Sets a note for a specific contact.
        /// </summary>
        /// <param name="id">The ID of the contact to set the note for.</param>
        /// <param name="note">The note to set</param>
        /// <exception cref="KeyNotFoundException">Specified contact not found.</exception>
        Task SetNoteAsync(string id, NoteDto note);

        /// <summary>
        /// Pokes a contact.
        /// </summary>
        /// <param name="id">The ID of the contact to poke.</param>
        /// <exception cref="KeyNotFoundException">Specified contact not found.</exception>
        Task PokeAsync(string id);
    }
}
