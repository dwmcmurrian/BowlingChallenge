namespace BowlingChallenge
{
    public class FinalFrame : IFrame
    {
        public bool HasExtraRoll { get; private set; }
        public bool IsBackFilled { get; private set; }
        public int[] Rolls { get; private set; }
        public int FrameTotal { get; private set; }

        public void SaveRolls(int[] rolls)
        {
            var numberOfRoles = rolls.Length;

            if (numberOfRoles == 0 || numberOfRoles > 3) throw new InvalidFrameException();

            Rolls = rolls;
            SetFrameTotal();
        }

        public void UpdateFrameTotal(int frameTotal)
        {
            IsBackFilled = true;
            FrameTotal = frameTotal;
        }

        private void SetFrameTotal()
        {
            var sum = 0;
            if (Rolls[0] + Rolls[1] >= (int) BowlingMarks.Strike)
                HasExtraRoll = true;
            else if (Rolls.Length == 3) throw new InvalidFrameException();

            foreach (var roll in Rolls) sum += roll;

            FrameTotal = sum;
        }
    }
}