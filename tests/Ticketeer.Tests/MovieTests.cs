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
}
