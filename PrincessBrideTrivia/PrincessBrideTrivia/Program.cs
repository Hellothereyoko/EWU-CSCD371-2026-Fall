namespace PrincessBrideTrivia; //Inherits Project Space 

public class Program
{
    /// <summary>
    /// This is the main entry point for the program. It loads the questions from a file,
    /// asks each question to the user, and keeps track of the number of correct answers.
    /// At the end, it displays the percentage of correct answers.
    /// </summary>
    public static void Main(string[] args)
    {
        
        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);

       
        int numberCorrect = 0;
        for (int i = 0; i < questions.Length; i++)
        {
            bool result = AskQuestion(questions[i]);
            if (result)
            {
                numberCorrect++; 
            }
        }
        Console.WriteLine("You got " + GetPercentCorrect(numberCorrect, questions.Length) + " correct");
    }


    /// <summary>
    /// Calculates the percentage of correct answers.
    /// </summary>
    /// <param name="numberCorrectAnswers">The number of correct answers.</param>
    /// <param name="numberOfQuestions">The total number of questions.</param>
    /// <returns>A formatted percentage string representing the percentage of correct answers.</returns>
    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        double percentCorrect = (double)numberCorrectAnswers/ numberOfQuestions* 100;
        return Math.Round(percentCorrect, MidpointRounding.AwayFromZero) + "%"; // bug fix// cast int vals to double for division and rounded the return value to nearest decimal
    }


    /// <summary>
    /// Asks a question, captures the user's guess, and returns whether the answer was correct.
    /// </summary>
    /// <param name="question">The question to ask the user.</param>
    /// <returns><c>true</c> if the user's guess is correct; otherwise, <c>false</c>.</returns>
    public static bool AskQuestion(Question question)
    {
        DisplayQuestion(question);

        string userGuess = GetGuessFromUser();
        return DisplayResult(userGuess, question);
    }


    /// <summary>
    /// Reads the user's guess from the console.
    /// </summary>
    /// <returns>The user's input as a string.</returns>
    public static string GetGuessFromUser()
    {
        return Console.ReadLine();
    }


    /// <summary>
    /// Displays the result of the user's guess for the specified question.
    /// </summary>
    /// <param name="userGuess">The user's guess.</param>
    /// <param name="question">The question being answered.</param>
    /// <returns><c>true</c> if the guess matches the correct answer; otherwise, <c>false</c>.</returns>
    public static bool DisplayResult(string userGuess, Question question)
    {
        if (userGuess == question.CorrectAnswerIndex)
        {
            Console.WriteLine("Correct");
            return true;
        }

        Console.WriteLine("Incorrect");
        return false;
    }


    /// <summary>
    /// Displays the question text and answer choices to the console.
    /// </summary>
    /// <param name="question">The question to display.</param>
    public static void DisplayQuestion(Question question)
    {
        Console.WriteLine("Question: " + question.Text);
        for (int i = 0; i < question.Answers.Length; i++)
        {
            Console.WriteLine((i + 1) + ": " + question.Answers[i]);
        }
    }


    /// <summary>
    /// Returns the file path for the trivia questions file.
    /// </summary>
    /// <returns>The path to the trivia questions file.</returns>
    public static string GetFilePath()
    {
        return "Trivia.txt";
    }


    /// <summary>
    /// Loads questions from a file and returns them as an array of <see cref="Question"/> objects.
    /// </summary>
    /// <param name="filePath">The path to the file containing the questions.</param>
    /// <returns>An array of questions loaded from the file.</returns>
    public static Question[] LoadQuestions(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        Question[] questions = new Question[lines.Length / 5];
        for (int i = 0; i < questions.Length; i++)
        {
            int lineIndex = i * 5;
            string questionText = lines[lineIndex];

            string answer1 = lines[lineIndex + 1];
            string answer2 = lines[lineIndex + 2];
            string answer3 = lines[lineIndex + 3];

            string correctAnswerIndex = lines[lineIndex + 4];

            Question question = new();
            question.Text = questionText;
            question.Answers = new string[3];
            question.Answers[0] = answer1;
            question.Answers[1] = answer2;
            question.Answers[2] = answer3;
            question.CorrectAnswerIndex = correctAnswerIndex;
            questions[i] = question; // bug fix // add the question object to the array
        }
        return questions;
    }


    /// <summary>
    /// Prompts the user to choose which quiz to take.
    /// </summary>
    /// <returns>The selected quiz number, either <c>1</c> or <c>2</c>.</returns>
    public static int GetQuizInputFromUser()
    {
        while (true)
        {
            Console.WriteLine("Would you like to take quiz one, or quiz two?");
            Console.Write("Enter 1 or 2: ");
            
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    return 1;
                
                case "2":
                    return 2;
                
                default:
                    Console.WriteLine("Invalid input. Please enter a 1 or a 2.");
                    break;
            }
        }
    }
}
