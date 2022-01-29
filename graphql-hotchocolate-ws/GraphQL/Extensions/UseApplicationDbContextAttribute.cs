
using System.Reflection;
using ConferencePlanner.GraphQL.Data;
using HotChocolate.Types.Descriptors;

namespace ConferencePlanner.GraphQL;

// creates a so-called descriptor-attribute
public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
{
    public override void OnConfigure(
        IDescriptorContext context,
        IObjectFieldDescriptor descriptor,
        MemberInfo member)
    {
        descriptor.UseDbContext<ApplicationDbContext>();
    }
}
