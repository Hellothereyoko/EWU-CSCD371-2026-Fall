using System.Net;
using System.Net.Http;
using Assignment.Interfaces;

namespace Assignment.Tests;

public sealed class PeopleEnrichmentServiceTests
{
    [Fact]
    public async Task SuccessWritesOnlyValidFullyEnrichedPeople()
    {
        using var handler = new DelegateHandler(request =>
        {
            if (request.RequestUri!.AbsolutePath == "/people/test")
            {
                return Response(
                    "FirstName,LastName,EmailAddress,ZipCode\n" +
                    "Ada,Lovelace,ada@example.com,99201\n" +
                    ",Bad,bad,99202");
            }

            return Response("\"Spokane\",\"WA\"");
        });
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client);
        var path = TemporaryPath();

        try
        {
            var result = await service.RunAsync("test", path);

            Assert.Equal(2, result.TotalPeople);
            Assert.Equal(1, result.RejectedPeople);
            Assert.Equal(1, result.EnrichedPeople);
            Assert.Contains("Missing name: 1", result.FormatRejectionSummary());
            var lines = await File.ReadAllLinesAsync(path);
            Assert.Equal("FirstName,LastName,EmailAddress,ZipCode,City,State", lines[0]);
            Assert.Single(lines[1..]);
            Assert.Contains("Ada,Lovelace,ada@example.com,99201,Spokane,WA", lines);
        }
        finally
        {
            Delete(path);
        }
    }

    [Fact]
    public async Task PeopleDownloadRetriesInvalidTimeoutAndExceptionModes()
    {
        var attempt = 0;
        using var handler = new DelegateHandler(request =>
        {
            if (request.RequestUri!.AbsolutePath != "/people/test")
            {
                return Response("\"Spokane\",\"WA\"");
            }

            attempt++;
            if (attempt == 1)
            {
                return Response("not,a,valid,header");
            }
            if (attempt == 2)
            {
                throw new TaskCanceledException("timeout");
            }
            if (attempt == 3)
            {
                throw new HttpRequestException("endpoint exception");
            }

            return Response(
                "FirstName,LastName,EmailAddress,ZipCode\n" +
                "Ada,Lovelace,ada@example.com,99201");
        });
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client);
        var path = TemporaryPath();

        try
        {
            var result = await service.RunAsync("test", path);
            Assert.Equal(4, attempt);
            Assert.Equal(1, result.EnrichedPeople);
        }
        finally
        {
            Delete(path);
        }
    }

    [Fact]
    public async Task LocationLookupRetriesEveryFailureAndRemovesBadResult()
    {
        var attempt = 0;
        using var handler = new DelegateHandler(request =>
        {
            if (request.RequestUri!.AbsolutePath == "/people/test")
            {
                return Response(
                    "FirstName,LastName,EmailAddress,ZipCode\n" +
                    "Ada,Lovelace,ada@example.com,99201");
            }

            attempt++;
            if (attempt == 1)
            {
                return Response("not valid location");
            }
            if (attempt == 2)
            {
                throw new HttpRequestException("exception");
            }
            if (attempt == 3)
            {
                throw new TaskCanceledException("timeout");
            }

            return Response("\"Spokane\",\"WA\"");
        });
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client);
        var path = TemporaryPath();

        try
        {
            var result = await service.RunAsync("test", path);
            Assert.Equal(4, attempt);
            Assert.Equal(1, result.EnrichedPeople);
        }
        finally
        {
            Delete(path);
        }
    }

    [Fact]
    public async Task ExhaustedPeopleRetriesThrowAndLeaveNoOutput()
    {
        using var handler = new DelegateHandler(_ => Response("bad"));
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client, retryCount: 3);
        var path = TemporaryPath();

        await Assert.ThrowsAsync<PeopleDownloadException>(
            () => service.RunAsync("test", path));
        Assert.False(File.Exists(path));
        Assert.Empty(Directory.GetFiles(Path.GetDirectoryName(path)!, "*.tmp"));
    }

    [Fact]
    public async Task CancellationStopsPeopleDownloadAndIsHonored()
    {
        using var handler = new DelegateHandler(_ => Response("bad"));
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.RunAsync("test", TemporaryPath(), cancellation.Token));
    }

    [Fact]
    public async Task LocationConcurrencyIsLimited()
    {
        var active = 0;
        var maximum = 0;
        using var handler = new DelegateHandler(async (request, cancellationToken) =>
        {
            if (request.RequestUri!.AbsolutePath == "/people/test")
            {
                var people = Enumerable.Range(1, 20)
                    .Select(index => $"Person{index},User,index{index}@example.com,99{index:000}")
                    .Prepend("FirstName,LastName,EmailAddress,ZipCode");
                return Response(string.Join('\n', people));
            }

            var current = Interlocked.Increment(ref active);
            UpdateMaximum(ref maximum, current);
            try
            {
                await Task.Delay(30, cancellationToken);
                return Response("\"City\",\"WA\"");
            }
            finally
            {
                Interlocked.Decrement(ref active);
            }
        });
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client, maxConcurrency: 3);
        var path = TemporaryPath();

        try
        {
            var result = await service.RunAsync("test", path);
            Assert.Equal(3, maximum);
            Assert.Equal(20, result.EnrichedPeople);
        }
        finally
        {
            Delete(path);
        }
    }

    [Fact]
    public async Task InvalidLocationDataRemovesPerson()
    {
        using var handler = new DelegateHandler(request =>
            request.RequestUri!.AbsolutePath == "/people/test"
                ? Response("FirstName,LastName,EmailAddress,ZipCode\nAda,Lovelace,ada@example.com,99201")
                : Response("\"\",\"not-a-state\""));
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client);
        var path = TemporaryPath();

        try
        {
            var result = await service.RunAsync("test", path);
            Assert.Equal(1, result.RejectedPeople);
            Assert.Contains("Ada Lovelace", result.Rejections.Select(rejection => rejection.RowDescription));
            Assert.Equal(0, result.EnrichedPeople);
        }
        finally
        {
            Delete(path);
        }
    }

    [Fact]
    public async Task UnexpectedExceptionPropagatesWithoutWrapping()
    {
        var unexpected = new InvalidOperationException("bug");
        using var handler = new DelegateHandler(_ => throw unexpected);
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client);

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(
            () => service.RunAsync("test", TemporaryPath()));
        Assert.Same(unexpected, thrown);
    }

    [Fact]
    public async Task FailedLocationRunDoesNotLeaveTemporaryFiles()
    {
        using var handler = new DelegateHandler(request =>
            request.RequestUri!.AbsolutePath == "/people/test"
                ? Response("FirstName,LastName,EmailAddress,ZipCode\nAda,Lovelace,ada@example.com,99201")
                : throw new HttpRequestException("failure"));
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://example.test/") };
        using var service = CreateService(client, retryCount: 2);
        var path = TemporaryPath();

        var result = await service.RunAsync("test", path);
        Assert.Equal(0, result.EnrichedPeople);
        Assert.True(File.Exists(path));
        Assert.Empty(Directory.GetFiles(Path.GetDirectoryName(path)!, "*.tmp"));
    }

    private static PeopleEnrichmentService CreateService(
        HttpClient client,
        int maxConcurrency = 10,
        int retryCount = 5) =>
        new(client, maxConcurrency, retryCount, TimeSpan.Zero);

    private static HttpResponseMessage Response(string content) =>
        new(HttpStatusCode.OK)
        {
            Content = new StringContent(content)
        };

    private static string TemporaryPath()
    {
        var directory = Path.Combine(Path.GetTempPath(), "assignment9-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "output.csv");
    }

    private static void Delete(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    private static void UpdateMaximum(ref int maximum, int candidate)
    {
        var observed = Volatile.Read(ref maximum);
        while (candidate > observed)
        {
            var original = Interlocked.CompareExchange(ref maximum, candidate, observed);
            if (original == observed)
            {
                return;
            }
            observed = original;
        }
    }

    private sealed class DelegateHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handler;

        public DelegateHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
            : this((request, _) => Task.FromResult(handler(request)))
        {
        }

        public DelegateHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            _handler(request, cancellationToken);
    }
}
