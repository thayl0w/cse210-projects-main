using System;
using System.Collections.Generic;
using System.IO;


// Base class for all goal types
public abstract class Goal
{
    protected string _shortName;
    protected string _description;
    protected int _points;

    public Goal(string name, string description, int points)
    {
        _shortName = name;
        _description = description;
        _points = points;
    }

    public abstract void RecordEvent();  // Tracks completion or progress of the goal
    public abstract bool IsComplete(); // Checks if the goal is fully completed
    public virtual string GetDetailsString()
    {
        return $"[{(IsComplete() ? "X" : " ")}] {_shortName}: {_description}";
    }

    // Returns basic details about the goal, showing [X] if complete
    public abstract string GetStringRepresentation();

    public int GetPoints() => _points;
    public string ShortName => _shortName;
}
// Class for simple one-time goals
public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points) : base(name, description, points)
    {
        _isComplete = false;
    }

    public override void RecordEvent()
    {
        if (!_isComplete) _isComplete = true;
    }

    public override bool IsComplete() => _isComplete;
    public override string GetStringRepresentation() => $"SimpleGoal,{_shortName},{_description},{_points},{_isComplete}";
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points) : base(name, description, points) {}

    public override void RecordEvent() {}
    public override bool IsComplete() => false;
    public override string GetStringRepresentation() => $"EternalGoal,{_shortName},{_description},{_points}";
}

public class ChecklistGoal : Goal
{
    private int _amountCompleted; // Tracks times completed

    private int _target;  // Target completion count
    private int _bonus;  // Extra points when target is reached

    public ChecklistGoal(string name, string description, int points, int target, int bonus)
        : base(name, description, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }

    public override void RecordEvent()
    {
        if (_amountCompleted < _target) _amountCompleted++;
    }

    public override bool IsComplete() => _amountCompleted >= _target;
    public override string GetDetailsString() => base.GetDetailsString() + $" ({_amountCompleted}/{_target})";
    public override string GetStringRepresentation() => $"ChecklistGoal,{_shortName},{_description},{_points},{_amountCompleted},{_target},{_bonus}";

    public int GetBonus() => IsComplete() ? _bonus : 0;
}

public class NegativeGoal : Goal
{
    public NegativeGoal(string name, string description, int points) : base(name, description, points) {}

    public override void RecordEvent() {}
    public override bool IsComplete() => false;
    public override string GetStringRepresentation() => $"NegativeGoal,{_shortName},{_description},{_points}";
}

public class ProgressGoal : Goal
{
    private int _currentProgress;
    private int _goalProgress;

    public ProgressGoal(string name, string description, int points, int goalProgress)
        : base(name, description, points)
    {
        _currentProgress = 0;
        _goalProgress = goalProgress;
    }

    public override void RecordEvent()
    {
        if (_currentProgress < _goalProgress) _currentProgress++;
    }

    public override bool IsComplete() => _currentProgress >= _goalProgress;
    public override string GetDetailsString() => base.GetDetailsString() + $" ({_currentProgress}/{_goalProgress})";
    public override string GetStringRepresentation() => $"ProgressGoal,{_shortName},{_description},{_points},{_currentProgress},{_goalProgress}";
}

public class Player
{
    private string _name;
    private int _score;
    private int _level;
    private Dictionary<string, int> _goalStreaks;

    public Player(string name)
    {
        _name = name;
        _score = 0;
        _level = 1;
        _goalStreaks = new Dictionary<string, int>();
    }

    public string Name => _name;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            LevelUp();
        }
    }
    public int Level => _level;

    private void LevelUp()
    {
        if (_score >= 100) _level = 2;
        if (_score >= 300) _level = 3;
        if (_score >= 600) _level = 4;
    }

    public string GetPlayerInfo() => $"Player: {_name}, Score: {_score} points, Level: {_level}";

    public void RecordGoal(string goalName)
    {
        if (_goalStreaks.ContainsKey(goalName)) _goalStreaks[goalName]++;
        else _goalStreaks[goalName] = 1;

        Console.WriteLine($"Streak for {goalName}: {_goalStreaks[goalName]} days in a row!");

        if (_goalStreaks[goalName] % 5 == 0)
        {
            Console.WriteLine("You've reached a streak milestone! Extra points awarded.");
            Score += 50;
        }
    }
}

public class GoalManager
{
    private List<Goal> _goals;
    private Player _player;
    private static GoalManager _instance;

