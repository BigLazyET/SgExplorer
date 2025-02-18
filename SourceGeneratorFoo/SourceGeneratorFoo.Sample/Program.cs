using System;

namespace SourceGeneratorFoo.Sample;

public static partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom("Foo is Bar");

        Console.WriteLine("Hello, World!");
    }

    static partial void HelloFrom(string name);
}