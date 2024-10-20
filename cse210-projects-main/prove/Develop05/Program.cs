/*
    EXCEEDING REQUIREMENTS:

    1. Added a new activity: "Meditation Activity".
       - Helps users focus on breathing and positive thoughts.

    2. Implemented activity tracking:
       - Tracks how many times each activity was done and saves to "activityLog.txt".
       - Log is loaded at the start to keep track across sessions.

    3. Enhanced user experience with file saving/loading:
       - Saves and loads data so users can continue their mindfulness journey later.
*/

using System;
using System.Threading;
using System.IO; // For saving and loading log files
using System.Collections.Generic;

class Program
{
    // Keeps track of how many times each activity was done
    private static Dictionary<string, int> activityLog = new Dictionary<string, int>();

    static void Main(string[] args)
    {
        // Load log when program starts
        LoadActivityLog();

        while (true)
        {
            // Main menu options
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Gratitude Activity");
            Console.WriteLine("5. Meditation Activity");
            Console.WriteLine("6. Quit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            // Run selected activity
            if (choice == "1")
            {
                BreathingActivity breathingActivity = new BreathingActivity();
                breathingActivity.StartActivity();
                LogActivity("Breathing Activity"); // Log the activity
            }
            else if (choice == "2")
            {
                ReflectionActivity reflectionActivity = new ReflectionActivity();
                reflectionActivity.StartActivity();
                LogActivity("Reflection Activity");
            }
            else if (choice == "3")
            {
                ListingActivity listingActivity = new ListingActivity();
                listingActivity.StartActivity();
                LogActivity("Listing Activity");
            }
            else if (choice == "4")
            {
                GratitudeActivity gratitudeActivity = new GratitudeActivity();
                gratitudeActivity.StartActivity();
                LogActivity("Gratitude Activity");
            }
            else if (choice == "5")
            {
                MeditationActivity meditationActivity = new MeditationActivity();
                meditationActivity.StartActivity();
                LogActivity("Meditation Activity");
            }
            else if (choice == "6")
            {
                // Save log before quitting
                SaveActivityLog();
                Console.WriteLine("Exiting program...");
                break;
            }
            else
            {
                Console.WriteLine("Invalid choice, please try again.");
            }
        }
    }

    // Logs how many times an activity was done
    private static void LogActivity(string activityName)
    {
        if (activityLog.ContainsKey(activityName))
        {
            activityLog[activityName]++;
        }
        else
        {
            activityLog[activityName] = 1;
        }
        // Show the user their progress
        Console.WriteLine($"{activityName} has been done {activityLog[activityName]} times during this session.");
    }

    // Saves the log to a file
    private static void SaveActivityLog()
    {
        using (StreamWriter writer = new StreamWriter("activityLog.txt"))
        {
            foreach (var entry in activityLog)
            {
                writer.WriteLine($"{entry.Key}:{entry.Value}");
            }
        }
    }

    // Loads the log from a file if it exists
    private static void LoadActivityLog()
    {
        if (File.Exists("activityLog.txt"))
        {
            try
            {
                using (StreamReader reader = new StreamReader("activityLog.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                        {
                            activityLog[parts[0]] = count;
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error loading activity log: {e.Message}");
            }
        }
    }
}

// Base class for all activities
abstract class MindfulnessActivity
{
    protected string activityName;
    protected string description;
    protected int duration;

    // Start the activity
    public void StartActivity()
    {
        DisplayStartingMessage();
        RunActivity(); // Runs specific activity
        DisplayEndingMessage();
    }

    // Shows start message and asks for duration
    private void DisplayStartingMessage()
    {
        Console.WriteLine($"Starting {activityName}: {description}");
        bool validInput = false;
        while (!validInput)
        {
            Console.Write("Enter the duration in seconds: ");
            if (int.TryParse(Console.ReadLine(), out duration) && duration > 0)
            {
                validInput = true;
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
            }
        }
        Console.WriteLine("Prepare to begin");
        Pause(4); // Pause before starting
    }

    // Shows end message after the activity
    private void DisplayEndingMessage()
    {
        Console.WriteLine($"Good job! You finished the {activityName} for {duration} seconds.");
        Pause(3);
    }

    // Simple pause with spinner animation
    protected void Pause(int seconds)
    {
        string[] spinner = { "|", "/", "-", "\\" };
        for (int i = 0; i < seconds * 4; i++)
        {
            Console.Write(spinner[i % spinner.Length] + "\r");
            Thread.Sleep(250); // 4 spinner moves per second
        }
        Console.WriteLine();
    }
    protected abstract void RunActivity();
}

// Breathing activity class
class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity()
    {
        activityName = "Breathing Activity";
        description = "Helps you relax by guiding you through deep breathing.";
    }

    // Breathing exercise
    protected override void RunActivity()
    {
        for (int i = 0; i < duration; i += 19) // Breathing cycle
        {
            Console.WriteLine("Breathe in...");
            NumericCountdown(4); // 4 seconds inhale
            Console.WriteLine("Hold your breath for 7 seconds...");
            NumericCountdown(7); // Hold for 7 seconds
            Console.WriteLine("Breathe out...");
            NumericCountdown(8); // 8 seconds exhale
        }
    }

    // Countdown for breathing
    private void NumericCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"{i} ");
            Thread.Sleep(1000);
        }
        Console.WriteLine();
    }
}

