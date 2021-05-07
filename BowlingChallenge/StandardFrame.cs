using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingChallenge
{
    public class StandardFrame : IFrame
    {
        public int[] Rolls { get; private set; }
        public int FrameTotal { get; private set; }


        public void SaveRolls(int[] rolls)
        {
            if (rolls.Length != 2) throw new InvalidFrameException();

            Rolls = rolls;
        }

        public int GetFrameTotal()
        {
            var sum = Rolls[0] + Rolls[1];

            if (Rolls[0] == (int)BowlingMarks.Strike)
            {
                FrameTotal = (int)BowlingMarks.Strike;
                return FrameTotal;
            }
            else if (sum <= (int)BowlingMarks.Strike)
            {
                FrameTotal = (sum + 1) == (int)BowlingMarks.Spare ? (int)BowlingMarks.Spare : sum;
                return FrameTotal;
            }

            throw new InvalidFrameException();
        }

        public void UpdateFrameTotal(int frameTotal)
        {
               FrameTotal = frameTotal;
        }
    }
}