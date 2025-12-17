namespace Ticketeer.Domain.Entities;

public sealed class Cinema
{
    public Cinema(string name, string address)
    {
        Name = name;
        Address = address;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Address { get; }
}
