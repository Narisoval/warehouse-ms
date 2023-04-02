using MassTransit.Testing;
using Warehouse.API.Messaging.Events;

namespace API.IntegrationTests.Helpers;

public class ControllerTestHelper
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ITestHarness _testHarness;

    public ControllerTestHelper(SemaphoreSlim semaphore, ITestHarness testHarness)
    {
        _semaphore = semaphore;
        _testHarness = testHarness;
    }

    public async Task<(int, HttpResponseMessage)> GetPublishedMessagesCount<T>(Func<Task<HttpResponseMessage>> method)
        where T : class, IEvent
    {
        await _semaphore.WaitAsync();
        try
        {
            await _testHarness.Start();
            var messagesPublishedBefore = _testHarness.Published.Select<T>().Count();
            var response = await method();
            var messagesPublishedAfter = _testHarness.Published.Select<T>().Count();
            var messagesPublished = messagesPublishedAfter - messagesPublishedBefore;
            return (messagesPublished, response);
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
