using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingChallenge
{
    interface IFrame
    {
        public void SaveRolls(int[] rolls);
        public int GetFrameTotal();
    }
}
