using System;

class Program
{
    static void Main()
    {
        bool playAgain = true;

        while (playAgain)
        {
            Random rand = new Random();
            int secretNumber = rand.Next(1, 101);
            int guess = 0;
            int attempts = 0;

            Console.WriteLine("Guess the secret number:");

            while (guess != secretNumber)
            {
                Console.Write("What is your guess: ");
                guess = int.Parse(Console.ReadLine());

                attempts += 1;

                if (guess < secretNumber)
                {
                    Console.WriteLine("Higher!");
                }
                else if (guess > secretNumber)
                {
                    Console.WriteLine("Lower!");
                }
            }

            Console.WriteLine("You guessed it!");
            Console.WriteLine($"You guessed it in {attempts} tries.");

            Console.Write("Do you want to play again? (yes/no): ");
            string response = Console.ReadLine().Trim().ToLower();
            if (response == "yes")
            {
                playAgain = true;
            }
            else
            {
                playAgain = false;
            }
            Console.WriteLine();
        }

        Console.WriteLine("Thanks for playing!");
    }
}
