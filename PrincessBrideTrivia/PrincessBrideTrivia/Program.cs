namespace PrincessBrideTrivia; //Inherits Project Space 

public class Program
{
     /*
    * This is the main entry point for the program. It loads the questions from a file, asks each question to the user, and keeps track of the number of correct answers. At the end, it displays the percentage of correct answers.
    */
    public static void Main(string[] args)
    {
        //Load question from defined file & structure according to Question Method
        string filePath = GetFilePath();
        Question[] questions = LoadQuestions(filePath);

        //Init numCorrect var and loop through until all have been asked AND answered! 
        int numberCorrect = 0;
        for (int i = 0; i < questions.Length; i++)
        {
            bool result = AskQuestion(questions[i]);
            if (result)
            {
                numberCorrect++; //If answer is correct, increment the numberCorrect variable
            }
        }
        Console.WriteLine("You got " + GetPercentCorrect(numberCorrect, questions.Length) + " correct");
    }


    /*
    * This method calculates the percentage of correct answers and returns it as a formatted string.
    * @param numberCorrectAnswers The number of correct answers.
    * @param numberOfQuestions The total number of questions.
    * @return A string representing the percentage of correct answers, formatted as a percentage.
    * 
    */
    public static string GetPercentCorrect(int numberCorrectAnswers, int numberOfQuestions)
    {
        return (numberCorrectAnswers / numberOfQuestions * 100) + "%"; // !! TEST CASE FAILURE CAUSE STRING ALSO DOESNT MATCH DUE TO MISCALC !!  
    }


    /*
    * This method asks a question to the user, gets their guess, and displays the result
    * @param question The question to ask.
    * @return A boolean indicating whether the user's guess was correct or not.
    */
    public static bool AskQuestion(Question question)
    {
        DisplayQuestion(question);

        string userGuess = GetGuessFromUser();
        return DisplayResult(userGuess, question);
    }


    /*
    * This method gets the user's guess from the console.
    * @return A string representing the user's guess.
    */
    public static string GetGuessFromUser()
    {
        return Console.ReadLine();
    }


    /*
    * This method displays the result of the user's guess.
    * @param userGuess The user's guess.
    * @param question The question being asked.
    * @return A boolean indicating whether the user's guess was correct or not.
    */
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


    /*
    * This method displays a question and its possible answers to the console.
    * @param question The question to display.
    */
    public static void DisplayQuestion(Question question)
    {
        Console.WriteLine("Question: " + question.Text);
        for (int i = 0; i < question.Answers.Length; i++)
        {
            Console.WriteLine((i + 1) + ": " + question.Answers[i]);
        }
    }


    /* 
    * This method returns the file path of the trivia questions file.
    * @return A string representing the file path of the trivia questions file.
    */
    public static string GetFilePath()
    {
        return "Trivia.txt";
    }


    /*
    * This method loads questions from a file and returns them as an array of Question objects.
    * @param filePath The path to the file containing the questions.
    * @return An array of Question objects.
    */
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
        }
        return questions;
    }
}
