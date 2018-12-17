using System;
using System.Collections.Generic;

namespace Prime_Minister_Game
{
	class Program
	{
		static void Main(string[] args)
		{
			// Welcome the player
			Console.WriteLine("*******************************************");
			Console.WriteLine("*   Welcome to the Prime Minister Game!   *");
			Console.WriteLine("*******************************************");
			Console.WriteLine();
			// Begin the game
			Game.loadPrimeMinisters();
			// Pause to avoid closing
            for (int i = 0; i < 4; i++)
            {
                // Clear the console
                Console.Clear();
                Console.WriteLine("*******************************************");
                Console.WriteLine();
                // Empty the list of already chosen Prime Ministers
                Game.playerOptions.Clear();
                Game.playerOptionsDates.Clear();
                // Load in the new Prime Ministers
                Game.loadPrimeMinisters();
            }
            Console.Clear();
            Console.WriteLine("*******************************************");
            Console.WriteLine();
            // Print out he player score at the end
            Console.WriteLine("Well done! You've scored {0} points!", Player.score);
            Console.WriteLine();
            Console.Write("Would you like to play again? (y/n):");
            // Ask if they'd like to play again and if yes, reset and restart
            if (Console.ReadLine() == "y")
            {
                Console.Clear();
                Player.score = 0;
                Game.alreadySeen.Clear();
                Game.playerOptions.Clear();
                Game.playerOptionsDates.Clear();
                Main(args);
            }
            // Otherwise close the application gracefully
            else
            {
                System.Environment.Exit(1);
            }
        }
	}

	class Game
	{
		// Declare an array to store Prime Ministers in for checking against (stopping duplicates)
		public static List<string> alreadySeen = new List<string>();
        public static List<string> playerOptions = new List<string>();
        public static List<DateTime> playerOptionsDates = new List<DateTime>();

        public static void loadPrimeMinisters()
		{
			// Declare a list for holding the different lines
			List<string> rawData = new List<string>();

			// Try to read the data from the resource
			try
			{
				// Use the csv saved as a resource and split it on each line
				string[] primeMinisters = Properties.Resources.list_of_prime_ministers_of_uk.Split('\n');
				// Loop through each line and add it to the list
				foreach (string person in primeMinisters)
				{
					// Add the line to the list
					rawData.Add(person);
				}
			}
			// Incase there is a problem reading the resource
			catch (Exception e)
			{
				// Print that execption out
				Console.WriteLine("Exception: " + e.Message);
				// Pause the program for the user to ready the exception
				Console.ReadLine();
				// Close the application once a key is pressed
				Environment.Exit(0);
			}

			// Call the function to print them out
			Game.printThree(rawData);
		}

		public static void printThree(List<string> primeMinisters)
		{
			// Declare a random function
			Random rnd = new Random();
			// Print out 3 different Prime Ministers
			for (int i = 0; i < 3; i++)
			{
				// Use the amount of Prime Ministers as the number to count inbetween
				int rand = rnd.Next(primeMinisters.Count);
				// Split the info for the selected person
				string[] personInfo;
				personInfo = primeMinisters[rand].Split(',');

                // If it's already in the array generate a new name to slot in
                while (alreadySeen.Contains(personInfo[1]))
                {
                    // Use the amount of Prime Ministers as the number to count inbetween
                    rand = rnd.Next(primeMinisters.Count);
                    // Split the info for the selected person
                    personInfo = primeMinisters[rand].Split(',');
                }
                // Add the person and their start of term date into arrays for comparing
                alreadySeen.Add(personInfo[1]);
                playerOptions.Add(personInfo[1]);
                playerOptionsDates.Add(DateTime.Parse(personInfo[3]));
                // Print out the options to the user
                Console.WriteLine("{0}: {1}", i + 1, playerOptions[i]);
			}
            // Get the users input
            Console.WriteLine();
            Console.Write("Who served first: ");

            // Let's do some validation, grab the input as a string
            string userRAW = Console.ReadLine();
            
            // Check to see if we need to do some validation on the input
            if (validate(userRAW) == false)
            {
                // We do? Okay, keep looping over the users inputs until they get it right
                while (validate(userRAW) == false)
                {
                    // Be polite, keep asking them to enter a value specified until our validate function returns true
                    Console.WriteLine("\nPlease make sure you are entering an integer between 1 and 3.");
                    Console.Write("Who served first: ");
                    // Read the input again and try to validate like before
                    userRAW = Console.ReadLine();
                }
            }

            // If we've made it this far, it means we can successfuly parse the input into an integer and make the guess
            int theGuess = int.Parse(userRAW) - 1;

            // Now let's check the dates
            bool isFirst = true;
            // Use the guess made here to check the dates against each other
            switch(theGuess)
            {
                // If they chose 1...
                case 0:
                    if (playerOptionsDates[0] > playerOptionsDates[1] || playerOptionsDates[0] > playerOptionsDates[2])
                    {
                        // If the person's start of term date is not the oldest, flag it!
                        isFirst = false;
                    }
                    break;
                // If they chose 2...
                case 1:
                    if (playerOptionsDates[1] > playerOptionsDates[0] || playerOptionsDates[1] > playerOptionsDates[2])
                    {
                        // If the person's start of term date is not the oldest, flag it!
                        isFirst = false;
                    }
                    break;
                // If they chose 3...
                case 2:
                    if (playerOptionsDates[2] > playerOptionsDates[0] || playerOptionsDates[2] > playerOptionsDates[1])
                    {
                        // If the person's start of term date is not the oldest, flag it!
                        isFirst = false;
                    }
                    break;
                // If for some reason we have an exception pass through, keep the flag as is (this shouldn't happen)
                default:
                    isFirst = true;
                    break;
            }
            // Should the player get a point here?
            if (isFirst == true)
            {
                // If so, reach out to the player class and add one to their score
                Player.score++;
            }

        }

        public static bool validate(string userRAW)
        {
            // First let's set a flag to true so was can switch it to false if we encounter an error
            bool flag = true;
            // We also need some other variables for holding the user's guess and a boolean for checking if it's an integer or not
            int userGuess;
            bool isInt = int.TryParse(userRAW, out userGuess);
            // Try to parse the users input into an integer
            isInt = int.TryParse(userRAW, out userGuess);
            // First, let's see if the input was successfully parsed to an integer
            if (isInt == false)
            {
                // If it wasn't set the flag to false!
                flag = false;
            }
            // If it was, great! Now let's see if it's between 1 and 3
            if (userGuess <= 0 || userGuess >= 4)
            {
                // It isn't? Oh dear...better set that flag to false again
                flag = false;
            }
            // Return the value held (true/false) to decide whether we need to ask for a new input or not
            return flag;
        }
	}

	class Player
	{
        // Setup the variable to hold the player score
        public static int score = 0;
	}
}
