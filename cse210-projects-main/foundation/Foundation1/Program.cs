using System;
using System.Collections.Generic;

public class Video
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Length { get; private set; } // Length in seconds
    private List<Comment> _comments;

    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        _comments = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    public void DisplayVideoDetails()
    {
        Console.WriteLine($"Title: {Title}, Author: {Author}, Length: {Length / 60} minutes {Length % 60} seconds");
        Console.WriteLine($"Number of Comments: {GetNumberOfComments()}");
        foreach (var comment in _comments)
        {
            comment.DisplayComment();
        }
        Console.WriteLine();
    }
}

public class Comment
{
    public string CommenterName { get; private set; }
    public string Text { get; private set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }

    public void DisplayComment()
    {
        Console.WriteLine($"{CommenterName}: {Text}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Video 1: Guitar Chords
        Video video1 = new Video("Beginner's Guide to Guitar Chords", "GuitarGuru", 615);
        video1.AddComment(new Comment("CongTv", "This video is perfect for beginners! I learned so much."));
        video1.AddComment(new Comment("Mr.Beast", "Struggling with the F chord but this really helped!"));
        video1.AddComment(new Comment("Speed", "Great explanations, especially the transitions between chords."));
        video1.AddComment(new Comment("Elon Musk", "Can you make a video on fingerpicking techniques next?"));

        // Video 2: Piano Scales
        Video video2 = new Video("Mastering Piano Scales in 10 Minutes", "PianoPro", 465);
        video2.AddComment(new Comment("Bulldog", "This was super helpful! My scales are smoother now."));
        video2.AddComment(new Comment("Chihuahua", "I wish I had this when I first started playing!"));
        video2.AddComment(new Comment("Aspin", "Quick and easy tutorial. Thanks!"));
        video2.AddComment(new Comment("Bulldet", "Could you do a video on jazz scales next?"));

        // Video 3: Fingerstyle Guitar
        Video video3 = new Video("Top 5 Tips for Fingerstyle Guitar", "StrumKing", 500);
        video3.AddComment(new Comment("Joy", "Finally getting better at fingerstyle! Thanks!"));
        video3.AddComment(new Comment("Sadness", "Your tip about thumb positioning was a game changer!"));
        video3.AddComment(new Comment("Disgust", "Can’t wait to try these out. Awesome content."));
        video3.AddComment(new Comment("Anger", "Would love a follow-up video on advanced techniques."));

        // Add videos to the list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display details for each video
        foreach (var video in videos)
        {
            video.DisplayVideoDetails();
        }
    }
}
