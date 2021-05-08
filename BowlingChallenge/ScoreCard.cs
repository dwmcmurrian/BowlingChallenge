using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingChallenge
{
    //  • List<frames>
    //	• Int current frame;
    //	• current Score

    public class ScoreCard
    {
        public ScoreCard()
        {
        }


        public int CurrentScore { get; set; }
        public int CurrentFrameCount { get; private set; } = 0;
        public List<int> Strikes { get; private set; } = new List<int>();
        public Dictionary<int, IFrame> Frames { get; private set; } = new Dictionary<int, IFrame>(); //Frame number number and frame
        public Dictionary<int, int> Scores { get; private set; } = new Dictionary<int, int>(); //Frame number number and frame
        //public List<IFrame> Frames { get; private set; } = new List<IFrame>();

        public void AddFrame(int[] framerolls)
        {
            InsertFrame(framerolls);
            CurrentFrameCount++;
        }

        private void InsertFrame(int[] framerolls)
        {
            var currentFrame = GetCurrentFrameType();
            currentFrame.SaveRolls(framerolls);

            Frames.Add(CurrentFrameCount, currentFrame);
            Scores.Add(CurrentFrameCount, 0);

            var numberOfStrikes = Strikes.Count();
            var secondStrikeFrame = 0;

            if (numberOfStrikes > 0)
            {
                var firstStrikeFrame = Strikes[0];
                
                if (currentFrame.FrameTotal == (int)BowlingMarks.Spare)
                {
                    if (numberOfStrikes == 1)
                    {
                        Frames[firstStrikeFrame].UpdateFrameTotal((int)BowlingMarks.OppositeMarks);
                    }
                    else
                    {
                        secondStrikeFrame = Strikes[1];
                        Frames[firstStrikeFrame].UpdateFrameTotal((int)BowlingMarks.OppositeMarks + currentFrame.Rolls[0]);
                        Frames[secondStrikeFrame].UpdateFrameTotal((int)BowlingMarks.OppositeMarks);
                    }

                    Strikes.Clear();
                }
                else if (currentFrame.FrameTotal == (int)BowlingMarks.Strike)
                {
                    if (numberOfStrikes < 2)
                    {
                        Strikes.Add(CurrentFrameCount);
                    }
                    else  if(numberOfStrikes == 2)
                    {
                        Frames[firstStrikeFrame].UpdateFrameTotal((int)BowlingMarks.Turkey);
                        Strikes = Strikes.Skip(1).ToList();
                        Strikes.Add(CurrentFrameCount);
                    }

                }
                else //non marking frame
                {
                    if (numberOfStrikes == 1)
                    {
                        Frames[firstStrikeFrame].UpdateFrameTotal((int)BowlingMarks.Spare - 1 + currentFrame.FrameTotal);
                    }
                    else
                    {
                        secondStrikeFrame = Strikes[1];
                        Frames[firstStrikeFrame].UpdateFrameTotal((int)BowlingMarks.OppositeMarks + currentFrame.Rolls[0]);
                        Frames[secondStrikeFrame].UpdateFrameTotal((int)BowlingMarks.Spare - 1 + currentFrame.FrameTotal);
                    }

                    Strikes.Clear();

                }
            }
            else
            {
                if (currentFrame.FrameTotal == (int)BowlingMarks.Strike)
                {
                    Strikes.Add(CurrentFrameCount);

                    if(Frames.Count > 1) {
                        if (Frames[CurrentFrameCount - 1].FrameTotal == (int)BowlingMarks.Spare)
                        {
                            Frames[CurrentFrameCount - 1].UpdateFrameTotal((int)BowlingMarks.OppositeMarks);

                        }
                    }
                    
                }
                else if (Frames.Count > 1)
                {

                    if ( Frames[CurrentFrameCount - 1].FrameTotal == (int)BowlingMarks.Spare)
                    {
                        Frames[CurrentFrameCount - 1].UpdateFrameTotal((int)BowlingMarks.Spare - 1 + currentFrame.Rolls[0]);
                    }
                    if (Frames[CurrentFrameCount - 1].FrameTotal == (int)BowlingMarks.Strike)
                    {
                        Frames[CurrentFrameCount - 1].UpdateFrameTotal((int)BowlingMarks.Spare - 1 + currentFrame.FrameTotal);
                    }
                }
            }

            UpdateScore(currentFrame);
        }

        private void UpdateScore(IFrame currentFrame)
        {
            CurrentScore = 0;

            for (int i = 0; i < Frames.Count; i++)
            {
                var frame = Frames[i];

                if (frame.FrameTotal < (int)BowlingMarks.Strike || frame.IsBackFilled)
                {
                    CurrentScore += frame.FrameTotal;
                    Scores[i] = CurrentScore;
                }
            }
        }

      
        public IFrame GetCurrentFrameType()
        {
            IFrame currentFrame;

            if (CurrentFrameCount == 10)
            {
                currentFrame = new FinalFrame();
            }
            else
            {
                currentFrame = new StandardFrame();
            }

            return currentFrame;
        }
    }
}
