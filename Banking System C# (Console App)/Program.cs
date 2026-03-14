using System;
using System.Collections.Generic;

public class Solution
{
    public Queue<int> CloneQueue(Queue<int> queue)
    {
        var clone = new Queue<int>();
        Clone(queue, clone, queue.Count);
        return clone;
    }

    private void Clone(Queue<int> original, Queue<int> clone, int n)
    {
        if (n == clone.Count)
        {
            return;
        }

        int x = original.Dequeue();

        clone.Enqueue(x);
        original.Enqueue(x);

        Clone(original, clone, n);
    }
}

public class Program
{

    static void Main(string[] args)
    {
        var solution = new Solution();
        var original = new Queue<int>();

        original.Enqueue(1);
        original.Enqueue(2);
        original.Enqueue(3);
        original.Enqueue(4);

        var clone = solution.CloneQueue(original);

        Console.WriteLine("\nThis is the original queue:");
        foreach (var item in clone)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\nThis is the cloned queue:");
        foreach (var item in original)
        {
            Console.WriteLine(item);
        }

        Console.ReadKey();
    }
}

