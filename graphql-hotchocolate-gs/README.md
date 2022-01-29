# GraphQL HotChocolate Get Started in .NET 6

* ASP.NET Core .NET 6 GraphQL server example
* HotChocolate Get Started sample app translated to .NET 6
  - https://chillicream.com/docs/hotchocolate/get-started

## Create ASP.NET Core Web Project

```bash
dotnet --version
# 6.0.101

# setup manually
dotnet new web -n Demo
dotnet add ./Demo package HotChocolate.AspNetCore
# add files as in the doc...

# or official template
dotnet new -i HotChocolate.Templates.Server
dotnet new graphql
```

## Start Server

```bash
dotnet run --project ./Demo
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:7239

open https://localhost:7239/graphql/
```

## Run Query from CLI

```bash
curl -X POST -H "Content-Type: application/json" \
    -d '{"query": "{ book { author { name } } }"}' \
    https://localhost:7239/graphql
# {"data":{"book":{"author":{"name":"Jon Skeet"}}}}
```

## Standalone GUI Client

* Insomnia client for REST Client and GraphQL
  - https://insomnia.rest/download
* How to use Insomnia
  - https://dev.classmethod.jp/articles/insomnia-for-graphql/
* `brew install --cask graphql-playground`
  - https://github.com/graphql/graphql-playground
