using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.IntegrationTests.Tests;

/// <summary>
/// Класс нужен только для применения атрибута [CollectionDefinition]
/// </summary>
[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<ApiWebApplicationFactory>;

[Collection("IntegrationTests")]
public abstract class IntegrationTestBase
    : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly Mock<IDateTimeProvider> MockDateTimeProvider;

    protected IntegrationTestBase(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        MockDateTimeProvider = Factory.MockDateTimeProvider;
    }

    public async ValueTask InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    protected async ValueTask AssertResponseWithDomainProblemDetails(
        HttpResponseMessage response)
    {
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(
            TestContext.Current.CancellationToken);

        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(StatusCodes.Status400BadRequest);
        problemDetails.Title.ShouldNotBeNullOrWhiteSpace();
        problemDetails.Type.ShouldNotBeNullOrWhiteSpace();
        problemDetails.Detail.ShouldNotBeNullOrWhiteSpace();
        problemDetails.Instance.ShouldNotBeNullOrWhiteSpace();
        problemDetails.Extensions.ShouldContainKey("timestamp");
    }
}
