# URL-Shortener-API

A simple URL Shortener API using .NET, AWS DynamoDB, AWS Lambda &amp; AWS API Gateway

## Tests

### Coverage Report Generation

```
dotnet test --settings coverlet.runsettings
```

```
reportgenerator -reports:"tests/**/TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```
