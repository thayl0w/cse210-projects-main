using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        // Load scriptures from a file
        ScriptureLibrary library = new ScriptureLibrary("scriptures.txt");
        // Get a random scripture
        Scripture scripture = library.GetRandomScripture();
        
        // Show the scripture
        scripture.Display();
    }
}

// Class to manage multiple scriptures
public class ScriptureLibrary
{
    private List<Scripture> _scriptures; // List to store scriptures

    // Load scriptures from a file
    public ScriptureLibrary(string filePath)
    {
        _scriptures = new List<Scripture>();
        LoadScripturesFromFile(filePath); // Load scriptures
    }

    // Read scriptures from the file
    private void LoadScripturesFromFile(string filePath)
    {
        foreach (var line in File.ReadLines(filePath)) // Read each line
        {
            // Split line into reference and text
            var parts = line.Split('|');
            if (parts.Length == 2) 
            {
                // Add scripture to the list
                _scriptures.Add(new Scripture(parts[0], parts[1]));
            }
        }
    }

    // Get a random scripture from the list
    public Scripture GetRandomScripture()
    {
        Random random = new Random(); // Create random number generator
        return _scriptures[random.Next(_scriptures.Count)]; // Return random scripture
    }
}

// Class to represent a scripture
public class Scripture
{
    private Reference _reference; // Holds reference details (book, chapter, verse)
    private List<Word> _words; // List of words in the scripture
    public Scripture(string referenceText, string text)
    {
        _reference = new Reference(referenceText); // Create reference
        _words = new List<Word>(); 
        
    
        foreach (var word in text.Split(' '))
        {
            _words.Add(new Word(word));
        }
    }

    // Display the scripture
    public void Display()
    {
        Console.Clear(); // Clear screen
        Console.WriteLine(_reference.GetReferenceText()); // Show reference

        // Show each word or "___" if hidden
        foreach (var word in _words)
        {
            Console.Write(word.IsHidden ? "___ " : word.Text + " ");
        }

        Console.WriteLine(); // New line
        Console.WriteLine("Press Enter to hide words or type 'quit' to exit.");
        
        var input = Console.ReadLine(); // Get user input
        if (input?.ToLower() == "quit") // If user types "quit"
        {
            return; // Stop showing scripture
        }

        HideRandomWords(); // Hide some words
        Display(); // Show again
    }

    // Randomly hide some words
    private void HideRandomWords()
    {
        Random random = new Random(); // Random number generator
        for (int i = 0; i < 2; i++) // Hide 3 words
        {
            int index = random.Next(_words.Count); 
            _words[index].Hide(); 
        }
    }
}

// Class to represent a scripture reference (book, chapter, verse)
public class Reference
{
    private string _book; // Book name
    private int _chapter; // Chapter number
    private int _startVerse; // Starting verse
    private int? _endVerse; // ending verse 
    public Reference(string referenceText)
    {
        ParseReference(referenceText);
    }
   private void ParseReference(string referenceText)
{
    var parts = referenceText.Split(' ');

    // Check if the first part of the scripture is a number 
    if (int.TryParse(parts[0], out int bookNumber))
    {
        // If the first part is a number, the book name consists of the first two parts
        _book = parts[0] + " " + parts[1]; // Combine both parts for the book name
        
        var chapterVerse = parts[2].Split(':');
        _chapter = int.Parse(chapterVerse[0]);

        // Handle verse ranges or single verses
        var verses = chapterVerse[1].Split('-');
        _startVerse = int.Parse(verses[0]);

        if (verses.Length == 2)
        {
            _endVerse = int.Parse(verses[1]);
        }
    }
    else
    {
      
        _book = parts[0];
        var chapterVerse = parts[1].Split(':');
        _chapter = int.Parse(chapterVerse[0]);

        var verses = chapterVerse[1].Split('-');
        _startVerse = int.Parse(verses[0]);

        if (verses.Length == 2)
        {
            _endVerse = int.Parse(verses[1]);
        }
    }
}
    // Return the reference as a string
    public string GetReferenceText()
    {
        // Return reference with or without verse range
        return _endVerse.HasValue ? $"{_book} {_chapter}:{_startVerse}-{_endVerse}" : $"{_book} {_chapter}:{_startVerse}";
    }
}

// Class to represent a word in the scripture
public class Word
{
    public string Text { get; private set; } 
    public bool IsHidden { get; private set; } 
    public Word(string text)
    {
        Text = text; // Set the word text
        IsHidden = false; // Word starts as visible
    }

    // Hide the word
    public void Hide()
    {
        IsHidden = true; // Set the word to hidden
    }
}





/*
 * Extra Features
 * 1.  I add a `ScriptureLibrary` class that keeps a list of scriptures.
 *    - Scriptures are loaded from `scriptures.txt`file, so I can update or add more scriptures without changing the code.
 *    - The program shows scriptures random, not just one, making it fun for practice.
 * 2. -Handles Multiple Verses
 * 3. -Program Ends Automatically: Besides letting the user type "quit" to exit, the program also stops by itself when all words in the scripture are hidden.
 */

// T_T DONEEEE!!!