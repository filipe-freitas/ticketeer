#pragma warning disable IDE0290 // Use primary constructor
namespace Ticketeer.Domain.Entities;

public sealed class Movie
{
  public Movie(string title, string description, int duration, DateOnly releaseDate, string genre)
  {
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
