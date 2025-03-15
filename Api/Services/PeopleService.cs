using Api.Models;
using AutoMapper;
using Database.Entities;
using Database.Repositories;

namespace Api.Services;

public class PeopleService : IPeopleService
{
    private readonly IMapper mapper;
    private readonly IPeopleRepository repository;

    public PeopleService(IMapper mapper,
                         IPeopleRepository repository)
    {
        this.mapper = mapper;
        this.repository = repository;
    }

    public async Task<IReadOnlyCollection<PersonDetails>> GetAll(CancellationToken cancellationToken)
    {
        var people = await repository.GetAllAsync(cancellationToken);
        return mapper.Map<IReadOnlyCollection<PersonDetails>>(people);
    }

    public async Task<PersonDetails?> FindById(int id, CancellationToken cancellationToken)
    {
        var person = await repository.FindByIdAsync(id, cancellationToken);
        return mapper.Map<PersonDetails?>(person);
    }

    public async Task<IReadOnlyCollection<PersonDetails>> Search(string term, CancellationToken cancellationToken)
    {
        var people = await repository.SearchAsync(term, cancellationToken);
        return mapper.Map<IReadOnlyCollection<PersonDetails>>(people);
    }

    public async Task<int> Create(NewPersonDetails newPersonDetails, CancellationToken cancellationToken) => await repository.AddAsync(mapper.Map<Person>(newPersonDetails), cancellationToken);

    public async Task Update(UpdatePersonDetails updatePersonDetails, CancellationToken cancellationToken)
    {
        await repository.UpdateAsync(mapper.Map<Person>(updatePersonDetails), cancellationToken);
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken) => await repository.DeleteAsync(id, cancellationToken);
}