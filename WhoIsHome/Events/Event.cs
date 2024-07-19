using Galaxus.Functional;
using Google.Cloud.Firestore;
using WhoIsHome.Persons;

namespace WhoIsHome.Events;

[FirestoreData]
public class Event
{
    [FirestoreDocumentId]
    public string? Id { get; set; }

    [FirestoreProperty] 
    public string EventName { get; set; } = null!;

    [FirestoreProperty] 
    public Person Person { get; set; } = null!;
    
    [FirestoreProperty]
    public Timestamp Date { get; set; }
    
    [FirestoreProperty]
    public Timestamp StartTime { get; set; }
    
    [FirestoreProperty]
    public Timestamp EndTime { get; set; }
    
    [FirestoreProperty]
    public bool RelevantForDinner { get; set; }
    
    [FirestoreProperty]
    public Timestamp DinnerAt { get; set; }

    public static Result<Event, string> TryCreate(
        string eventName,
        Person person,
        DateTime date,
        DateTime startTime,
        DateTime endTime,
        bool relevantForDinner,
        DateTime dinnerAt)
    {
        if (startTime >= endTime)
        {
            return $"{nameof(StartTime)} must be before {nameof(EndTime)}.";
        }

        if (eventName.Length is <= 0 or >= 30)
        {
            return $"{nameof(EventName)} must be between 1 and 30 characters long.";
        }

        return new Event
        {
            Id = null,
            EventName = eventName,
            Person = person,
            Date = Timestamp.FromDateTime(date),
            StartTime = Timestamp.FromDateTime(startTime),
            EndTime = Timestamp.FromDateTime(endTime),
            RelevantForDinner = relevantForDinner,
            DinnerAt = Timestamp.FromDateTime(dinnerAt)
        };
    }
}