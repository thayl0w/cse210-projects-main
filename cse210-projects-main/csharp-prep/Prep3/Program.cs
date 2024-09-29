using System;

class Program
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)

        {
            Random randomGenerator = new Random();
            int magicNumber = randomGenerator.Next(1,101);

            int guess = -1;
            int numberOfGuess = 0;

            while (guess != magicNumber)
            {
                Console.Write("What is your guess number? ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out guess) || guess < 1 || guess > 100)
                {
                    Console.WriteLine("Please enter a valid number between 1-100.");
                    continue;
                }

                numberOfGuess++;

                if (magicNumber > guess)
                {
                    Console.WriteLine("Higher");
                }
                else if (magicNumber < guess)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine($"You guess it in {numberOfGuess} guesses !");
                }       
            }
        
            Console.Write("Do you want to play again? (yes/no): ");
            string response = Console.ReadLine().ToLower();

            if (response != "yes")
            {
                playAgain = false;
                Console.WriteLine("Thank you for playing! Goodbye!");
            }
        }
    }
}