using System;
using System.IO;
using Xunit;

namespace Hangman.Tests
{
    public class HangmanGameShould
    {
        [Theory]
        [InlineData(0, "programmering")]
        [InlineData(1, "it")]
        [InlineData(2, "applikation")]
        [InlineData(3, "webbsida")]
        [InlineData(4, "server")]
        public void GenerateALetter(int index, string testWord)
        {
            string path = Directory.GetCurrentDirectory() + "\\words.txt";

            string testStr = Program.GenerateLetters(index, path);

            Assert.Equal(testWord, testStr);
        }

        [Fact]
        public void ThrowAnExceptionWhenUsingAnInvalidPath()
        {
            string path = "test";
            int index = 0;

            Assert.Throws<Exception>(() => Program.GenerateLetters(index, path));
        }

        [Theory]
        [InlineData("test", "_ _ _ _ ")]
        public void UpdateTheDashedWord(string testWord, string testWordExpected)
        {
            string testWordStr = Program.UpdateDashedWord(testWord);

            Assert.Equal(testWordExpected, testWordStr);
        }

        [Fact]
        public void CountGuessesMade()
        {
            int testGuesses = 10;
            int expectedTestGuessesLeft = 9;
            char testCharacter = 't';

            int actualTestGuessesLeft = Program.CountNumberOfGuesses(testGuesses, testCharacter);

            Assert.Equal(expectedTestGuessesLeft, actualTestGuessesLeft);
        }

        [Fact]
        public void ReturnSameCharacter()
        {
            char testCharacter = 't';
            char expectedCharacter = 't';

            char actualCharacter = Program.CheckLetterInStorage(testCharacter);

            Assert.Equal(expectedCharacter, actualCharacter);
        }

        [Fact]
        public void ReturnABoolean()
        {
            string testWord = "test";

            bool actualBoolean = Program.CheckWholeWord(testWord);

            Assert.False(actualBoolean);
        }

        [Fact]
        public void CheckIfGameIsOverOrWon()
        {
            int expectedValue = 0;

            int actualValue = Program.CheckIfGameOver();

            Assert.Equal(expectedValue, actualValue);
        }
    }
}
