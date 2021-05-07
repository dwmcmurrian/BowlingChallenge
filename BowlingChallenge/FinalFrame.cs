namespace BowlingChallenge
{
    public class FinalFrame : IFrame
    {

        public int[] Rolls { get; private set; }
        public bool ExtaRoll { get; private set; }

        public void SaveRolls(int[] rolls)
        {
            var numberOfRoles = rolls.Length;

            if (numberOfRoles == 0 || numberOfRoles > 3) throw new InvalidFrameException();

            Rolls = rolls;

        }

        public int GetFrameTotal()
        {
         
            var sum = 0;
            if (Rolls[0] + Rolls[1] >= (int)BowlingMarks.Strike)
            {
                ExtaRoll = true;

                if (Rolls.Length != 3) throw new InvalidFrameException();
               
            }else if (Rolls.Length == 3)
            {
                throw new InvalidFrameException();
            }

            foreach (var roll in Rolls)
            {
                sum += roll;
            }
            return sum;
        }
    }
}
