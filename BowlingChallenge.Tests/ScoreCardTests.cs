using BowlingChallenge;
using NUnit.Framework;
using System.Linq;

namespace BowlingChallengeTests
{
    public class ScoreCardTests
    {
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

            var frameScore = new[] { firstRoll, secondRoll };

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
            var previousFrameRolls = new[] { (int) BowlingMarks.Strike, 0 };
            var nextFrameRolls = new[] { 2, 8 };
            var expectedFrameCount = 2;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual((int) BowlingMarks.OppositeMarks, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual((int) BowlingMarks.Spare, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual((int) BowlingMarks.OppositeMarks, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingStrikeAfterSpare_ShouldSetPreviousFrameTo20()
        {
            //arrange
            var previousFrameRolls = new[] { 1, 9 };
            var nextFrameRolls = new[] { (int) BowlingMarks.Strike, 0 };
            var expectedFrameCount = 2;

            //act
            _scoreCard.AddFrame(previousFrameRolls);
            _scoreCard.AddFrame(nextFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual((int) BowlingMarks.OppositeMarks, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual((int) BowlingMarks.Strike, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual((int) BowlingMarks.OppositeMarks, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingNonMarkAfterSpare_ShouldSetPreviousFrameToTenPlusFirstRoll()
        {
            //arrange
            var previousFrameRolls = new[] { 1, 9 };
            var nextFrameRolls = new[] { 1, 2 };
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
            var previousFrameRolls = new[] { 1, 9 };
            var nextFrameRolls = new[] { 4, 6 };
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
            Assert.AreEqual((int) BowlingMarks.Spare, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual(currentExpectedSum, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingNonMarkAfterStrike_ShouldSetPreviousFrameToTenPlusFirstAndSecondRoll()
        {
            //arrange
            var previousFrameRolls = new[] { (int) BowlingMarks.Strike, 0 };
            var nextFrameRolls = new[] { 1, 2 };
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
        public void AddFrame_AddingTurkeyFollowedByASpare_ShouldScaleStreakScoreForTheStrikeFields()
        {
            //arrange
            var firstFrameRolls = new[] { (int) BowlingMarks.Strike, 0 };
            var secondFrameRolls = new[] { (int) BowlingMarks.Strike, 0 };
            var thirdFrameRolls = new[] { (int) BowlingMarks.Strike, 0 };
            var fourthFrameRolls = new[] { 2, 8 };
            var expectedFrameCount = 4;
            var expectedFrame2Sum = (int) BowlingMarks.OppositeMarks + fourthFrameRolls[0];
            var expectedFrame3Sum = (int) BowlingMarks.OppositeMarks;
            var expectedTotal = (int) BowlingMarks.Turkey + expectedFrame2Sum + expectedFrame3Sum;

            //act
            _scoreCard.AddFrame(firstFrameRolls);
            _scoreCard.AddFrame(secondFrameRolls);
            _scoreCard.AddFrame(thirdFrameRolls);
            _scoreCard.AddFrame(fourthFrameRolls);

            //assert
            Assert.AreEqual(expectedFrameCount, _scoreCard.CurrentFrameCount);
            Assert.AreEqual((int) BowlingMarks.Turkey, _scoreCard.Frames[0].FrameTotal);
            Assert.AreEqual(expectedFrame2Sum, _scoreCard.Frames[1].FrameTotal);
            Assert.AreEqual(expectedFrame3Sum, _scoreCard.Frames[2].FrameTotal);
            Assert.AreEqual((int) BowlingMarks.Spare, _scoreCard.Frames[3].FrameTotal);
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingFinalFrameTurkeyAfterTwoStrikes_ShouldTallyUpValidFinalScore()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { (int) BowlingMarks.Strike, 0 },
                new[] { (int) BowlingMarks.Strike, 0 },
                new[] { (int) BowlingMarks.Strike, (int) BowlingMarks.Strike, (int) BowlingMarks.Strike }
            };
            const int expectedTotal = 160;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingTurkeyFrameAfterSingleStrike_ShouldTallyUpValidFinalScore()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 4, 0 },
                new[] { (int) BowlingMarks.Strike, 0 },
                new[] { (int) BowlingMarks.Strike, (int) BowlingMarks.Strike, (int) BowlingMarks.Strike }
            };
            const int expectedTotal = 134;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingFinalFrameTurkeyAfterSpare_ShouldTallyUpValidFinalScore()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 4, 0 },
                new[] { 1, 9 },
                new[] { (int) BowlingMarks.Strike, (int) BowlingMarks.Strike, (int) BowlingMarks.Strike }
            };
            const int expectedTotal = 124;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingTurkeyFrameAfterNonMarking_ShouldTallyUpValidFinalScore()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 4, 0 },
                new[] { 2, 4 },
                new[] { (int) BowlingMarks.Strike, (int) BowlingMarks.Strike, (int) BowlingMarks.Strike }
            };
            const int expectedTotal = 110;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingFinalFrameNoExtraRoll_ShouldTallyUpValidFinalScore()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 4, 0 },
                new[] { 1, 9 },
                new[] { 1, 2 }
            };
            const int expectedTotal = 88;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void AddFrame_AddingSpareAndStrike_ShouldTallyUpValidFinalScore()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 4, 0 },
                new[] { 2, 4 },
                new[] { 1, 9, (int) BowlingMarks.Strike }
            };
            const int expectedTotal = 100;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void CloseGameOut_MidGameEndingOnTwoStrikeStreak_ShouldTallyOutAnyOpenStrikes()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { (int) BowlingMarks.Strike, 0 },
                new[] { (int) BowlingMarks.Strike, 0 }
            };
            const int expectedTotal = 66;
            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }

        [Test]
        public void CloseGameOut_MidGameEndingOnSpare_ShouldTallyOutOpenSpares()
        {
            //arrange
            int[][] array1 =
            {
                new[] { 0, 2 },
                new[] { 5, 4 },
                new[] { 3, 7 },
                new[] { 7, 1 },
                new[] { 5, 0 },
                new[] { 1, 9 }
            };
            var expectedTotal = 51;

            foreach (var array2 in array1) _scoreCard.AddFrame(array2);

            //act
            _scoreCard.CloseGameOut();

            //assert
            Assert.AreEqual(expectedTotal, _scoreCard.CurrentScore);
        }
    }
}