using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace MyVendor.MyApp.Contacts
{
    public class ContactServiceFacts : DatabaseFactsBase<ContactService>
    {
        [Fact]
        public async Task ReadsAllFromDatabase()
        {
            string id1 = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            string id2 = Context.Contacts.Add(new ContactEntity {FirstName = "Jane", LastName = "Doe"}).Entity.Id;
            Context.SaveChanges();

            var result = await Subject.ReadAllAsync();
            result.Should().Equal(
                new ContactDto {Id = id1, FirstName = "John", LastName = "Smith"},
                new ContactDto {Id = id2, FirstName = "Jane", LastName = "Doe"});
        }

        [Fact]
        public async Task ReadsFromDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            var result = await Subject.ReadAsync(id);
            result.Should().Be(new ContactDto {Id = id, FirstName = "John", LastName = "Smith"});
        }

        [Fact]
        public async Task CreatesInDatabase()
        {
            var result = await Subject.CreateAsync(new ContactDto {FirstName = "John", LastName = "Smith"});

            Context.Contacts.Single().Should().BeEquivalentTo(new ContactEntity {Id = result.Id, FirstName = "John", LastName = "Smith"});
            GetMock<IContactMetrics>().Verify(x => x.Write());
        }

        [Fact]
        public async Task UpdatesInDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await Subject.UpdateAsync(new ContactDto {Id = id, FirstName = "Jane", LastName = "Doe"});

            var entity = Context.Contacts.Find(id);
            entity.FirstName.Should().Be("Jane");
            entity.LastName.Should().Be("Doe");
            GetMock<IContactMetrics>().Verify(x => x.Write());
        }

        [Fact]
        public async Task DeletesFromDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await Subject.DeleteAsync(id);

            Context.Contacts.Should().BeEmpty();
        }

        [Fact]
        public async Task ReadsNoteFromDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith", Note = "my note"}).Entity.Id;
            Context.SaveChanges();

            var note = await Subject.ReadNoteAsync(id);
            note.Should().Be(new NoteDto {Content = "my note"});
        }

        [Fact]
        public async Task WritesNoteInDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await Subject.SetNoteAsync(id, new NoteDto {Content = "my note"});

            Context.Contacts.Find(id).Note.Should().Be("my note");
            GetMock<IContactMetrics>().Verify(x => x.Write());
        }

        [Fact]
        public async Task StoresPokeInDatabase()
        {
            string id = Context.Contacts.Add(new ContactEntity {FirstName = "John", LastName = "Smith"}).Entity.Id;
            Context.SaveChanges();

            await Subject.PokeAsync(id);

            Context.Pokes.Single().ContactId.Should().Be(id);
            GetMock<IContactMetrics>().Verify(x => x.Poke());
        }
    }
}
