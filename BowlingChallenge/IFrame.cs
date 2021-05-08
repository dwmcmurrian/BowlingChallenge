namespace BowlingChallenge
{
    public interface IFrame
    {
        public int FrameTotal { get; }
        public int[] Rolls { get; }
        public bool HasExtraRoll { get; }
        public bool IsBackFilled { get; }
        public bool IsSpare => FrameTotal == (int) BowlingMarks.Spare;
        public bool IsStrike => FrameTotal == (int) BowlingMarks.Strike;
        public bool IsTurkey => FrameTotal == (int) BowlingMarks.Turkey;


        public void SaveRolls(int[] rolls);
        public void UpdateFrameTotal(int frameTotal);
    }
}