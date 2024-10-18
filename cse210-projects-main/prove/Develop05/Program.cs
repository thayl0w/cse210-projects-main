using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Main menu loop
        while (true)
        {
            // Display the main menu options
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Quit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                BreathingActivity breathingActivity = new BreathingActivity();
                breathingActivity.StartActivity();
            }
            else if (choice == "2")
            {
                ReflectionActivity reflectionActivity = new ReflectionActivity();
                reflectionActivity.StartActivity();
            }
            else if (choice == "3")
            {
                ListingActivity listingActivity = new ListingActivity();
                listingActivity.StartActivity();
            }
            else if (choice == "4")
            {
                // Exit the program
                Console.WriteLine("Exiting program...");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, please try again.");
            }
        }
    }
}

// Base class for all activities (common attributes and methods)
abstract class MindfulnessActivity
{
    protected string activityName;
    protected string description;
    protected int duration;

    // This method handles starting the activity
    public void StartActivity()
    {
        DisplayStartingMessage();
        RunActivity();
        DisplayEndingMessage();
    }

    // Display the start message and ask for duration
    private void DisplayStartingMessage()
    {
        Console.WriteLine($"Starting {activityName}: {description}");
        Console.Write("Enter the duration in seconds: ");
        duration = int.Parse(Console.ReadLine());
        Console.WriteLine("Prepare to begin...");
        Pause(3); // Pause before starting the activity
    }

    // Display a generic ending message
    private void DisplayEndingMessage()
    {
        Console.WriteLine($"Good job! You have completed the {activityName} for {duration} seconds.");
        Pause(3);
    }

    // This method pauses and shows an animation
    protected void Pause(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            Console.Write(". ");
            Thread.Sleep(1000); // 1-second delay
        }
        Console.WriteLine();
    }

    // Abstract method to run the specific activity (must be implemented by derived classes)
    protected abstract void RunActivity();
}

// Derived class for the Breathing Activity
class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity()
    {
        activityName = "Breathing Activity";
        description = "This activity will help you relax by guiding you through deep breathing.";
    }

    // Implement the breathing exercise
    protected override void RunActivity()
    {
        for (int i = 0; i < duration; i += 6) // Breathing cycle (2 counts per cycle)
        {
            Console.WriteLine("Breathe in...");
            Pause(3); // Simulate breathing in for 3 seconds
            Console.WriteLine("Breathe out...");
            Pause(3); // Simulate breathing out for 3 seconds
        }
    }
}

// Derived class for the Reflection Activity
class ReflectionActivity : MindfulnessActivity
{
    private string[] prompts = {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private string[] questions = {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times?",
        "What is your favorite thing about this experience?",
        "What did you learn from this experience?",
        "How can you apply this to other areas of your life?"
    };

    public ReflectionActivity()
    {
        activityName = "Reflection Activity";
        description = "This activity helps you reflect on times when you showed strength or resilience.";
    }

    // Implement the reflection exercise
    protected override void RunActivity()
    {
        Random random = new Random();
        Console.WriteLine(prompts[random.Next(prompts.Length)]);
        Pause(2);

        for (int i = 0; i < duration; i += 10) // Display questions in intervals
        {
            Console.WriteLine(questions[random.Next(questions.Length)]);
            Pause(5);
        }
    }
}

// Derived class for the Listing Activity
class ListingActivity : MindfulnessActivity
{
    private string[] prompts = {
        "Who are people that you appreciate?",
        "What are your personal strengths?",
        "Who have you helped recently?",
        "When have you felt peace recently?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity()
    {
        activityName = "Listing Activity";
        description = "This activity helps you reflect on the good things in your life.";
    }

    // Implement the listing exercise
    protected override void RunActivity()
    {
        Random random = new Random();
        Console.WriteLine(prompts[random.Next(prompts.Length)]);
        Pause(3);

        Console.WriteLine("Start listing items now:");
        int itemCount = 0;
        for (int i = 0; i < duration; i += 5) // 5 seconds per item
        {
            Console.Write("Item: ");
            Console.ReadLine(); // Read user input for each item
            itemCount++;
        }

        Console.WriteLine($"You listed {itemCount} items.");
    }
}
