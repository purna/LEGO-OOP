using UnityEngine;

public class QuizScoreManager : MonoBehaviour
{
    private int totalQuestions = 0;
    private int correctAnswers = 0;

    public int TotalQuestions => totalQuestions; // Expose total questions (read-only)
    public int CorrectAnswers => correctAnswers; // Expose correct answers (read-only)

    // Singleton pattern (optional if only one instance of ScoreManager is needed)
    private static QuizScoreManager instance;

    public static QuizScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuizScoreManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Enforce singleton
        }
    }

    /// <summary>
    /// Increment the total number of questions presented.
    /// </summary>
    public void IncrementTotalQuestions()
    {
        totalQuestions++;
    }

    /// <summary>
    /// Increment the number of correct answers.
    /// </summary>
    public void IncrementCorrectAnswers()
    {
        correctAnswers++;
    }

    /// <summary>
    /// Reset the score.
    /// </summary>
    public void ResetScore()
    {
        totalQuestions = 0;
        correctAnswers = 0;
    }

    /// <summary>
    /// Get a formatted score string, e.g., "3/10".
    /// </summary>
    public string GetScoreString()
    {
        return $"{correctAnswers}/{totalQuestions}";
    }
}
