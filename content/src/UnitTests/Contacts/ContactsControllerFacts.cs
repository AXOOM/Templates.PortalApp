using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace MyVendor.MyApp.Contacts
{
    public class ContactsControllerFacts : ControllerFactsBase
    {
        public ContactsControllerFacts(ITestOutputHelper output)
            : base(output)
        {}

        private readonly Mock<IContactService> _serviceMock = new Mock<IContactService>();

        protected override void ConfigureService(IServiceCollection services)
            => services.AddMock(_serviceMock);

        [Fact]
        public async Task ReadsAllFromService()
        {
            var contacts = new List<ContactDto>
            {
                new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"},
                new ContactDto {Id = "2", FirstName = "Jane", LastName = "Doe"}
            };
            _serviceMock.Setup(x => x.ReadAllAsync()).ReturnsAsync(contacts);

            var result = await HttpClient.GetAsync("http://localhost/api/contacts/");
            var body = await result.Content.ReadAsAsync<List<ContactDto>>();

            body.Should().Equal(contacts);
        }

        [Fact]
        public async Task ReadsFromService()
        {
            var contact = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};
            _serviceMock.Setup(x => x.ReadAsync("1")).ReturnsAsync(contact);

            var result = await HttpClient.GetAsync("http://localhost/api/contacts/1");
            var body = await result.Content.ReadAsAsync<ContactDto>();

            body.Should().Be(contact);
        }

        [Fact]
        public async Task CreatesInService()
        {
            var contactWithoutId = new ContactDto {FirstName = "John", LastName = "Smith"};
            var contactWithId = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};
            _serviceMock.Setup(x => x.CreateAsync(contactWithoutId)).ReturnsAsync(contactWithId);

            var result = await HttpClient.PostAsync("http://localhost/api/contacts", contactWithoutId, new JsonMediaTypeFormatter());
            var body = await result.Content.ReadAsAsync<ContactDto>();

            result.Headers.Location.Should().Be("http://localhost/api/contacts/1");
            body.Should().Be(contactWithId);
        }

        [Fact]
        public async Task RejectsCreateOnIncompleteBody()
        {
            var result = await HttpClient.PostAsync("http://localhost/api/contacts", new ContactDto(), new JsonMediaTypeFormatter());

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdatesInService()
        {
            var contact = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};

            var result = await HttpClient.PutAsync("http://localhost/api/contacts/1", contact, new JsonMediaTypeFormatter());

            result.EnsureSuccessStatusCode();
            _serviceMock.Verify(x => x.UpdateAsync(contact));
        }

        [Fact]
        public async Task RejectsUpdateOnIdMismatch()
        {
            var contact = new ContactDto {Id = "1", FirstName = "John", LastName = "Smith"};

            var result = await HttpClient.PutAsync("http://localhost/api/contacts/2", contact, new JsonMediaTypeFormatter());

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeletesFromService()
        {
            var result = await HttpClient.DeleteAsync("http://localhost/api/contacts/1");

            result.EnsureSuccessStatusCode();
            _serviceMock.Verify(x => x.DeleteAsync("1"));
        }

        [Fact]
        public async Task ReadsNoteFromService()
        {
            var note = new NoteDto {Content = "my note"};
            _serviceMock.Setup(x => x.ReadNoteAsync("1")).ReturnsAsync(note);

            var result = await HttpClient.GetAsync("http://localhost/api/contacts/1/note");
            var body = await result.Content.ReadAsAsync<NoteDto>();

            body.Should().Be(note);
        }

        [Fact]
        public async Task SetsNoteInService()
        {
            var note = new NoteDto {Content = "my note"};

            await HttpClient.PutAsync("http://localhost/api/contacts/1/note", note, new JsonMediaTypeFormatter());

            _serviceMock.Verify(x => x.SetNoteAsync("1", note));
        }

        [Fact]
        public async Task PokesViaService()
        {
            await HttpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/contacts/1/poke"));

            _serviceMock.Verify(x => x.PokeAsync("1"));
        }
    }
}
