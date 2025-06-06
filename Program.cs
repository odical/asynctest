using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Remove incorrect namespace reference

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Parallel.ForEachAsync example...");

        // create array of 500 integers
        var items = new List<int>();
        for(int i=0; i< 500; i++)
        {
            items.Add(i);
        }

        // run the example, it will crash on a random index and will show the first exception, cancel remaining tasks, and then exit
        try
        {
            await ParallelExample.RunAsync(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Caught exception: {ex.Message}");
        }

        Console.WriteLine("Exit Application.");
    }
}
