using Api.Controllers;
using Api.Models;
using Api.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Controllers;

[TestFixture]
public class PeopleControllerTests
{
    private readonly IReadOnlyCollection<PersonDetails> samplePeople = new List<PersonDetails>
    {
        new(1, "John", "Doe", null, "john.doe@example.com", "123 Street", new DateTime(1985, 5, 15)),
        new(2, "Jane", "Doe", null, "jane.doe@example.com", "456 Avenue", new DateTime(1990, 8, 20))
    };

    private readonly PersonDetails samplePerson = new(1, "John", "Doe", null, "john.doe@example.com", "123 Street", new DateTime(1985, 5, 15));
    private PeopleController controller;
    private Mock<IPeopleService> peopleServiceMock;

    [SetUp]
    public void SetUp()
    {
        peopleServiceMock = new();
        controller = new(peopleServiceMock.Object);
    }

    [Test]
    public async Task GetAll_ShouldReturnPeople()
    {
        peopleServiceMock.Setup(service => service.GetAll(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(samplePeople);

        var result = await controller.GetAll(CancellationToken.None);

        Assert.That(result, Is.InstanceOf<IReadOnlyCollection<PersonDetails>>());
        Assert.That(result, Is.EqualTo(samplePeople));
    }

    [Test]
    public async Task FindById_ShouldReturnPerson_WhenPersonExists()
    {
        const int personId = 1;
        peopleServiceMock.Setup(service => service.FindById(personId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(samplePerson);

        var result = await controller.FindById(personId, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<PersonDetails>());
        Assert.That(result?.Id, Is.EqualTo(personId));
        Assert.That(result?.FirstName, Is.EqualTo("John"));
    }

    [Test]
    public async Task FindById_ShouldReturnNull_WhenPersonDoesNotExist()
    {
        const int personId = 999;
        peopleServiceMock.Setup(service => service.FindById(personId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync((PersonDetails?) null);

        var result = await controller.FindById(personId, CancellationToken.None);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task Search_ShouldReturnMatchingPeople()
    {
        const string searchTerm = "John";
        peopleServiceMock.Setup(service => service.Search(searchTerm, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(samplePeople);

        var result = await controller.Search(searchTerm, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<IReadOnlyCollection<PersonDetails>>());
        Assert.That(result, Is.EqualTo(samplePeople));
    }

    [Test]
    public async Task Create_ShouldReturnCreatedPersonId()
    {
        var newPersonDetails = new NewPersonDetails("James", "Smith", null, "james.smith@example.com", "789 Road", new DateTime(1995, 12, 10));
        peopleServiceMock.Setup(service => service.Create(It.IsAny<NewPersonDetails>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(3);

        var result = await controller.Create(newPersonDetails, CancellationToken.None);

        Assert.That(result, Is.EqualTo(3));
    }

    [Test]
    public async Task Update_ShouldReturnSuccess()
    {
        var updatePersonDetails = new UpdatePersonDetails(1, "UpdatedFirstName", "UpdatedLastName", null, "updated.email@example.com", "Updated Address", new DateTime(1985, 5, 15));
        peopleServiceMock.Setup(service => service.Update(It.IsAny<UpdatePersonDetails>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask);

        await controller.Update(updatePersonDetails, CancellationToken.None);

        peopleServiceMock.Verify(service => service.Update(It.IsAny<UpdatePersonDetails>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Delete_ShouldReturnTrue_WhenPersonIsDeleted()
    {
        const int personId = 1;
        peopleServiceMock.Setup(service => service.Delete(personId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

        var result = await controller.Delete(personId, CancellationToken.None);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task Delete_ShouldReturnFalse_WhenPersonDoesNotExist()
    {
        const int personId = 999;
        peopleServiceMock.Setup(service => service.Delete(personId, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

        var result = await controller.Delete(personId, CancellationToken.None);

        Assert.That(result, Is.False);
    }
}