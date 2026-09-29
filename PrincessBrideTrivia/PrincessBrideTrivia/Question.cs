namespace PrincessBrideTrivia;

public class Question
{
    //Utilizes getters and setters to define the properties of a question, including the text of the question, the possible answers, and the index of the correct answer. 
    public string Text { get; set; } 
    public string[] Answers { get; set; }
    public string CorrectAnswerIndex { get; set; }
}