using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is your first name? ");
        string first = Console.ReadLine();

        Console.Write("What is yout last name? ");
        string last = Console.ReadLine();

        Console.WriteLine($"\nYour name is {last}, {first} {last}.");
    }
}