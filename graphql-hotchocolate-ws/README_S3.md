# Session 3: Understanding GraphQL query execution and DataLoader

## Configure field scoped services

* DBContext is not thread-safe
* therefore change it to use DBContext pooling
* this is Entity Framework specific topic

```bash
# change AddDbContext to AddPooledDbContextFactory in Program.cs
# it will keep 128 DBContext instances in its pool

mkdir GraphQL/Extensions

# create ObjectFieldDescriptorExtensions.cs UseApplicationDbContextAttribute
# update Query.cs Mutation.cs using UseApplicationDbContext and ScopedService
```

* fetch the speaker three times in parallel should work

```graphql
query GetSpeakerNamesInParallel {
  a: speakers {
    name
    bio
  }
  b: speakers {
    name
    bio
  }
  c: speakers {
    name
    bio
  }
}
```

## Adding the remaining data models

```bash
dotnet build

dotnet ef migrations add Refactoring --project GraphQL
dotnet ef database update --project GraphQL

dotnet run --project GraphQL
```

## Adding DataLoader

* BatchDataLoader collects requests to avoid n+1 problem
  - https://chillicream.com/docs/hotchocolate/v10/data-fetching

```bash
mkdir GraphQL/DataLoader
# add GraphQL/DataLoader/SpeakerByIdDataLoader.cs
# register the DataLoader with the schema in Startup.cs
```

* this will be resolved as one query

```graphql
query GetSpecificSpeakerById {
  a: speaker(id: 1) {
    name
  }
  b: speaker(id: 1) {
    name
  }
}
```

## Fluent type configurations

* to change the shape of types that we do not want to annotate with GraphQL attributes.

```bash
# add GraphQL/DataLoader/SessionByIdDataLoader.cs

mkdir GraphQL/Types

# add GraphQL/Types/SpeakerType.cs
```

* new GraphQL representation of our speaker type

```graphql
type Speaker {
    sessions: [Sessions]
    id: Int!
    name: String!
    bio: String
    website: String
}
```

* now this query should work

```graphql
query GetSpeakerWithSessions {
   speakers {
       name
       sessions {
           title
       }
   }
}
```
