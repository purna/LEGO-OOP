using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class QuestionCollection : MonoBehaviour
{
    [Header("ScriptableObject for Questions (optional)")]
    public QuestionData scriptableQuestion; // Drag a ScriptableObject here if available

    private QuizQuestion[] allQuestions;

    private void Awake()
    {
        if (scriptableQuestion != null)
        {
            // Load a single question from ScriptableObject
            LoadQuestionFromScriptableObject();
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

    private void LoadQuestionFromScriptableObject()
    {
        // Convert AnswerData[] to string[] for QuizQuestion
        string[] answerTexts = scriptableQuestion.answers.Select(a => a.answerText).ToArray();

        allQuestions = new QuizQuestion[]
        {
            new QuizQuestion
            {
                Question = scriptableQuestion.question,
                Answers = answerTexts,
                CorrectAnswer = scriptableQuestion.correctAnswer
            }
        };
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
