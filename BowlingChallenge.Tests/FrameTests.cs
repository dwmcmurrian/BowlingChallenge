using BowlingChallenge;
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
            Assert.AreEqual(firstRoll, _standardFrame.Rolls[0]);
            Assert.AreEqual(secondRoll, _standardFrame.Rolls[1]);
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

            //act
            _standardFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(expectedSum, _standardFrame.FrameTotal);
        }

        [Test]
        public void SaveRolls_RollSumOf10_ShouldReturnSpare()
        {
            //arrange
            var firstRoll = 1;
            var secondRoll = 9;
            var rolls = new int[] { firstRoll, secondRoll };

            //act
            _standardFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(_standardFrame.FrameTotal, (int)BowlingMarks.Spare);
        }

        [Test]
        public void SaveRolls_FirstRollEqualTo10_ShouldReturnStrike()

        {
            var firstRoll = 10;
            var secondRoll = 0;
            var rolls = new int[] { firstRoll, secondRoll };

            //act
            _standardFrame.SaveRolls(rolls); ;

            //assert
            Assert.AreEqual(_standardFrame.FrameTotal, (int)BowlingMarks.Strike);
        }

        [Test]
        public void SaveRolls_RollSumGreaterThan10_ShouldThrowInvalidFrameException()

        {
            var firstRoll = 2;
            var secondRoll = 9;
            var rolls = new int[] { firstRoll, secondRoll };

            //assert
            Assert.Throws<InvalidFrameException>(() => _standardFrame.SaveRolls(rolls));

        }

        [Test]
        public void UpdateFrameTotal_ValidStandardTotal_ShouldUpdateFrameTotal()

        {
            //arrange
            var newFrameValue = 5;

            //act
            _standardFrame.UpdateFrameTotal(newFrameValue);

            //assert
            Assert.AreEqual(newFrameValue, _standardFrame.FrameTotal);
        }
        #endregion

        #region FinalFrame Tests

                [Test]
        public void SaveRolls_ValidNumberOfFinalRolls_ShouldSaveMathcingValues()
        {
            //arrange
            var firstRoll = 1;
            var secondRoll = 9;
            var thirdRoll = 3;
            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };

            //act
            _finalFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(firstRoll, _finalFrame.Rolls[0]);
            Assert.AreEqual(secondRoll, _finalFrame.Rolls[1]);
            Assert.AreEqual(thirdRoll, _finalFrame.Rolls[2]);

        }

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
            var expectedSum = firstRoll + secondRoll + thirdRoll;
            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };

            //act
            _finalFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(expectedSum, _finalFrame.FrameTotal);
        }
         
        [Test]
        public void GetFrameTotal_FinalFrameSpareRolledInFirstTwoFrames_ShouldReturnSumOfThreeRolls()
        {
            // arrange
            var firstRoll = 2;
            var secondRoll = 8;
            var thirdRoll = 2;
            var expectedSum = firstRoll + secondRoll + thirdRoll;
            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };

            //act
            _finalFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(expectedSum, _finalFrame.FrameTotal);
        }

        [Test]
        public void SaveRolls_FinalFrameNonMarkingFirstTwoRows_ShouldReturnSumOfTwoRolls()
        {
            // arrange
            var firstRoll = 2;
            var secondRoll = 6;
            var expectedSum = firstRoll + secondRoll;
            var rolls = new int[] { firstRoll, secondRoll };

            //act
            _finalFrame.SaveRolls(rolls);

            //assert
            Assert.AreEqual(expectedSum, _finalFrame.FrameTotal);
        }

        [Test]
        public void SaveRolls_InvalidThirdRoll_ShouldThrowInvalidFrameException()
        {
            // arrange
            var firstRoll = 2;
            var secondRoll = 3;
            var thirdRoll = 2;

            var rolls = new int[] { firstRoll, secondRoll, thirdRoll };

            //assert
            Assert.Throws<InvalidFrameException>(() => _finalFrame.SaveRolls(rolls));

        }

        [Test]
        public void UpdateFrameTotal_ValidFinalTotal_ShouldUpdateFrameTotal()

        {
            //arrange
            var newFrameValue = 5;

            //act
            _standardFrame.UpdateFrameTotal(newFrameValue);

            //assert
            Assert.AreEqual(newFrameValue, _standardFrame.FrameTotal);
        }



        #endregion


    }
}