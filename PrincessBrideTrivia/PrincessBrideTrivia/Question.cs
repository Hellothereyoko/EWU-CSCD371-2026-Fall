namespace PrincessBrideTrivia;

/// <summary>
/// Represents a trivia question with its text, possible answers, and the index of the correct answer.
/// </summary>
public class Question
{
    
    public string Text { get; set; } 
    public string[] Answers { get; set; }
    public string CorrectAnswerIndex { get; set; }
}