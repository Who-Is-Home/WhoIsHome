using System.Net.Mail;
using Galaxus.Functional;
using Google.Cloud.Firestore;

namespace WhoIsHome.Persons;

public class PersonService(FirestoreDb firestoreDb) : IPersonService
{
    private const string Collection = "person";

    public async Task<Result<Person, string>> GetPersonAsync(string id)
    {
        var result = await firestoreDb.Collection(Collection)
            .Where(Filter.EqualTo("id", id))
            .GetSnapshotAsync();

        var personDoc = result.Documents.SingleOrDefault();

        return personDoc is null 
            ? $"Can't find {nameof(Person)} with Id {id}" 
            : ConvertDocument(personDoc);
    }

    public async Task<Result<Person, string>> GetPersonByMailAsync(string email)
    {
        if (!MailAddress.TryCreate(email, out _))
        {
            return "Invalid Mail Address Format.";
        }

        var result = await firestoreDb.Collection(Collection)
            .Where(Filter.EqualTo("email", email))
            .GetSnapshotAsync();

        var personDoc = result.Documents.SingleOrDefault();
        
        return personDoc is null 
            ? $"Can't find {nameof(Person)} with Email {email}" 
            : ConvertDocument(personDoc);
    }
    
    public async Task<Result<Person, string>> TryCreateAsync(string name, string email)
    {
        var person = ConvertToModel(name, email);
        if (person.IsErr) return person.Err.Unwrap();

        var docRef = await firestoreDb.Collection(Collection).AddAsync(person.Unwrap());
        var personDoc = await docRef.GetSnapshotAsync();
        return ConvertDocument(personDoc);
    }

    private static Result<Person, string> ConvertToModel(string displayName, string email)
    {
        if (displayName.Length is 0 or > 30)
        {
            return "Name must be between 1 and 30 Characters Long.";
        }

        if (!MailAddress.TryCreate(email, out _))
        {
            return "Invalid Mail Address Format.";
        }
        
        return new Person
        {
            Id = null,
            DisplayName = displayName,
            Email = email
        };
    }

    private static Result<Person, string> ConvertDocument(DocumentSnapshot documentSnapshot)
    {
        var personDbModel = documentSnapshot.ConvertTo<Person>();

        if (personDbModel is null)
        {
            return $"Can't convert {documentSnapshot} to type ${nameof(Person)}";
        }

        return personDbModel;
    }
}