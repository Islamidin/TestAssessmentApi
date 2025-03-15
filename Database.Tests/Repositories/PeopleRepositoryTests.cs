using Database;
using Database.Entities;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests.Repositories;

[TestFixture]
public class PeopleRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;
    private AppDbContext context;
    private PeopleRepository repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
                      .UseInMemoryDatabase("TestDatabase")
                      .Options;

        context = new(options);
        repository = new(context);

        context.Database.EnsureCreated();
        context.People.RemoveRange(context.People);

        context.People.AddRange(
            new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Address = "123 Street", DateOfBirth = new DateTime(1980, 1, 1) },
            new Person { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Address = "456 Avenue", DateOfBirth = new DateTime(1990, 2, 2) },
            new Person { Id = 3, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@example.com", Address = "789 Boulevard", DateOfBirth = new DateTime(1985, 3, 3) }
        );
        context.SaveChanges();
    }

    [Test]
    public async Task GetAllAsync_ShouldReturnAllPeople()
    {
        var result = await repository.GetAllAsync(cancellationToken);

        Assert.That(result.Length, Is.EqualTo(3));
    }

    [Test]
    public async Task SearchAsync_ShouldReturnMatchingPeople()
    {
        const string term = "John";

        var result = await repository.SearchAsync(term, cancellationToken);

        Assert.That(result.Length, Is.EqualTo(1));
        Assert.That(result[0].FirstName, Is.EqualTo("John"));
    }

    [Test]
    public async Task FindByIdAsync_ShouldReturnPerson_WhenPersonExists()
    {
        const int id = 1;

        var result = await repository.FindByIdAsync(id, cancellationToken);

        Assert.That(result, Is.Not.Null);
        Assert.That(result?.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task FindByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        const int id = 999;

        var result = await repository.FindByIdAsync(id, cancellationToken);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddAsync_ShouldAddNewPersonAndReturnId()
    {
        var person = new Person { FirstName = "Tom", LastName = "Jerry", Email = "tom.jerry@example.com", Address = "101 Road", DateOfBirth = new DateTime(2000, 4, 4) };

        var newPersonId = await repository.AddAsync(person, cancellationToken);

        Assert.That(newPersonId, Is.EqualTo(4));
    }

    [Test]
    public async Task UpdateAsync_ShouldUpdatePerson()
    {
        var person = await repository.FindByIdAsync(1, CancellationToken.None);
        Assert.That(person, Is.Not.Null);
        person!.FirstName = "Updated Name";

        await repository.UpdateAsync(person, CancellationToken.None);

        var updatedPerson = await repository.FindByIdAsync(person.Id, CancellationToken.None);
        Assert.That(updatedPerson?.FirstName, Is.EqualTo("Updated Name"));
    }

    [Test]
    public async Task DeleteAsync_ShouldDeletePerson_WhenPersonExists()
    {
        const int id = 1;

        var result = await repository.DeleteAsync(id, cancellationToken);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPersonDoesNotExist()
    {
        const int id = 999;

        var result = await repository.DeleteAsync(id, cancellationToken);

        Assert.That(result, Is.False);
    }
}