    public static GoalManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Console.Write("Enter your name: ");
                string playerName = Console.ReadLine();
                _instance = new GoalManager(new Player(playerName));
            }
            return _instance;
        }
    }

    private GoalManager(Player player)
    {
        _goals = new List<Goal>();
        _player = player;
    }

    public void Start()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n1. Display Player Info\n2. List Goals\n3. Create Goal\n4. Record Event\n5. Save Goals\n6. Load Goals\n7. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayPlayerInfo();
                    break;
                case "2":
                    ListGoalDetails();
                    break;
                case "3":
                    CreateGoal();
                    break;
                case "4":
                    RecordEvent();
                    break;
                case "5":
                    Console.Write("Enter filename to save goals: ");
                    string saveFile = Console.ReadLine();
                    SaveGoals(saveFile);
                    break;
                case "6":
                    Console.Write("Enter filename to load goals: ");
                    string loadFile = Console.ReadLine();
                    LoadGoals(loadFile);
                    break;
                case "7":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    public void DisplayPlayerInfo() => Console.WriteLine($"\n{_player.GetPlayerInfo()}");

    public void ListGoalDetails()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("\nNo goals available.");
            return;
        }
        Console.WriteLine("\nGoals:");
        for (int i = 0; i < _goals.Count; i++)
            Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
    }

    public void CreateGoal()
    {
        Console.WriteLine("\n1. Simple Goal\n2. Eternal Goal\n3. Checklist Goal\n4. Negative Goal\n5. Progress Goal");
        Console.Write("Choose a goal type: ");
        string choice = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();
        Console.Write("Enter goal description: ");
        string description = Console.ReadLine();
        Console.Write("Enter goal points: ");
        int points = int.Parse(Console.ReadLine());

        Goal newGoal = choice switch
        {
            "1" => new SimpleGoal(name, description, points),
            "2" => new EternalGoal(name, description, points),
            "3" => new ChecklistGoal(name, description, points, int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine())),
            "4" => new NegativeGoal(name, description, points),
            "5" => new ProgressGoal(name, description, points, int.Parse(Console.ReadLine())),
            _ => null
        };

        if (newGoal != null)
        {
            _goals.Add(newGoal);
            Console.WriteLine("Goal created successfully.");
        }
    }

    public void RecordEvent()
    {  
           // Allows user to record a goal completion and update score
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available to record.");
            return;
        }
        Console.WriteLine("\nWhich goal did you accomplish?");
        ListGoalDetails();
        int goalIndex = int.Parse(Console.ReadLine()) - 1;

        if (goalIndex < 0 || goalIndex >= _goals.Count)
        {
            Console.WriteLine("Invalid goal selection.");
            return;
        }

        Goal goal = _goals[goalIndex];
        goal.RecordEvent();
        int pointsEarned = goal.GetPoints();
        _player.Score += pointsEarned;

        if (goal is ChecklistGoal checklistGoal && checklistGoal.IsComplete())
            pointsEarned += checklistGoal.GetBonus();

        _player.RecordGoal(goal.ShortName);
        Console.WriteLine($"Recorded accomplishment for goal '{goal.ShortName}', earned {pointsEarned} points.");
    }

    public void SaveGoals(string filename)
    {
        using StreamWriter writer = new StreamWriter(filename);
        writer.WriteLine(_player.Name);
        writer.WriteLine(_player.Score);
        writer.WriteLine(_player.Level);

        foreach (Goal goal in _goals)
            writer.WriteLine(goal.GetStringRepresentation());

        Console.WriteLine("Goals saved successfully.");
    }

    public void LoadGoals(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        using StreamReader reader = new StreamReader(filename);
        _player = new Player(reader.ReadLine());
        _player.Score = int.Parse(reader.ReadLine());

        _goals = new List<Goal>();
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(',');
            Goal goal = parts[0] switch
            {
                "SimpleGoal" => new SimpleGoal(parts[1], parts[2], int.Parse(parts[3])),
                "EternalGoal" => new EternalGoal(parts[1], parts[2], int.Parse(parts[3])),
                "ChecklistGoal" => new ChecklistGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5])),
                "NegativeGoal" => new NegativeGoal(parts[1], parts[2], int.Parse(parts[3])),
                "ProgressGoal" => new ProgressGoal(parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4])),
                _ => null
            };

            if (goal != null) _goals.Add(goal);
        }

        Console.WriteLine("Goals loaded successfully.");
    }
}

class Program
{
    static void Main(string[] args) => GoalManager.Instance.Start();
}


/*
 * 1. Gamification:
 *    - Added goal streak tracking. Users earn extra points for completing goals in a row.
 * 
 * 2. New Goals:
 *    - Negative goals let users lose points for bad habits.
 *    - Progress goals reward users for working towards bigger goals.
 * 
 * 3. User Interaction:
 *    - Clear menu to choose goal types.
 *    - Shows goal completion status for easy tracking.
 * 
 */
