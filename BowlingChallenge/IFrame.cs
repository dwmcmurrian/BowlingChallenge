using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingChallenge
{
    public interface IFrame
    {
        public int FrameTotal { get; }

        public void SaveRolls(int[] rolls);
        public void UpdateFrameTotal(int frameTotal);
    }
}
