using BowlingChallenge;
using NUnit.Framework;
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
            var expectedFrameSum = firstRoll + secondRoll;
            var currentFameSum = firstRoll + secondRoll;

            var frameScore = new int[] { firstRoll, secondRoll };

            //act
            _scoreCard.AddFrame(frameScore);

            //assert
            Assert.AreEqual(1, _scoreCard.CurrentFrameCount);
            Assert.AreEqual(expectedFrameSum, _scoreCard.Frames.FirstOrDefault().Value.FrameTotal);
            Assert.AreEqual(currentFameSum, _scoreCard.CurrentScore);
        }


        [Test]
        public void AddFrame_AddingSpareFrameAfterStrike_ShouldSetPreviousFrameTo20()
        {
            //arrange
            var previousFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var nextFrameRolls = new int[] { 2, 8 };
            var expectedFrameCount = 2;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual((int)BowlingMarks.OppositeMarks, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual((int)BowlingMarks.Spare, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual((int)BowlingMarks.OppositeMarks, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingStrikeAfterSpare_ShouldSetPreviousFrameTo20()
        {
            //arrange
            var previousFrameRolls = new int[] { 1, 9 };
            var nextFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var expectedFrameCount = 2;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);


            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual((int)BowlingMarks.OppositeMarks, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual((int)BowlingMarks.Strike, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual((int)BowlingMarks.OppositeMarks, _scoreCard.CurrentScore);
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
            var currentExpectedSum = expectedPreviousFrameSum + expectedCurrentFrameSum;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);


            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual(expectedCurrentFrameSum, _scoreCard.Frames[1].FrameTotal);
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
            var currentExpectedSum = expectedPreviousFrameSum;
            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);


            //assert
            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual((int)BowlingMarks.Spare, _scoreCard.Frames[1].FrameTotal);
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
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual(expectedPreviousFrameSum, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual(expectedCurrentFrameSum, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual(currentExpectedSum, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingTurekeyFollowedByASpare_ShouldScaleStreakScoreForTheStrikeFields()
        {
            //arrange
            var firstFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var secondFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var thirdFrameRolls = new int[] { (int)BowlingMarks.Strike, 0 };
            var fourthFrameRolls = new int[] { 2, 8 };
            var expectedFrameCount = 4;
            var expectedFrame2Sum = (int)BowlingMarks.OppositeMarks + fourthFrameRolls[0];
            var expectedFrame3Sum = (int)BowlingMarks.OppositeMarks;
            var expectedTotal = (int)BowlingMarks.Turkey + expectedFrame2Sum + expectedFrame3Sum;

            //act
            _scoreCard.AddFrame(firstFrameRolls);
            _scoreCard.AddFrame(secondFrameRolls);
            _scoreCard.AddFrame(thirdFrameRolls);
            _scoreCard.AddFrame(fourthFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual((int)BowlingMarks.Turkey, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual(expectedFrame2Sum, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual(expectedFrame3Sum, _scoreCard.Frames[2].FrameTotal);
            Assert.AreEqual((int)BowlingMarks.Spare, _scoreCard.Frames[3].FrameTotal);
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }
    }
}

