using Database.Entities;

namespace Database.Repositories;

public interface IPeopleRepository
{
    Task<Person[]> GetAllAsync(CancellationToken cancellationToken);

    Task<Person[]> SearchAsync(string term, CancellationToken cancellationToken);

    Task<Person?> FindByIdAsync(int id, CancellationToken cancellationToken);

    Task<int> AddAsync(Person person, CancellationToken cancellationToken);

    Task UpdateAsync(Person person, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}