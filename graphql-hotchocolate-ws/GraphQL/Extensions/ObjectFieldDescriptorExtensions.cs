using Microsoft.EntityFrameworkCore;

namespace ConferencePlanner.GraphQL;

public static class ObjectFieldDescriptorExtensions
{
    // UseDbContext: create a new middleware that handles scoping for a field
    // create: rent DBContext from the pool
    // disposeAsync: return it after the middleware is finished
    public static IObjectFieldDescriptor UseDbContext<TDbContext>(
        this IObjectFieldDescriptor descriptor
    ) where TDbContext : DbContext
    {
        return descriptor.UseScopedService<TDbContext>(
            create: s
                => s.GetRequiredService<IDbContextFactory<TDbContext>>()
                    .CreateDbContext(),
            disposeAsync: (s, c)
                => c.DisposeAsync());
    }
}
