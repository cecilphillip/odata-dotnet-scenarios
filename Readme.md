## Exploring various IQueryable backends with OData queries

### Samples

- [API backed with in-memory collection](InMemoryApi/)
- [API backed by MongoDB](MongoApi/)
- [API backed by EF Core and SQLite](EfCoreApi/)
- [API backed by Cosmos DB (using the v3 .NET SDK)](CosmosSQlAi/)

### Making requests

The [requests.http](requests.http) file includes various requests to be run against each API endpoint when run locally. It also contains loose notes on various issues.

If you're using VS Code, the [REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client&WT.mc_id=academic-0000-cephilli) supports running requests in this format.

### Populating the databases.

Each API has a `/api/populate` endpoint that you can make a HTTP `POST` request to for setting up the data. This does not apply to the in-memory sample.

### Reqirements

- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core?WT.mc_id=academic-0000-cephilli)
- [REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client&WT.mc_id=academic-0000-cephilli)
- [MongoDB Community Server](https://www.mongodb.com/download-center/community) or compatiable service for version 3.6
- [Azure Cosmos DB database w/ the SQL API ](https://azure.microsoft.com/try/cosmosdb/?WT.mc_id=academic-0000-cephilli)

> Optionally you can run MongoDB on a local port with docker using the following command.

```shell
> docker run -d -p 27017:27017 --name mongodb mongo:4.2.5-bionic
```

### Questions

- Is `EnableQuery` needed on `ODataController`?
- What is `OData.JsonLight` ?
