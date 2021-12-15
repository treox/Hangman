using System;
using System.Text;

namespace Hangman
{
    class Program
    {
        private static bool isNotGameOverOrWordMatch = true;
        private static Random rnd = new Random();
        private static string secretWord = "";
        private static string updatedDashedWord = "";
        private static char[] dashedWord = new char[1];
        private static int numberOfGuesses = 10;
        private static char[] correctLetters = new char[1];
        private static int correctLettersIndex = 0;
        private static StringBuilder sbIncorrectLetters = new StringBuilder();

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Välkommen till Hangman!");

            do
            {
                Console.WriteLine();

                if (secretWord == "")
                {
                    int wordIndex = rnd.Next(0, 5);
                    secretWord = GenerateLetters(wordIndex);
                    updatedDashedWord = UpdateDashedWord(secretWord);
                    Console.WriteLine("Återstående gissningar: " + numberOfGuesses);
                }

                Console.WriteLine("Felaktiga bokstäver som du gissat på: ");
                Console.WriteLine(sbIncorrectLetters.ToString());

                Console.WriteLine(updatedDashedWord);
                Console.WriteLine();

                char suggestedLetter = '\0';
                bool caughtException = true;

                while (caughtException)
                {
                    try
                    {
                        Console.Write("Föreslå en bokstav genom att trycka på en tangent | Tryck på mellanslag för att gissa på hela ordet: ");
                        suggestedLetter = Console.ReadKey().KeyChar;
                        Console.WriteLine();

                        bool isLetter = char.IsLetter(suggestedLetter);

                        if (isLetter || suggestedLetter == ' ')
                        {
                            caughtException = false;
                        }
                        else
                        {
                            CauseException();
                        }
                    }
                    catch(InvalidOperationException)
                    {
                        Console.WriteLine("Fel: Inte giltigt!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                //Note: The method CountNumberOfGuesses() accounts for spacebar ' '
                int numberOfGuessesLeft = CountNumberOfGuesses(numberOfGuesses, suggestedLetter);
                numberOfGuesses = numberOfGuessesLeft;

                if(suggestedLetter != ' ')
                {
                    char guessedLetter = CheckLetterInStorage(suggestedLetter);
                    Console.WriteLine("Bokstaven som du gissade på var: " + guessedLetter);
                    updatedDashedWord = UpdateDashedWord(secretWord);
                }
                else
                {
                    Console.Write("Skriv in ett ord och tryck på enter: ");
                    string wholeWord = Console.ReadLine();
                    bool isWordCorrect = CheckWholeWord(wholeWord);

                    if(isWordCorrect == false)
                        Console.WriteLine($"Ordet {wholeWord} som du gissade på är tyvärr inkorrekt!");
                }

                Console.WriteLine();
                Console.WriteLine("Återstående gissningar: " + numberOfGuessesLeft);

                int endGame = CheckIfGameOver();

                switch (endGame)
                {
                    case 0:
                        Console.WriteLine("_______________");
                        break;
                    case 1:
                        Console.WriteLine("Grattis! Du vann! Du gissade rätt ord!");
                        string correctWord = string.Join("", dashedWord);
                        Console.WriteLine("Rätt ord är: " + correctWord);
                        break;
                    case 2:
                        Console.WriteLine("GAME OVER! Du förlorade!");
                        break;
                    default:
                        Console.WriteLine("Fel: Något blev fel!");
                        break;
                }
            } while(isNotGameOverOrWordMatch);

            Console.WriteLine();
            Console.WriteLine("Spelet avslutas...");
        }

        private static string GenerateLetters(int index)
        {
            string[] letters = new string[5] { "programmering", "it", "applikation", "webbsida", "server" };

            return letters[index];
        }

        private static string UpdateDashedWord(string word)
        {
            string dashedWordStr = "";
            int loopValue = 0;

            if(dashedWord.Length <= 1)
            {
                foreach (char character in word)
                {
                    dashedWord[loopValue] = Convert.ToChar("_");
                    if(dashedWord.Length != word.Length)
                        Array.Resize<char>(ref dashedWord, dashedWord.Length + 1);
                    loopValue++;
                }
            }

            foreach (char letter in correctLetters)
            {
                if (secretWord.Contains(letter))
                {
                    for(int i = 0; i <= secretWord.Length - 1; i++)
                    {
                        if(secretWord[i] == letter)
                        {
                            dashedWord[i] = letter;
                        }
                    }
                }
            }

            foreach(char appendToString in dashedWord)
            {
                dashedWordStr += appendToString + " ";
            }

            return dashedWordStr;
        }

        private static int CountNumberOfGuesses(int guesses, char letter)
        {
            int guessesLeft;
            bool containsLetter = false;

            if(letter != ' ')
            {
                for (int c = 0; c < correctLetters.Length; c++)
                {
                    if(correctLetters[c] == letter || sbIncorrectLetters.ToString().Contains(letter))
                    {
                        containsLetter = true;
                    }
                }
            }

            if(containsLetter == false)
            {
                guessesLeft = --guesses;
            }
            else
            {
                guessesLeft = guesses;
            }

            return guessesLeft;
        }

        private static char CheckLetterInStorage(char character)
        {
            if (secretWord.Contains(character))
            {
                bool containsCharacter = false;

                for(int l = 0; l < correctLetters.Length; l++)
                {
                    if(correctLetters[l] == character)
                        containsCharacter = true;
                }

                if(containsCharacter == false)
                {
                    correctLetters.SetValue(character, correctLettersIndex);
                    correctLettersIndex++;
                    Array.Resize<char>(ref correctLetters, correctLettersIndex + 1);
                }
            }
            else
            {
                bool sbContainsCharacter = false;

                if (sbIncorrectLetters.ToString().Contains(character))
                    sbContainsCharacter = true;

                if(sbContainsCharacter == false)
                    sbIncorrectLetters.Append(character + " ");
            }

            return character;
        }

        private static bool CheckWholeWord(string word)
        {
            bool wordIsCorrect = false;

            if(word == secretWord)
            {
                for(int w = 0; w < secretWord.Length; w++)
                {
                    dashedWord[w] = secretWord[w];
                }

                wordIsCorrect = true;
            }

            return wordIsCorrect;
        }

        private static int CheckIfGameOver()
        {
            string dashedWordjoined = string.Join("", dashedWord);

            if (dashedWordjoined == secretWord)
            {
                isNotGameOverOrWordMatch = false;
                return 1;
            }
            else if (numberOfGuesses == 0)
            {
                isNotGameOverOrWordMatch = false;
                return 2;
            }
            else
            {
                return 0;
            }
        }

        private static void CauseException()
        {
            throw new Exception("Fel: Du behöver mata in en bokstav eller trycka på mellanslag!");
        }
    }
}
