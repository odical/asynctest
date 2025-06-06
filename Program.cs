using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Remove incorrect namespace reference

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting ParallelExample demonstration...");

        var items = new List<int>();
        for(int i=0; i< 500; i++)
        {
            items.Add(i);
        }

        try
        {
            await ParallelExample.RunAsync(items);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unhandled exception: {ex.Message}");
        }

        Console.WriteLine("Demonstration complete.");
    }
}