// Meditation activity class
class MeditationActivity : MindfulnessActivity
{
    public MeditationActivity()
    {
        activityName = "Meditation Activity";
        description = "Guides you through calm breathing and focus.";
    }

    // Meditation exercise
    protected override void RunActivity()
    {
        Console.WriteLine("Close your eyes and focus on your breath.");
        Pause(5);
        Console.WriteLine("Now, think about a positive thought or mantra.");
        Pause(10);
        Console.WriteLine("Continue focusing until the session ends...");
        Pause(duration - 15); // Continue meditation for remaining time
    }
}

// Reflection activity class
class ReflectionActivity : MindfulnessActivity
{
    private string[] prompts = {
        "Think of a time when you stood up for someone.",
        "Think of a time when you did something really hard.",
        "Think of a time when you helped someone.",
        "Think of a time when you were selfless."
    };

    private string[] questions = {
        "Why was this meaningful?",
        "Have you done this before?",
        "How did you start?",
        "How did you feel when it was done?",
        "What made this time special?",
        "What is your favorite part of this?",
        "What can you learn from this?"
    };

    public ReflectionActivity()
    {
        activityName = "Reflection Activity";
        description = "Helps you reflect on meaningful moments.";
    }

    // Reflection exercise
    protected override void RunActivity()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Length)];
        Console.WriteLine(prompt);
        Pause(5);

        foreach (string question in questions)
        {
            Console.WriteLine(question);
            Pause(5); // Give time to think
        }
    }
}

// Listing activity class
class ListingActivity : MindfulnessActivity
{
    private string[] prompts = {
        "List as many strengths as you can.",
        "List your personal achievements.",
        "List goals for the next 5 years."
    };

    public ListingActivity()
    {
        activityName = "Listing Activity";
        description = "Helps you focus on listing strengths, achievements, and goals.";
    }

    // Listing exercise
    protected override void RunActivity()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Length)];
        Console.WriteLine(prompt);
        Pause(3);
        Console.WriteLine("Start listing your thoughts:");
        Pause(duration); // Simulate listing
    }
}

// Gratitude activity class
class GratitudeActivity : MindfulnessActivity
{
    private string[] prompts = {
        "Think of three things you're grateful for today.",
        "Consider the people who make your life better.",
        "Reflect on opportunities you've had."
    };

    public GratitudeActivity()
    {
        activityName = "Gratitude Activity";
        description = "Helps you think about what you're thankful for.";
    }

    // Gratitude exercise
    protected override void RunActivity()
    {
        Random random = new Random();
        string prompt = prompts[random.Next(prompts.Length)];
        Console.WriteLine(prompt);
        Pause(duration); // Simulate reflection time
    }
}
