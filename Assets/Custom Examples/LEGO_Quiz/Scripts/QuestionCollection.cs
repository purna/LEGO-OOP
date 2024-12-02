using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class QuestionCollection : MonoBehaviour
{
    [Header("ScriptableObjects for Questions (optional)")]
    [Tooltip("Array of scriptable objects to use for quiz questions.")]
    public QuestionData[] scriptableQuestions; // Array of ScriptableObject questions

    private QuizQuestion[] allQuestions;

    private void Awake()
    {
        if (scriptableQuestions != null && scriptableQuestions.Length > 0)
        {
            // Load questions from the array of ScriptableObjects
            LoadQuestionsFromScriptableObjects();
        }
        else
        {
            if (!File.Exists("Questions.xml"))
            {
                WriteSampleQuestionsToXml();
            }

            LoadAllQuestions();
        }
    }

    private void LoadQuestionsFromScriptableObjects()
    {
        // Convert each ScriptableObject into a QuizQuestion
        allQuestions = scriptableQuestions.Select(scriptableQuestion =>
        {
            // Convert AnswerData[] to string[] for QuizQuestion
            string[] answerTexts = scriptableQuestion.answers.Select(a => a.answerText).ToArray();

            return new QuizQuestion
            {
                Question = scriptableQuestion.question,
                Answers = answerTexts,
                CorrectAnswer = scriptableQuestion.correctAnswer,
                QuestionImage = scriptableQuestion.questionImage // Optional question image
            };
        }).ToArray();
    }

    private void LoadAllQuestions()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(QuizQuestion[]));
        using (StreamReader streamReader = new StreamReader("Questions.xml"))
        {
            allQuestions = (QuizQuestion[])serializer.Deserialize(streamReader);
        }
    }

    public QuizQuestion GetUnaskedQuestion()
    {
        ResetQuestionsIfAllHaveBeenAsked();

        var question = allQuestions
            .Where(t => t.Asked == false)
            .OrderBy(t => UnityEngine.Random.Range(0, int.MaxValue))
            .FirstOrDefault();

        question.Asked = true;
        return question;
    }

    private void ResetQuestionsIfAllHaveBeenAsked()
    {
        if (allQuestions.Any(t => t.Asked == false) == false)
        {
            ResetQuestions();
        }
    }

    private void ResetQuestions()
    {
        foreach (var question in allQuestions)
            question.Asked = false;
    }

    /// <summary>
    /// This method is used to generate a starting sample xml file if none exists
    /// </summary>
    private void WriteSampleQuestionsToXml()
    {
        allQuestions = new QuizQuestion[] {
            new QuizQuestion { Question = "If it's noon in Boston, what time is it in New York",
                Answers = new string[] { "1PM", "2PM", "Noon", "11AM" }, CorrectAnswer = 2 },
            new QuizQuestion { Question = "What type of animal was Babe in the film of the same name",
                Answers = new string[] { "Donkey", "Spider", "Dog", "Pig" }, CorrectAnswer = 3 },
        };

        XmlSerializer serializer = new XmlSerializer(typeof(QuizQuestion[]));
        using (StreamWriter streamWriter = new StreamWriter("Questions.xml"))
        {
            serializer.Serialize(streamWriter, allQuestions);
        }
    }
}
