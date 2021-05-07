using BowlingChallenge;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;


namespace BowlingChallengeTests
{
    public class ScoreCardTests
    {
        //may use auto fixture///
        private ScoreCard _scoreCard;

        [SetUp]
        public void Setup()
        {
            _scoreCard = new ScoreCard();
        }

        [Test]
        public void AddFrame_AddingStandardRolls_ShouldAddFrameAndUpdateCurrentStatus()
        {
            //arrange
            var firstRoll = 4;
            var secondRoll = 5;
            var currentScore = 5;
            var expectedFrameSum = firstRoll + secondRoll;
            var currentFameSum = firstRoll + secondRoll + currentScore;

            var frameScore = new int[] { firstRoll, secondRoll };
            var scoreCard = new ScoreCard(currentScore);

            //act
            scoreCard.AddFrame(frameScore);

            //assert
            Assert.AreEqual(1, scoreCard.CurrentFrame);
            Assert.AreEqual(expectedFrameSum, scoreCard.Frames.FirstOrDefault());
            Assert.AreEqual(currentFameSum, scoreCard.CurrentScore);
        }


        [Test]
        public void AddFrame_AddingSpareFrameAfterStrike_ShouldSetPreviousFrameTo20()
        {
            //arrange
            var previousFrameRolls = new int[] { 10, 0 };
            var nextFrameRolls = new int[] { 2, 5 };
            var expectedFrameCount = 2;
            var expectedPreviousFrameSum = 20;
            var currentExpectedSum = 20;
            var scoreCard = new ScoreCard(currentExpectedSum);

            //act
            scoreCard.AddFrame(previousFrameRolls);
            scoreCard.AddFrame(nextFrameRolls);


            //assert
            Assert.AreEqual(expectedFrameCount, scoreCard.CurrentFrame);
            Assert.AreEqual(expectedPreviousFrameSum, scoreCard.Frames[0].GetFrameTotal());
            Assert.AreEqual((int)BowlingMarks.Spare, scoreCard.Frames[1].GetFrameTotal());
            Assert.AreEqual(currentExpectedSum, scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingStrikeAfterSpare_ShouldSetPreviousFrameTo20()
        {
            //arrange
            var previousFrameRolls = new int[] { 1, 9 };
            var nextFrameRolls = new int[] { 10, 0 };
            var expectedFrameCount = 2;
            var expectedPreviousFrameSum = 20;
            var currentExpectedSum = 20;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);


            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrame);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].GetFrameTotal());
            Assert.AreEqual((int)BowlingMarks.Strike, _scoreCard.Frames[1].GetFrameTotal());
            Assert.AreEqual(currentExpectedSum, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingNonMarkAfterSpare_ShouldSetPreviousFrameToTenPlusFirstRoll()
        {
            //arrange
            var previousFrameRolls = new int[] { 1, 9 };
            var nextFrameRolls = new int[] {1, 2};
            var expectedFrameCount = 2;
            var expectedPreviousFrameSum = 10 + nextFrameRolls[0];
            var expectedCurrentFrameSum = nextFrameRolls[0] + nextFrameRolls[1];
            var currentExpectedSum = expectedPreviousFrameSum + nextFrameRolls[1];

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);


            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrame);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].GetFrameTotal());
            Assert.AreEqual(expectedCurrentFrameSum, _scoreCard.Frames[1].GetFrameTotal());
            Assert.AreEqual(currentExpectedSum, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingSpareAfterSpare_ShouldSetPreviousFrameToTenPlusFirstRoll()
        {
            //arrange
            var previousFrameRolls = new int[] { 1, 9 };
            var nextFrameRolls = new int[] { 4, 6 };
            var expectedFrameCount = 2;
            var expectedPreviousFrameSum = 10 + nextFrameRolls[0];
            var currentExpectedSum = expectedPreviousFrameSum ;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);


            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrame);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].GetFrameTotal());
            Assert.AreEqual((int)BowlingMarks.Spare, _scoreCard.Frames[1].GetFrameTotal());
            Assert.AreEqual(currentExpectedSum, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingNonMarkAfterStrike_ShouldSetPreviousFrameToTenPlusFirstAndSecondRoll()
        {
            //arrange
            var previousFrameRolls = new int[] {(int)BowlingMarks.Strike, 0 };
            var nextFrameRolls = new int[] { 1, 2 };
            var expectedFrameCount = 2;
            var expectedCurrentFrameSum = nextFrameRolls[0] + nextFrameRolls[1];
            var expectedPreviousFrameSum = 10 + expectedCurrentFrameSum;
            var currentExpectedSum = expectedPreviousFrameSum + expectedCurrentFrameSum;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrame);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].GetFrameTotal());
            Assert.AreEqual(expectedCurrentFrameSum, _scoreCard.Frames[1].GetFrameTotal());
            Assert.AreEqual(currentExpectedSum, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingMultipleStrikesAfterStrike_ShouldSetPreviousFrameToThirty()
        {
            //arrange
            var firstFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var secondFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var thirdFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var expectedFrameCount = 3;

            //act
            _scoreCard.AddFrame(firstFrameRolls);
            _scoreCard.AddFrame(secondFrameRolls);
            _scoreCard.AddFrame(thirdFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrame);
            Assert.AreEqual((int)BowlingMarks.Turkey, _scoreCard.Frames[0].GetFrameTotal());
            Assert.AreEqual((int)BowlingMarks.Strike, _scoreCard.Frames[1].GetFrameTotal());
            Assert.AreEqual((int)BowlingMarks.Strike, _scoreCard.Frames[2].GetFrameTotal());
            Assert.AreEqual((int)BowlingMarks.Turkey, _scoreCard.CurrentScore);
        }

        //[Test]
        //public void GetCurrentTotal_SingleFrameRoll_ShouldReturnUpdatedValue()
        //{
        //    //arrange
        //    var firstRoll = 4;
        //    var secondRoll = 5;
        //    var currentScore = 5;
        //    var expectedFrameSum = firstRoll + secondRoll;
        //    var currentExpectedSum = firstRoll + secondRoll + currentScore;

        //    var frameScore = new int[] { firstRoll, secondRoll };
        //    var scoreCard = new ScoreCard(currentScore);

        //    //act
        //    scoreCard.AddFrame(frameScore);
        //    var currentSum = scoreCard.GetCurrentTotal();


        //    //assert
        //    Assert.AreEqual(currentFameSum, currentScore);
        //}
    }
}

