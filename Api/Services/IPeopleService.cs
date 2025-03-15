using Api.Models;

namespace Api.Services;

public interface IPeopleService
{
    Task<IReadOnlyCollection<PersonDetails>> GetAll(CancellationToken cancellationToken);

    Task<PersonDetails?> FindById(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PersonDetails>> Search(string term, CancellationToken cancellationToken);

    Task<int> Create(NewPersonDetails newPersonDetails, CancellationToken cancellationToken);

    Task Update(UpdatePersonDetails updatePersonDetails, CancellationToken cancellationToken);

    Task<bool> Delete(int id, CancellationToken cancellationToken);
}