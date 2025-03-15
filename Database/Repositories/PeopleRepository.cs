using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories;

public class PeopleRepository : IPeopleRepository
{
    private readonly AppDbContext context;

    public PeopleRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Person[]> GetAllAsync(CancellationToken cancellationToken) => await context.People.ToArrayAsync(cancellationToken);

    public async Task<Person[]> SearchAsync(string term, CancellationToken cancellationToken)
    {
        var search = $"%{term.ToLowerInvariant()}%";
        return await context.People.Where(x => EF.Functions.Like(x.FirstName.ToLower(), search) || EF.Functions.Like(x.LastName.ToLower(), search)).ToArrayAsync(cancellationToken);
    }

    public async Task<Person?> FindByIdAsync(int id, CancellationToken cancellationToken) => await context.People.FindAsync([id], cancellationToken);

    public async Task<int> AddAsync(Person person, CancellationToken cancellationToken)
    {
        await context.People.AddAsync(person, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return person.Id;
    }

    public async Task UpdateAsync(Person person, CancellationToken cancellationToken)
    {
        context.People.Update(person);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var person = await FindByIdAsync(id, cancellationToken);
        if (person == null)
        {
            return false;
        }

        context.People.Remove(person);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}