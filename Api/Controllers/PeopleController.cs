using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PeopleController : ControllerBase
{
    private readonly IPeopleService peopleService;

    public PeopleController(IPeopleService peopleService)
    {
        this.peopleService = peopleService;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<PersonDetails>> GetAll(CancellationToken cancellationToken) => await peopleService.GetAll(cancellationToken);

    [HttpGet("id")]
    public async Task<PersonDetails?> FindById(int id, CancellationToken cancellationToken) => await peopleService.FindById(id, cancellationToken);

    [HttpGet("term")]
    public async Task<IReadOnlyCollection<PersonDetails>> Search(string term, CancellationToken cancellationToken) => await peopleService.Search(term, cancellationToken);

    [HttpPut]
    public async Task<int> Create(NewPersonDetails newPersonDetails, CancellationToken cancellationToken) => await peopleService.Create(newPersonDetails, cancellationToken);

    [HttpPost]
    public async Task Update(UpdatePersonDetails updatePersonDetails, CancellationToken cancellationToken) => await peopleService.Update(updatePersonDetails, cancellationToken);

    [HttpDelete]
    public async Task<bool> Delete(int id, CancellationToken cancellationToken) => await peopleService.Delete(id, cancellationToken);
}