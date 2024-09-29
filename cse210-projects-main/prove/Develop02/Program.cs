
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Entry
{
    public string Content { get; set; }
    public string Prompt { get; set; }
    public string Date { get; set; }
    public string Category { get; set; } 

    // Creating a new entry
    public Entry(string content, string prompt, string category)
    {
        Content = content;
        Prompt = prompt;
        Category = category;
        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

   
    public override string ToString()
    {
        return $"Date: {Date}\nCategory: {Category}\nPrompt: {Prompt}\nEntry: {Content}\n";
    }

    
    public string ToCSVFormat()
    {
        return $"\"{Date}\",\"{Prompt}\",\"{Category}\",\"{Content.Replace("\"", "\"\"")}\"";
    }

    
    private static List<string> SplitCSV(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string currentField = "";

        foreach (char c in line)
        {
            if (c == '"' && !inQuotes)
                inQuotes = true; 
            else if (c == '"' && inQuotes)
                inQuotes = false; 
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentField.Trim()); 
                currentField = "";
            }
            else
            {
                currentField += c; 
            }
        }
        result.Add(currentField.Trim()); 

        return result;
    }

    // Convert a line from CSV format into an Entry object
    public static Entry FromCSVFormat(string line)
    {
        var parts = SplitCSV(line);
        string date = parts[0].Trim('"');
        string prompt = parts[1].Trim('"');
        string category = parts[2].Trim('"');
        string content = parts[3].Trim('"').Replace("\"\"", "\"");

        return new Entry(content, prompt, category) { Date = date };
    }
}

public class Journal
{
    public List<Entry> Entries { get; set; } = new List<Entry>();

    // new entry to the journal with category
    public void AddEntry(string content, string prompt, string category)
    {
        Entry entry = new Entry(content, prompt, category);
        Entries.Add(entry);
    }

    // show journal entries
    public void DisplayEntries()
    {
        if (Entries.Count == 0)
        {
            Console.WriteLine("No entries available.");
        }
        else
        {
            foreach (Entry entry in Entries)
            {
                Console.WriteLine(entry.ToString());
            }
        }
    }

    // Save to CSV file
    public void SaveToCSV(string filename)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filename, false, System.Text.Encoding.UTF8))
            {
                writer.WriteLine("Date,Prompt,Category,Entry"); // Header for CSV
                foreach (Entry entry in Entries)
                {
                    writer.WriteLine(entry.ToCSVFormat());
                }
            }
            Console.WriteLine($"Journal saved to {filename} in CSV format.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving the journal: {ex.Message}");
        }
    }

    // Load fromCSV file
    public void LoadFromCSV(string filename)
    {
        try
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("File not found.");
                return;
            }

            Entries.Clear(); // Clear current entries before loading new ones
            foreach (var line in File.ReadLines(filename, System.Text.Encoding.UTF8).Skip(1)) // Skip header
            {
                Entries.Add(Entry.FromCSVFormat(line));
            }
            Console.WriteLine($"Journal loaded from {filename} in CSV format.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading the journal: {ex.Message}");
        }
    }

    //  prompt generato
    public string GeneratePrompt()
    {
        string[] prompts = {
            "Who was the most challenging part of the day?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?",
            "What did I learn today?"
        };
        Random rand = new Random();
        return prompts[rand.Next(prompts.Length)];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal myJournal = new Journal();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\nJournal Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display all entries");
            Console.WriteLine("3. Save journal to CSV file");
            Console.WriteLine("4. Load journal from CSV file");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    string prompt = myJournal.GeneratePrompt();
                    Console.WriteLine($"\nPrompt: {prompt}");
                    Console.Write("Write your journal entry: ");
                    string content = Console.ReadLine();
                    Console.Write("Enter a category for this entry (e.g., Personal, Work, Reflection): ");
                    string category = Console.ReadLine();
                    myJournal.AddEntry(content, prompt, category);
                    break;

                case "2":
                    myJournal.DisplayEntries();
                    break;

                case "3":
                    Console.Write("Enter the filename to save the journal (with .csv extension): ");
                    string saveFilename = Console.ReadLine();
                    myJournal.SaveToCSV(saveFilename);
                    break;

                case "4":
                    Console.Write("Enter the filename to load the journal (with .csv extension): ");
                    string loadFilename = Console.ReadLine();
                    myJournal.LoadFromCSV(loadFilename);
                    break;

                case "5":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}

/*
   Enhancements:
   1. Added a "Category" field to journal entries, allowing users to categorize their journal entries 
      (e.g., Personal, Work, Reflection).
   2. Implemented CSV saving/loading to allow journal entries to be opened in Excel or other spreadsheet software.

   // TAPOS NAAAAAA!!! YEEEYYYYYY //  
*/
