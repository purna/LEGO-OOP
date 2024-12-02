public class QuizQuestion
{
    public string Question { get; set; }
    public string[] Answers { get; set; }
    public int CorrectAnswer { get; set; }

    public bool Asked { get; set; }
}