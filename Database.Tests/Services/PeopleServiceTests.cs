using Api.Models;
using Api.Services;
using AutoMapper;
using Database.Entities;
using Database.Repositories;
using Moq;
using NUnit.Framework;

namespace Tests.Services;

[TestFixture]
public class PeopleServiceTests
{
    private readonly Person[] samplePeople =
    [
        new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Address = "123 Street", DateOfBirth = new DateTime(1985, 5, 15) },
        new() { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Address = "456 Avenue", DateOfBirth = new DateTime(1990, 8, 20) }
    ];

    private readonly Person samplePerson = new()
    {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        Address = "123 Street",
        DateOfBirth = new DateTime(1985, 5, 15)
    };

    private readonly IReadOnlyCollection<PersonDetails> samplePersonDetails = new List<PersonDetails>
    {
        new(1, "John", "Doe", null, "john.doe@example.com", "123 Street", new DateTime(1985, 5, 15)),
        new(2, "Jane", "Doe", null, "jane.doe@example.com", "456 Avenue", new DateTime(1990, 8, 20))
    };

    private Mock<IMapper> mapperMock;
    private PeopleService peopleService;
    private Mock<IPeopleRepository> repositoryMock;

    [SetUp]
    public void SetUp()
    {
        repositoryMock = new();
        mapperMock = new();

        peopleService = new(mapperMock.Object, repositoryMock.Object);
    }

    [Test]
    public async Task GetAll_ShouldReturnMappedPeople()
    {
        repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(samplePeople);

        mapperMock.Setup(m => m.Map<IReadOnlyCollection<PersonDetails>>(It.IsAny<IReadOnlyCollection<Person>>()))
                  .Returns(samplePersonDetails);

        var result = await peopleService.GetAll(CancellationToken.None);

        Assert.That(result, Is.InstanceOf<IReadOnlyCollection<PersonDetails>>());
        Assert.That(result, Is.EqualTo(samplePersonDetails));
    }

    [Test]
    public async Task FindById_ShouldReturnMappedPerson_WhenPersonExists()
    {
        const int personId = 1;
        repositoryMock.Setup(r => r.FindByIdAsync(personId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(samplePerson);
        mapperMock.Setup(m => m.Map<PersonDetails>(It.IsAny<Person>()))
                  .Returns(new PersonDetails(1, "John", "Doe", null, "john.doe@example.com", "123 Street", new DateTime(1985, 5, 15)));

        var result = await peopleService.FindById(personId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<PersonDetails>());
        Assert.That(result?.Id, Is.EqualTo(personId));
        Assert.That(result?.FirstName, Is.EqualTo("John"));
    }

    [Test]
    public async Task FindById_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        const int personId = 999;
        repositoryMock.Setup(r => r.FindByIdAsync(personId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Person?) null);

        var result = await peopleService.FindById(personId, CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Search_ShouldReturnMappedPeople()
    {
        const string searchTerm = "John";
        repositoryMock.Setup(r => r.SearchAsync(searchTerm, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(samplePeople);
        mapperMock.Setup(m => m.Map<IReadOnlyCollection<PersonDetails>>(It.IsAny<IReadOnlyCollection<Person>>()))
                  .Returns(samplePersonDetails);

        var result = await peopleService.Search(searchTerm, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<IReadOnlyCollection<PersonDetails>>());
        Assert.That(result, Is.EqualTo(samplePersonDetails));
    }

    [Test]
    public async Task Create_ShouldReturnPersonId_WhenPersonIsCreated()
    {
        var newPersonDetails = new NewPersonDetails("James", "Smith", null, "james.smith@example.com", "789 Road", new DateTime(1995, 12, 10));
        var person = new Person { Id = 3, FirstName = "James", LastName = "Smith", Email = "james.smith@example.com", Address = "789 Road", DateOfBirth = new DateTime(1995, 12, 10) };
        repositoryMock.Setup(r => r.AddAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(3);
        mapperMock.Setup(m => m.Map<Person>(It.IsAny<NewPersonDetails>()))
                  .Returns(person);

        var result = await peopleService.Create(newPersonDetails, CancellationToken.None);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public async Task Update_ShouldCallUpdateOnRepository()
    {
        var updatePersonDetails = new UpdatePersonDetails(1, "UpdatedFirstName", "UpdatedLastName", null, "updated.email@example.com", "Updated Address", new DateTime(1985, 5, 15));
        var person = new Person { Id = 1, FirstName = "UpdatedFirstName", LastName = "UpdatedLastName", Email = "updated.email@example.com", Address = "Updated Address", DateOfBirth = new DateTime(1985, 5, 15) };
        repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask);
        mapperMock.Setup(m => m.Map<Person>(It.IsAny<UpdatePersonDetails>()))
                  .Returns(person);

        await peopleService.Update(updatePersonDetails, CancellationToken.None);

        repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Person>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Delete_ShouldReturnTrue_WhenPersonIsDeleted()
    {
        const int personId = 1;
        repositoryMock.Setup(r => r.DeleteAsync(personId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        var result = await peopleService.Delete(personId, CancellationToken.None);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task Delete_ShouldReturnFalse_WhenPersonDoesNotExist()
    {
        const int personId = 999;
        repositoryMock.Setup(r => r.DeleteAsync(personId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false);

        var result = await peopleService.Delete(personId, CancellationToken.None);

        Assert.That(result, Is.False);
    }
}