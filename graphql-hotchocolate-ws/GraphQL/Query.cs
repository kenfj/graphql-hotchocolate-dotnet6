using ConferencePlanner.GraphQL.Data;
using ConferencePlanner.GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL;

public class Query
{
    // original GetSpeakers since session 1
    // public IQueryable<Speaker> GetSpeakers(
    //      [Service] ApplicationDbContext context
    // ) => context.Speakers;

    // By annotating UseApplicationDbContext,
    // applying a Middleware to the field resolver pipeline.
    [UseApplicationDbContext]
    public Task<List<Speaker>> GetSpeakers(
        [ScopedService] ApplicationDbContext context
    ) => context.Speakers.ToListAsync();

    public Task<Speaker> GetSpeakerAsync(
        int id,
        SpeakerByIdDataLoader dataLoader,
        CancellationToken cancellationToken
    ) => dataLoader.LoadAsync(id, cancellationToken);
}
