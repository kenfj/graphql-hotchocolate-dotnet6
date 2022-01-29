# Session 1: Create a new GraphQL server project

## Create Project

```bash
dotnet --version
# 6.0.101

dotnet new sln -n ConferencePlanner
dotnet new web -n GraphQL
dotnet sln add GraphQL

mkdir GraphQL/Data
# create GraphQL/Data/Speaker.cs

dotnet add GraphQL package Microsoft.EntityFrameworkCore.Sqlite
# create GraphQL/Data/ApplicationDbContext.cs

# Register the DB Context Service in Program.cs

# Configuring EF Migrations
dotnet add GraphQL package Microsoft.EntityFrameworkCore.Tools

dotnet new tool-manifest
dotnet tool install dotnet-ef --local

cd ./GraphQL/

dotnet build GraphQL
dotnet ef migrations add Initial --project GraphQL
dotnet ef database update --project GraphQL

# reset database in case when recreate
# dotnet ef database drop --project GraphQL
# dotnet ef database update --project GraphQL

# check db (on MacOS)
sqlite3 $TMPDIR/conferences.db
sqlite> .tables
# Speakers               __EFMigrationsHistory
sqlite> .schema Speakers
# CREATE TABLE IF NOT EXISTS "Speakers" (
#     "Id" INTEGER NOT NULL CONSTRAINT "PK_Speakers" PRIMARY KEY AUTOINCREMENT,
#     "Name" TEXT NOT NULL,
#     "Bio" TEXT NOT NULL,
#     "WebSite" TEXT NOT NULL
# );
sqlite> .exit
```

## Adding HotChocolate GraphQL

```bash
dotnet add GraphQL package HotChocolate.AspNetCore
# create Query.cs
# AddQueryType Query and UseEndpoints of GraphQL in Program.cs

# start server
dotnet run --project GraphQL
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:7016

open https://localhost:7016/graphql
# check the return type of the speakers field
# Document > Schema reference

# Adding Mutations with relay mutation pattern
# create AddSpeakerInput.cs AddSpeakerPayload.cs Mutation.cs
# add the new Mutation type to your schema in Program.cs

# restart server
dotnet run --project GraphQL
```

## Insert

* Add a speaker by writing a GraphQL `mutation`

```graphql
# https://localhost:7016/graphql/
# > Documents > Operations > Execute
mutation AddSpeaker {
  addSpeaker(input: {
    name: "Speaker Name"
    bio: "Speaker Bio"
    webSite: "http://speaker.website" }) {
    speaker {
      id
    }
  }
}
# {"data":{"addSpeaker":{"speaker":{"id":123}}}}
```

* or add speaker by curl `query`

```bash
QUERY='mutation AddSpeaker {
    addSpeaker(input: {
        name: \"Foo Name\"
        bio: \"Foo Bio\"
        webSite: \"http://foo.website\"
    }) {
        speaker { id }
    }
}'
# remove new line
QUERY2=$(echo $QUERY | tr -d '\n')

curl -X POST -H "Content-Type: application/json" \
    https://localhost:7016/graphql \
    -d "{\"query\": \"${QUERY2}\"}"
# {"data":{"addSpeaker":{"speaker":{"id":123}}}}
```

* or curl using `query` and `variables`

```bash
QUERY='mutation ($speaker: AddSpeakerInput!) {
    addSpeaker(input: $speaker) {
        speaker { id }
    }
}'
QUERY2=$(echo $QUERY | tr -d '\n')
VARIABLES='{
    "speaker": {
        "name": "Bar Name",
        "bio": "Bar Bio",
        "webSite": "http://bar.website"
    }
}'
VARIABLES2=$(echo $VARIABLES | tr -d '\n')

curl -X POST -H "Content-Type: application/json" \
    https://localhost:7016/graphql \
    -d "{\"query\": \"${QUERY2}\", \"variables\": ${VARIABLES2}}"
# {"data":{"addSpeaker":{"speaker":{"id":123}}}}
```

## Select

* Query the names of all the speakers

```graphql
# https://localhost:7016/graphql/
# > Documents > Operations > Execute
query GetSpeakerNames {
  speakers {
    name
  }
}
```

* or omit operation type (`query`) and operation name (`GetSpeakerNames`)
  - but encouraged to use operation type and name
  - https://graphql.org/learn/queries/#operation-name

```graphql
{
  speakers {
    name
  }
}
```

* or run by curl

```bash
curl -X POST -H "Content-Type: application/json" \
    https://localhost:7016/graphql \
    -d '{"query": "query GetSpeakerNames { speakers { name } }"}'
# {"data":{"speakers":[{"name":"Speaker Name"},{"name":"Foo Name"}]}}
```

* select one speaker by id

```graphql
query GetSpeakerName {
  speaker(id:2) {
    name
  }
}
```

## Package Versions

```bash
dotnet list ./GraphQL package
# [net6.0]:
# > HotChocolate.AspNetCore                   12.5.0   12.5.0
# > Microsoft.EntityFrameworkCore.Sqlite      6.0.1    6.0.1
# > Microsoft.EntityFrameworkCore.Tools       6.0.1    6.0.1
```
