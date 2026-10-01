namespace PrincessBrideTrivia.Tests;

/// <summary>
/// Contains unit tests for the trivia program behavior.
/// </summary>
[TestClass]
public class ProgramTests
{

    /// <summary>
    /// Verifies that a valid question file produces the expected number of loaded questions.
    /// </summary>
    [TestMethod]
    public void LoadQuestions_ValidFilePath_ReturnsCorrectNumberOfQuestions()
    {
        string filePath = Path.GetRandomFileName();
        try
        {
            // Arrange
            GenerateQuestionsFile(filePath, 2);

            // Act
            Question[] questions = Program.LoadQuestions(filePath);

            // Assert 
            Assert.HasCount(2, questions);
        }
        finally
        {
            File.Delete(filePath);
        }
    }


    /// <summary>
    /// Verifies that a guessed answer returns the expected pass/fail result.
    /// </summary>
    /// <param name="userGuess">The user's guess.</param>
    /// <param name="expectedResult"><c>true</c> when the guess matches the correct answer; otherwise, <c>false</c>.</param>
    [TestMethod]
    [DataRow("1", true)]
    [DataRow("2", false)]
    public void DisplayResult_ValidUserGuess_ReturnsExpectedBoolean(string userGuess, bool expectedResult)
    {
        // Arrange
        Question question = new();
        question.CorrectAnswerIndex = "1";

        // Act
        bool displayResult = Program.DisplayResult(userGuess, question);

        // Assert
        Assert.AreEqual(expectedResult, displayResult);
    }


    /// <summary>
    /// Verifies that the trivia file path resolves to an existing file.
    /// </summary>
    [TestMethod]
    public void GetFilePath_WhenCalled_ReturnsExistingFilePath()
    {
        // Arrange

        // Act
        string filePath = Program.GetFilePath();

        // Assert
        Assert.IsTrue(File.Exists(filePath));
    }


    /// <summary>
    /// Verifies that the percentage string is formatted correctly for valid input values.
    /// </summary>
    /// <param name="numberOfCorrectGuesses">The number of correct guesses.</param>
    /// <param name="numberOfQuestions">The total number of questions.</param>
    /// <param name="expectedString">The expected formatted percentage string.</param>
    [TestMethod]
    [DataRow(1, 1, "100%")]
    [DataRow(5, 10, "50%")]
    [DataRow(1, 10, "10%")]
    [DataRow(0, 10, "0%")]
    public void GetPercentCorrect_ValidCorrectAndTotalCounts_ReturnsFormattedPercentageString(int numberOfCorrectGuesses,
        int numberOfQuestions, string expectedString)
    {
        // Arrange

        // Act
        string percentage = Program.GetPercentCorrect(numberOfCorrectGuesses, numberOfQuestions);

        // Assert
        Assert.AreEqual(expectedString, percentage); 
    }


    /// <summary>
    /// Creates a temporary trivia question file with the requested number of entries.
    /// </summary>
    /// <param name="filePath">The path to the file to create.</param>
    /// <param name="numberOfQuestions">The number of question entries to generate.</param>
    private static void GenerateQuestionsFile(string filePath, int numberOfQuestions)
    {
        for (int i = 0; i < numberOfQuestions; i++)
        {
            string[] lines =
            [
                "Question " + i + " this is the question text",
                "Answer 1",
                "Answer 2",
                "Answer 3",
                "2",
            ];
            File.AppendAllLines(filePath, lines);
        }
    }


    /// <summary>
    /// Verifies that invalid quiz selections are rejected until a valid value is entered.
    /// </summary>
    /// <param name="simInput">The simulated console input sequence.</param>
    /// <param name="expectedResult">The expected quiz selection after processing the input.</param>
    [ResourceLock(WellKnownResources.Console)] // MSTEST0074: on 'Console.SetIn'
    [TestMethod]
    [DataRow(3, 9, "33%")]   // rounds down 33.33
    [DataRow(6, 9, "67%")]   // rounds up 66.67
    public void GetPercentCorrect_NinthsRoundCorrectly(int correct, int total, string expected)
    {
        Assert.AreEqual(expected, Program.GetPercentCorrect(correct, total));
    }
    [TestMethod]
    public void DisplayResult_CorrectGuess_ReturnsTrueAndPrintsCorrect()
    {
        TextWriter originalOut = Console.Out;
        StringWriter output = new();
        try
        {
            Console.SetOut(output);
            Question question = new Question
            {
                Text = "Q?",
                Answers = ["A", "B", "C"],
                CorrectAnswerIndex = "1"
            };

            bool result = Program.DisplayResult("1", question);

            Assert.IsTrue(result);
            Assert.AreEqual("Correct", output.ToString().Trim());
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}
