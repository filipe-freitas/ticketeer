#pragma warning disable IDE0290 // Use primary constructor
namespace Ticketeer.Domain.Entities;

public sealed class Movie
{
    public Movie(string title, string description, int duration, DateOnly releaseDate, string genre)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title), "Title cannot be null or whitespace.");
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentNullException(nameof(description), "Description cannot be null or whitespace.");
        }
        if (string.IsNullOrWhiteSpace(genre))
        {
            throw new ArgumentNullException(nameof(genre), "Genre cannot be null or whitespace.");
        }
        if (duration <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration cannot be zero or negative.");
        }

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Duration = duration;
        ReleaseDate = releaseDate;
        Genre = genre;
    }

    public Guid Id { get; }
    public string Title { get; }
    public string Description { get; }
    public int Duration { get; }
    public DateOnly ReleaseDate { get; }
    public string Genre { get; }
}
