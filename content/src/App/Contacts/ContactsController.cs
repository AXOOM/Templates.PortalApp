using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace VendorName.AppName.Contacts
{
    /// <summary>
    /// Provides access to contacts in an address book.
    /// </summary>
    [Route("api/contacts")]
    public class ContactsController : CollectionController<ContactDto>
    {
        private readonly IContactService _service;

        public ContactsController(IContactService service)
            : base(service)
        {
            _service = service;
        }

        /// <summary>
        /// Returns the note for a specific contact.
        /// </summary>
        /// <param name="id">The ID of the contact to get the note for.</param>
        [HttpGet, Route("{id}/note")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified contact not found.")]
        public async Task<NoteDto> ReadNote([FromRoute] string id)
            => await _service.ReadNoteAsync(id);

        /// <summary>
        /// Sets a note for a specific contact.
        /// </summary>
        /// <param name="id">The ID of the contact to set the note for.</param>
        /// <param name="note">The note to set</param>
        [HttpPut, Route("{id}/note")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, description: "Missing or invalid request body.")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified contact not found.")]
        public async Task<ActionResult> SetNote([FromRoute] string id, [FromBody] NoteDto note)
        {
            if (note == null) throw new InvalidDataException("Missing request body.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _service.SetNoteAsync(id, note);

            return StatusCode((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Pokes a contact.
        /// </summary>
        /// <param name="id">The ID of the contact to poke.</param>
        [HttpPost, Route("{id}/poke")]
        [SwaggerResponse((int)HttpStatusCode.Accepted)]
        [SwaggerResponse((int)HttpStatusCode.NotFound, description: "Specified contact not found.")]
        public async Task<ActionResult> Poke([FromRoute] string id)
        {
            await _service.PokeAsync(id);

            return Accepted();
        }
    }
}
