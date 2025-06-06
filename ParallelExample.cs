using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public class ParallelExample
{
    static int _randomFailureIndex = new Random().Next(0, 100); // Randomly select an index to fail
    public static async Task RunAsync(IEnumerable<int> items)
    {
        using var cts = new CancellationTokenSource();
        int canceledTasks = 0;
        Console.WriteLine($"Test failure will occur on index: {_randomFailureIndex}");
        // Create a variable to hold the first exception.
        Exception? firstException = null;
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 4,
            CancellationToken = cts.Token
        };
        try
        {
            await Parallel.ForEachAsync(items, options, async (item, token) =>
            {
                try
                {
                    // Simulate work
                    await DoWorkAsync(item, token);
                }
                catch (Exception ex)
                {
                    // Cancel all remaining tasks
                    if (ex is OperationCanceledException)
                    {
                        canceledTasks++;
                        Console.WriteLine($"{item} canceled:");
                    }
                    else
                    {
                        // store the exception
                        Interlocked.CompareExchange(ref firstException, ex, null);
                        Console.WriteLine($"got exception on {item}");
                        // cancel any ongoing tasks
                        cts.Cancel(true); // cancel any remaining tasks
                    }
                }
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Async loop exiting with exception: {e.Message},  {e.GetType()}");
        }
        if (firstException is not null)
        {
            // Complete the progress with the true exception before throwing.
            Console.WriteLine($"Task exception was thrown: {firstException.Message}.   Canceled {canceledTasks} tasks that were not complete.");
            throw firstException;
        }
    }

    private static async Task DoWorkAsync(int index, CancellationToken token)
    {
        // Simulate possible failure
        if (index == _randomFailureIndex)
        {
            throw new InvalidOperationException($"Failed on {index}");
        }

        await Task.Delay(1000, token); // Simulate async work
        Console.WriteLine($"Processed item {index}");
    }
}
