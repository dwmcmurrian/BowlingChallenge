using System.Collections.Generic;
using System.Linq;

namespace BowlingChallenge
{
    public class ScoreCard
    {
        public int CurrentScore { get; set; }
        public int CurrentFrameCount { get; private set; }
        public int PreviousFrameCount => CurrentFrameCount - 1;
        public bool Turkey { get; private set; }
        public List<int> Strikes { get; private set; } = new List<int>();
        public Dictionary<int, IFrame> Frames { get; } = new Dictionary<int, IFrame>();
        public Dictionary<int, int> Scores { get; } = new Dictionary<int, int>();

        public void AddFrame(int[] frameRolls)
        {
            InsertFrame(frameRolls);
            CurrentFrameCount++;
        }

        public void CloseGameOut()
        {
            InsertFrame(new[] { 0, 0 });
            UpdateScore(Frames.Last().Value);
        }

        private IFrame GetCurrentFrameType()
        {
            IFrame currentFrame;

            if (CurrentFrameCount == 9)
                currentFrame = new FinalFrame();
            else
                currentFrame = new StandardFrame();

            return currentFrame;
        }

        private void InsertFrame(int[] frameRolls)
        {
            Turkey = false;
            var currentFrame = GetCurrentFrameType();
            currentFrame.SaveRolls(frameRolls);

            if (currentFrame.HasExtraRoll)
                ProcessFinalFrameScoring(currentFrame);
            else
                AddAndProcessFrames(currentFrame);
        }

        private void AddAndProcessFrames(IFrame currentFrame)
        {
            Frames.Add(CurrentFrameCount, currentFrame);
            Scores.Add(CurrentFrameCount, 0);
            ProcessFrameScoring(currentFrame);
            UpdateScore(currentFrame);
        }

        private void ProcessFinalFrameScoring(IFrame currentFrame)
        {
            var ball1 = currentFrame.Rolls[0];
            var ball2 = currentFrame.Rolls[1];
            var lastFrame = Frames[CurrentFrameCount - 1];
            var secondToLastFrame = Frames[CurrentFrameCount - 2];
            var numberOfStrikes = Strikes.Count;

            if (secondToLastFrame.IsStrike && lastFrame.IsStrike)
            {
                var newSecondToLastTotal = (int) BowlingMarks.Strike + (int) BowlingMarks.Strike + ball1;
                var newLastTotal = (int) BowlingMarks.Strike + ball1 + ball2;
                Frames[CurrentFrameCount - 2].UpdateFrameTotal(newSecondToLastTotal);
                Frames[CurrentFrameCount - 1].UpdateFrameTotal(newLastTotal);
            }
            else if (lastFrame.IsStrike)
            {
                var newLastTotal = (int) BowlingMarks.Strike + ball1 + ball2;
                Frames[CurrentFrameCount - 1].UpdateFrameTotal(newLastTotal);
            }
            else if (lastFrame.IsSpare)
            {
                var newLastTotal = (int) BowlingMarks.Strike + ball1;
                Frames[CurrentFrameCount - 1].UpdateFrameTotal(newLastTotal);
            }

            Frames.Add(CurrentFrameCount, currentFrame);
            Scores.Add(CurrentFrameCount, currentFrame.FrameTotal);
            Strikes.Clear();
            UpdateScore(currentFrame);
        }

        private void UpdateScore(IFrame currentFrame)
        {
            CurrentScore = 0;

            for (var i = 0; i < Frames.Count; i++)
            {
                var frame = Frames[i];

                if (frame.FrameTotal >= (int) BowlingMarks.Strike && !frame.IsBackFilled && i != 9) continue;

                CurrentScore += frame.FrameTotal;
                Scores[i] = CurrentScore;
            }
        }

        private void ProcessFrameScoring(IFrame currentFrame)
        {
            var numberOfStrikes = Strikes.Count();

            if (numberOfStrikes > 0)
            {
                var firstStrikeFrame = Strikes[0];

                var secondStrikeFrame = 0;
                if (currentFrame.FrameTotal != (int) BowlingMarks.Spare)
                {
                    if (currentFrame.FrameTotal == (int) BowlingMarks.Strike)
                    {
                        if (numberOfStrikes < 2)
                        {
                            Strikes.Add(CurrentFrameCount);
                        }
                        else if (numberOfStrikes == 2)
                        {
                            Frames[firstStrikeFrame].UpdateFrameTotal((int) BowlingMarks.Turkey);
                            Turkey = true;
                            Strikes = Strikes.Skip(1).ToList();
                            Strikes.Add(CurrentFrameCount);
                        }
                    }
                    else
                    {
                        if (numberOfStrikes == 1)
                        {
                            Frames[firstStrikeFrame]
                                .UpdateFrameTotal((int) BowlingMarks.Strike + currentFrame.FrameTotal);
                        }
                        else
                        {
                            secondStrikeFrame = Strikes[1];
                            Frames[firstStrikeFrame]
                                .UpdateFrameTotal((int) BowlingMarks.OppositeMarks + currentFrame.Rolls[0]);
                            Frames[secondStrikeFrame]
                                .UpdateFrameTotal((int) BowlingMarks.Strike + currentFrame.FrameTotal);
                        }

                        Strikes.Clear();
                    }
                }
                else
                {
                    if (numberOfStrikes == 1)
                    {
                        Frames[firstStrikeFrame].UpdateFrameTotal((int) BowlingMarks.OppositeMarks);
                    }
                    else
                    {
                        secondStrikeFrame = Strikes[1];
                        Frames[firstStrikeFrame]
                            .UpdateFrameTotal((int) BowlingMarks.OppositeMarks + currentFrame.Rolls[0]);
                        Frames[secondStrikeFrame].UpdateFrameTotal((int) BowlingMarks.OppositeMarks);
                    }

                    Strikes.Clear();
                }
            }
            else
            {
                if (currentFrame.IsStrike)
                {
                    Strikes.Add(CurrentFrameCount);

                    if (Frames.Count <= 1) return;

                    if (Frames[PreviousFrameCount].IsSpare)
                        Frames[PreviousFrameCount].UpdateFrameTotal((int) BowlingMarks.OppositeMarks);
                }
                else if (Frames.Count > 1)
                {
                    if (Frames[PreviousFrameCount].IsSpare)
                        Frames[PreviousFrameCount]
                            .UpdateFrameTotal((int) BowlingMarks.Strike + currentFrame.Rolls[0]);
                    if (Frames[PreviousFrameCount].IsStrike)
                        Frames[PreviousFrameCount]
                            .UpdateFrameTotal((int) BowlingMarks.Strike + currentFrame.FrameTotal);
                }
            }
        }
    }
}