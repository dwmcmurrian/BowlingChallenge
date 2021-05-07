using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BowlingChallengeTests
{
    public class FrameTests
    {

        //may use auto fixture///
        private StandardFrame _standardFrame;
        private FinalFrame _finalFrame;

        [SetUp]
        public void Setup()
        {
            _standardFrame = new StandardFrame();
            _finalFrame = new FinalFrame();

        }

        #region StandardFrame Tests
        [Test]
        public void SaveRolls_ValidStandardRolls_ShouldSetRollsToMatch()
        {
            //arrange
            var firstRoll = 0;
            var secondRoll = 9;
            var rolls = new int[] { firstRoll, secondRoll };

            //act
            _standardFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(firstRoll, _standardFrame.FirstRoll);
            Assert.AreEqual(secondRoll, _standardFrame.SecondRoll);
        }

        [Test]
        public void SaveRolls_InvalidNumberOfRolls_ShouldThrowInvalidFrameException()
        {
            //arrange
            var firstRoll = 0;
            var secondRoll = 9;
            var thirdRoll = 3;
            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };

            //assert
            Assert.Throws<InvalidFrameException>(() => _standardFrame.SaveRolls(rolls));

        }

        [Test]
        public void SaveRolls_EmptyNumberOfRolls_ShouldThrowInvalidFrameException()
        {
            //arrange
            var rolls = new int[0];

            //assert
            Assert.Throws<InvalidFrameException>(() => _standardFrame.SaveRolls(rolls));

        }

        [Test]
        public void GetFrameTotal_RollSumLessThan10_ShouldReturnSum()
        {
            //arrange
            var firstRoll = 1;
            var secondRoll = 8;
            var expectedSum = firstRoll + secondRoll;

            var rolls = new int[] { firstRoll, secondRoll };
            _standardFrame.SaveRolls(rolls);

            //act
            var total = _standardFrame.GetFrameTotal();

            //assert
            Assert.AreEqual(expectedSum, total);
        }

        [Test]
        public void GetFrameTotal_RollSumOf10_ShouldReturnSpare()
        {
            //arrange
            var firstRoll = 1;
            var secondRoll = 9;
            var rolls = new int[] { firstRoll, secondRoll };
            _standardFrame.SaveRolls(rolls);

            //act
            var total = _standardFrame.GetFrameTotal();

            //assert
            Assert.AreEqual(total, BowlingScores.Spare);
        }

        [Test]
        public void GetFrameTotal_FirstRollEqualTo10_ShouldReturnStrike()

        {
            var firstRoll = 10;
            var secondRoll = 0;
            var rolls = new int[] { firstRoll, secondRoll };
            _standardFrame.SaveRolls(rolls);

            //act
            var total = _standardFrame.GetFrameTotal();

            //assert
            Assert.AreEqual(total, BowlingScores.Strike);
        }

        [Test]
        public void GetFrameTotal_RollSumGreateThan10_ShouldThrowInvalidFrameException()

        {
            var firstRoll = 2;
            var secondRoll = 9;
            var rolls = new int[] { firstRoll, secondRoll };
            _standardFrame.SaveRolls(rolls);

            //assert
            Assert.Throws<InvalidFrameException>(() => _standardFrame.GetFrameTotal());

        }

        #endregion

        #region FinalFrame Tests

        [Test]
        public void SaveRolls_InvalidNumberOfFinalRolls_ShouldThrowInvalidFrameException()
        {
            //arrange
            var firstRoll = 1;
            var secondRoll = 9;
            var thirdRoll = 3;
            var fourthRoll = 4;

            var rolls = new int[] { firstRoll, secondRoll, thirdRoll, fourthRoll };

            //assert
            Assert.Throws<InvalidFrameException>(() => _finalFrame.SaveRolls(rolls));

        }

        [Test]
        public void SaveRolls_EmptyNumberOfFinalRolls_ShouldThrowInvalidFrameException()
        {
            //arrange
            var rolls = new int[0];

            //assert
            Assert.Throws<InvalidFrameException>(() => _finalFrame.SaveRolls(rolls));

        }

        [Test]
        public void GetFrameTotal_FinalFrameStrikeRolledFirst_ShouldReturnSumOfThreeRolls()
        {
            //arrange
            var firstRoll = 10;
            var secondRoll = 8;
            var thirdRoll = 2;

            var expectedSum = firstRoll + secondRoll;//need to update sum
            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };
            _finalFrame.SaveRolls(rolls);

            //act
            var total = _finalFrame.GetFrameTotal();

            //assert
            Assert.AreEqual(expectedSum, total);
        }

        [Test]
        public void GetFrameTotal_FinalFrameSpareRolledInFirstTwoFrames_ShouldReturnSumOfThreeRolls()
        {
           // arrange
            var firstRoll = 2;
            var secondRoll = 8;
            var thirdRoll = 2;

            var expectedSum = firstRoll + secondRoll;//need to update sum
            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };
            _finalFrame.SaveRolls(rolls);

            //act
            var total = _finalFrame.GetFrameTotal();

            //assert
            Assert.AreEqual(expectedSum, total);
        }

        [Test]
        public void GetFrameTotal_FinalFrameNonMarkingFirstTwoRows_ShouldReturnSumOfTwoRolls()
        {
            // arrange
            var firstRoll = 2;
            var secondRoll = 6;

            var expectedSum = firstRoll + secondRoll;
            var rolls = new int[] { firstRoll, secondRoll };
            _finalFrame.SaveRolls(rolls);

            //act
            var total = _finalFrame.GetFrameTotal();

            //assert
            Assert.AreEqual(expectedSum, total);
        }

        [Test]
        public void GetFrameTotal_InvalidThirdRoll_ShouldThrowInvalidFrameException()
        {
            // arrange
            var firstRoll = 2;
            var secondRoll = 3;
            var thirdRoll = 2;

            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };
            _finalFrame.SaveRolls(rolls);

            //assert
            Assert.Throws<InvalidFrameException>(() => _finalFrame.GetFrameTotal(rolls));

        }





        #endregion


    }
}