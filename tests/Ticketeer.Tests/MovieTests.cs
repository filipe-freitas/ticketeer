using Ticketeer.Domain.Entities;

using Shouldly;

namespace Ticketeer.Tests;

public class MovieTests
{
    [Fact]
    public void Constructor_WithValidData_SetsPropertiesCorrectly()
    {
        // Arrange
        var movieData = (Title: "Inception", Description: "Space movie", Duration: 212, ReleaseDate: new DateOnly(2025, 12, 13), Genre: "Drama");

        // Act
        var newMovie = new Movie(
            movieData.Title,
            movieData.Description,
            movieData.Duration,
            movieData.ReleaseDate,
            movieData.Genre
        );

        // Assert
        newMovie.ShouldNotBeNull();
        newMovie.Id.ShouldBeOfType<Guid>();
        newMovie.Title.ShouldBe(movieData.Title);
        newMovie.Description.ShouldBe(movieData.Description);
        newMovie.Duration.ShouldBe(movieData.Duration);
        newMovie.ReleaseDate.ShouldBe(movieData.ReleaseDate);
        newMovie.Genre.ShouldBe(movieData.Genre);
    }

    [Theory]
    [InlineData("", "Valid description", "Valid genre")]
    [InlineData(" ", "Valid description", "Valid genre")]
    [InlineData("Valid title", "", "Valid genre")]
    [InlineData("Valid title", " ", "Valid genre")]
    [InlineData("Valid title", "Valid description", "")]
    [InlineData("Valid title", "Valid description", " ")]
    [InlineData(null, null, null)]
    public void Constructor_WithInvalidStrings_ThrowsArgumentNullException(string? title, string? description, string? genre)
    {
        // Arrange
        var movieData = (Title: title, Description: description, Duration: 212, ReleaseDate: new DateOnly(2025, 12, 13), Genre: genre);

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => new Movie(
            movieData.Title!,
            movieData.Description!,
            movieData.Duration,
            movieData.ReleaseDate,
            movieData.Genre!
        ));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidDuration_ThrowsArgumentOutOfRangeException(int duration)
    {
        // Arrange
        var movieData = (Title: "Inception", Description: "Space movie", Duration: duration, ReleaseDate: new DateOnly(2025, 12, 13), Genre: "Drama");

        // Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => new Movie(
            movieData.Title,
            movieData.Description,
            movieData.Duration,
            movieData.ReleaseDate,
            movieData.Genre
        ));
    }
}
