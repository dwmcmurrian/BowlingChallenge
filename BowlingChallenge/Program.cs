using System;
using System.Collections.Generic;

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

            Console.WriteLine("\n\nHello, Would you LIKE to play A game! o_O\n\n");

            var userInput = "";
            var scoreCard = new ScoreCard();
            var maxValue = 10;
            var maxFrames = 10;

            var list = new List<List<int>>();
            for (var i = 0; i < maxFrames; i++)
            {
                var frameSum = 10;
                var rollCount = 1;
                var currentRolls = new List<int>();
                var maxRolls = 3;

                do
                {
                    Console.WriteLine($"\nFrame: {i + 1} - Roll: {rollCount}:");
                    Console.WriteLine($"\nPlease enter a value: 0 - {frameSum} (you can also enter 'stop' to exit: ");
                    userInput = Console.ReadLine(); //need to add roll 1
                    var numericalEntry = int.TryParse(userInput, out var userInt);
                    if (numericalEntry && userInt <= frameSum)
                    {
                        currentRolls.Add(userInt);
                        frameSum -= userInt;

                        if (i == 9 && rollCount <= 2 && frameSum == 0) maxRolls++;
                        rollCount++;
                    }
                } while (rollCount < maxRolls && userInput.ToLower() != "stop");

                list.Add(currentRolls);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rolls = string.Join(",", list[i]);
                Console.WriteLine($"\nRolls: {rolls}");
                scoreCard.AddFrame(list[i].ToArray());
                Console.WriteLine($"FrameTotal: {scoreCard.Scores[i]}\n");
            }
        }
    }
}