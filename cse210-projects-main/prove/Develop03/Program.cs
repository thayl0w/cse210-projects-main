using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        // Load scriptures from a file
        List<Scripture> scriptures = LoadScripturesFromFile("scriptures.txt");
        
        if (scriptures.Count == 0)
        {
            Console.WriteLine("No scriptures found in the file.");
            return;
        }

        // Choose a random scripture to display
        Random random = new Random();
        Scripture selectedScripture = scriptures[random.Next(scriptures.Count)];

        // Display the selected scripture
        Console.Clear();
        selectedScripture.Display();

        while (true)
        {
            Console.WriteLine("Press Enter to hide words or type 'quit' to exit.");
            string input = Console.ReadLine();

            if (input?.ToLower() == "quit")
                break;

            if (!selectedScripture.HideRandomWords(3))
            {
                Console.WriteLine("All words are hidden. Well done!");
                break;
            }

            Console.Clear();
            selectedScripture.Display();
        }
    }

    // Method to load scriptures from a file
    private static List<Scripture> LoadScripturesFromFile(string filePath)
    {
        List<Scripture> scriptures = new List<Scripture>();

        try
        {
            foreach (string line in File.ReadLines(filePath))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 2)
                {
                    scriptures.Add(new Scripture(parts[0], parts[1]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading scriptures: {ex.Message}");
        }

        return scriptures;
    }
}

// Class to represent a scripture with a reference and text
public class Scripture
{
    private Reference _reference; // Holds reference details (book, chapter, verse)
    private List<Word> _words; // List of words in the scripture
    private Random _random = new Random(); // Random number generator for hiding words

    public Scripture(string referenceText, string text)
    {
        _reference = new Reference(referenceText);
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    // Display the scripture
    public void Display()
    {
        Console.WriteLine(_reference.ToString());

        foreach (Word word in _words)
        {
            Console.Write(word.IsHidden ? "___ " : word.Text + " ");
        }

        Console.WriteLine(); // New line
    }

    // Randomly hide words and return false if all words are hidden
    public bool HideRandomWords(int count)
    {
        List<Word> visibleWords = _words.Where(word => !word.IsHidden).ToList();

        if (visibleWords.Count == 0)
            return false;

        for (int i = 0; i < count && visibleWords.Count > 0; i++)
        {
            Word wordToHide = visibleWords[_random.Next(visibleWords.Count)];
            wordToHide.Hide();
            visibleWords.Remove(wordToHide);
        }

        return true;
    }
}

// Class to represent the reference of a scripture
public class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int? _endVerse;

    public Reference(string referenceText)
    {
        ParseReference(referenceText);
    }

    // Parse the reference text
    private void ParseReference(string referenceText)
    {
        string[] parts = referenceText.Split(' ');

        if (int.TryParse(parts[0], out _))
        {
            _book = $"{parts[0]} {parts[1]}";
            ParseChapterAndVerse(parts[2]);
        }
        else
        {
            _book = parts[0];
            ParseChapterAndVerse(parts[1]);
        }
    }

    private void ParseChapterAndVerse(string chapterVerse)
    {
        string[] chapterParts = chapterVerse.Split(':');
        _chapter = int.Parse(chapterParts[0]);

        string[] verseParts = chapterParts[1].Split('-');
        _startVerse = int.Parse(verseParts[0]);

        if (verseParts.Length == 2)
        {
            _endVerse = int.Parse(verseParts[1]);
        }
    }

    public override string ToString()
    {
        return _endVerse.HasValue ? $"{_book} {_chapter}:{_startVerse}-{_endVerse}" : $"{_book} {_chapter}:{_startVerse}";
    }
}

// Class to represent a single word in the scripture
public class Word
{
    public string Text { get; private set; }
    public bool IsHidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        IsHidden = false;
    }

    public void Hide()
    {
        IsHidden = true;
    }
}