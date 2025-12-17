using Shouldly;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Tests;

public sealed class CinemaTests
{
  [Fact]
  public void Constructor_WithValidData_ReturnCorrectData()
  {
    // Act
    var cinemaData = (Name: "BigScreen", Address: "One Street, 290");

    // Arrange
    var newCinema = new Cinema(cinemaData.Name, cinemaData.Address);

    // Assert
    newCinema.ShouldNotBeNull();
    newCinema.Name.ShouldBe(cinemaData.Name);
    newCinema.Address.ShouldBe(cinemaData.Address);
  }
}
