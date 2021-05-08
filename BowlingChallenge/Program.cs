using System;

namespace BowlingChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int[][] jaggedArray2 = new int[][]
            {
                new int[] { 0, 2,},
                new int[] { 5, 4 },
                new int[] { 3, 7,},
                new int[] { 7, 1 },
                new int[] {10, 0},
                new int[] { 10, 0 },
                new int[] { 3, 2,},
                new int[] { 2, 4 },
             };

            var scoreCard = new ScoreCard();
            for( var i = 0; i< jaggedArray2.Length; i++)
            {
                var array = jaggedArray2[i];

                Console.WriteLine($"roll 1: {array[0]}\n roll 2: {array[1]}\n");
                scoreCard.AddFrame(array);
                Console.WriteLine($"FrameTotal: {scoreCard.Scores[i]}\n");

            }
           
        }
    }
}
