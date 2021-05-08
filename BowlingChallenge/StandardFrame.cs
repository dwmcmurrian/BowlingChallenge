using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingChallenge
{
    public class StandardFrame : IFrame
    {
        public int[] Rolls { get; private set; }
        public int FrameTotal { get; private set; }
        public bool IsBackFilled { get; private set; }

        public void SaveRolls(int[] rolls)
        {
            if (rolls.Length != 2) throw new InvalidFrameException();

            Rolls = rolls;
            SetFrameTotal();
        }

        private void SetFrameTotal()
        {
            var sum = Rolls[0] + Rolls[1];

            if (Rolls[0] == (int)BowlingMarks.Strike)
            {
                FrameTotal = (int)BowlingMarks.Strike;
            }
            else if (sum <= (int)BowlingMarks.Strike)
            {
                FrameTotal = (sum + 1) == (int)BowlingMarks.Spare ? (int)BowlingMarks.Spare : sum;
            }
            else
            {
                throw new InvalidFrameException();
            }
        }

        public void UpdateFrameTotal(int frameTotal)
        {
            IsBackFilled = true;
            FrameTotal = frameTotal;
        }
    }
}