using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingChallenge
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(@"
 ____                                                           
/\  _`\                                                         
\ \ \/\ \  __  __  __     __     __  __    ___      __    ____  
 \ \ \ \ \/\ \/\ \/\ \  /'__`\  /\ \/\ \ /' _ `\  /'__`\ /',__\ 
  \ \ \_\ \ \ \_/ \_/ \/\ \L\.\_\ \ \_\ \/\ \/\ \/\  __//\__, `\
   \ \____/\ \___x___/'\ \__/.\_\\/`____ \ \_\ \_\ \____\/\____/
    \/___/  \/__//__/   \/__/\/_/ `/___/> \/_/\/_/\/____/\/___/ 
                                     /\___/                     
                                     \/__/                      
 ____                         ___                                                 __     
/\  _`\                      /\_ \                                               /\ \    
\ \ \L\ \    ___   __  __  __\//\ \      __     _ __    __      ___ ___      __  \ \ \   
 \ \  _ <'  / __`\/\ \/\ \/\ \ \ \ \   /'__`\  /\`'__\/'__`\  /' __` __`\  /'__`\ \ \ \  
  \ \ \L\ \/\ \L\ \ \ \_/ \_/ \ \_\ \_/\ \L\.\_\ \ \//\ \L\.\_/\ \/\ \/\ \/\ \L\.\_\ \_\ 
   \ \____/\ \____/\ \___x___/' /\____\ \__/.\_\\ \_\\ \__/.\_\ \_\ \_\ \_\ \__/.\_\\/\_\
    \/___/  \/___/  \/__//__/   \/____/\/__/\/_/ \/_/ \/__/\/_/\/_/\/_/\/_/\/__/\/_/ \/_/
                                                                                         
");
            do
            {
                Console.WriteLine("\n\nHello, Would you LIKE to play A game! o_O\n\n");

                var scoreCard = new ScoreCard();
                var list = new List<List<int>>();
                try
                {
                    CollectUserFrames(list);
                    TallyScoreCard(list, scoreCard);
                }
                catch (InvalidFrameException)
                {
                    Console.WriteLine("An invalid frame was entered");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown error: {ex.InnerException}");
                }

                Console.WriteLine("Hit any key to run again");
                Console.ReadKey();
            } while (true);
        }

        private static void TallyScoreCard(List<List<int>> list, ScoreCard scoreCard)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var rolls = string.Join(",", list[i]);
                Console.WriteLine($"\nRolls: {rolls}");
                scoreCard.AddFrame(list[i].ToArray());
                Console.WriteLine($"FrameTotal: {scoreCard.Scores[i]}\n");
            }
        }

        private static void CollectUserFrames(List<List<int>> list)
        {
            const int maxFrames = 10;

            for (var i = 0; i < maxFrames; i++)
            {
                var rollCount = 1;
                var currentRolls = new List<int>();
                var maxRolls = 3;
                var enableExtraFrame = false;

                var userInput = "";
                do
                {
                    if (CollectUserStandardFrames(i, currentRolls, ref rollCount, out userInput, ref enableExtraFrame))
                        break;
                } while (userInput != null && rollCount < maxRolls && userInput.ToLower() != "stop");

                if (userInput != null && userInput.ToLower() == "stop") break;

                if (enableExtraFrame)
                {
                    if (CollectUserExtraFrame(currentRolls)) break;
                }
                else
                {
                    if (currentRolls.Count < 2)
                        for (var c = currentRolls.Count - 1; c < 2; c++)
                            currentRolls.Add(0);
                }

                list.Add(currentRolls);
            }
        }

        private static bool CollectUserExtraFrame(List<int> currentRolls)
        {
            string userInput;
            Console.WriteLine(
                "\nPlease enter a bonus Frame value between  0 - 10, or press enter to " +
                "continue.: ");
            userInput = Console.ReadLine();
            var numericalEntry = int.TryParse(userInput, out var userInt);
            if (userInput != null && userInput.ToLower() == "stop") return true;

            // if (!numericalEntry || userInt > 10) continue;

            currentRolls.Add(userInt);

            if (currentRolls.Count < 3)
                for (var c = currentRolls.Count - 1; c < 3; c++)
                    currentRolls.Add(0);
            return false;
        }

        private static bool CollectUserStandardFrames(int i, List<int> currentRolls, ref int rollCount,
            out string userInput,
            ref bool enableExtraFrame)
        {
            Console.WriteLine($"\nFrame: {i + 1} - Roll: {rollCount}:");
            Console.WriteLine("\nPlease enter a value: 0 - 10. (you can also enter 'stop' to exit): ");
            userInput = Console.ReadLine(); //need to add roll 1
            var numericalEntry = int.TryParse(userInput, out var userInt);
            if (userInput != null && userInput.ToLower() == "stop") return true;

            if (!numericalEntry || userInt > 10) return false;

            currentRolls.Add(userInt);

            if (i == 9)
                if (currentRolls.Sum() >= 10) //9th frame, first roll and strike
                    enableExtraFrame = true;
            rollCount++;
            return false;
        }
    }
}