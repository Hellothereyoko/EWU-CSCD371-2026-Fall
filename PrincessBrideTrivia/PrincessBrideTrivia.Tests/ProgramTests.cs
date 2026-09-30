namespace PrincessBrideTrivia.Tests;

[TestClass]
public class ProgramTests
{
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

    [TestMethod]
    public void GetFilePath_WhenCalled_ReturnsExistingFilePath()
    {
        // Arrange

        // Act
        string filePath = Program.GetFilePath();

        // Assert
        Assert.IsTrue(File.Exists(filePath));
    }

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
