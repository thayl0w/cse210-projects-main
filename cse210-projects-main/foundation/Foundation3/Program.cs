using System;
using System.Collections.Generic;

// Base Activity class
abstract class Activity
{
    private string _date;
    private int _minutes;

    protected Activity(string date, int minutes)
    {
        _date = date;
        _minutes = minutes;
    }

    public int Minutes => _minutes;

    public abstract double GetDistance();
    public abstract double GetSpeed();
    public abstract double GetPace();

    public string GetSummary()
    {
        return $"{_date} ({_minutes} min) - " +
               $"Distance: {GetDistance():0.00} miles, " +
               $"Speed: {GetSpeed():0.00} mph, " +
               $"Pace: {GetPace():0.00} min per mile";
    }
}

// Running derived class
class Running : Activity
{
    private double _distance;

    public Running(string date, int minutes, double distance) 
        : base(date, minutes)
    {
        _distance = distance;
    }

    public override double GetDistance() => _distance;

    public override double GetSpeed() => (_distance / Minutes) * 60;

    public override double GetPace() => Minutes / _distance;
}

// Cycling derived class
class Cycling : Activity
{
    private double _speed;

    public Cycling(string date, int minutes, double speed) 
        : base(date, minutes)
    {
        _speed = speed;
    }

    public override double GetDistance() => (_speed * Minutes) / 60;

    public override double GetSpeed() => _speed;

    public override double GetPace() => 60 / _speed;
}

// Swimming derived class
class Swimming : Activity
{
    private int _laps;

    public Swimming(string date, int minutes, int laps) 
        : base(date, minutes)
    {
        _laps = laps;
    }

    public override double GetDistance() => (_laps * 50) / 1000 * 0.62; // meters to miles conversion

    public override double GetSpeed() => (GetDistance() / Minutes) * 60;

    public override double GetPace() => Minutes / GetDistance();
}

// Program entry
class Program
{
    static void Main()
    {
        List<Activity> activities = new List<Activity>
        {
            new Running("03 Nov 2022", 30, 3.0),
            new Cycling("03 Nov 2022", 30, 12.0),
            new Swimming("03 Nov 2022", 30, 40)
        };

        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}