using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for TextMesh Pro components

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text questionText; // For the question text
    [SerializeField] private Image questionImage; // For the question text
    [SerializeField] private GameObject answerButtonContainer; // Parent GameObject containing all answer buttons
    [SerializeField] private GameObject correctAnswerPopup; // Correct answer popup
    [SerializeField] private GameObject wrongAnswerPopup; // Wrong answer popup

    private Button[] answerButtons; // Array to store references to answer buttons

    private void Awake()
    {
        // Automatically populate answerButtons from children of the container
        answerButtons = answerButtonContainer.GetComponentsInChildren<Button>();
    }

    public void SetupUIForQuestion(QuizQuestion question)
    {
        correctAnswerPopup.SetActive(false);
        wrongAnswerPopup.SetActive(false);

        questionText.text = question.Question;

       // Set question image if available (optional)
        if (questionImage != null)
        {
            questionImage.sprite = question.QuestionImage; // Set the image for the question
            questionImage.gameObject.SetActive(true); // Enable the image object
        }
        else
        {
            questionImage.gameObject.SetActive(false); // Hide the image object if no image is set
        }


        for (int i = 0; i < question.Answers.Length; i++)
        {
            var answerText = answerButtons[i].GetComponentInChildren<TMP_Text>(); // Fetch TMP_Text from children
            if (answerText != null)
            {
                answerText.text = question.Answers[i]; // Set answer text
            }
            else
            {
                Debug.LogError($"Button {answerButtons[i].name} is missing a TMP_Text component in its children.");
            }

            answerButtons[i].gameObject.SetActive(true); // Activate button
        }

        // Hide any extra buttons if the question has fewer answers
        for (int i = question.Answers.Length; i < answerButtons.Length; i++)
        {
            answerButtons[i].gameObject.SetActive(false);
        }
    }

    public void HandleSubmittedAnswer(bool isCorrect)
    {
        ToggleAnswerButtons(false);
        if (isCorrect)
        {
            ShowCorrectAnswerPopup();
        }
        else
        {
            ShowWrongAnswerPopup();
        }
    }

    private void ToggleAnswerButtons(bool value)
    {
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(value);
        }
    }

    private void ShowCorrectAnswerPopup()
    {
        correctAnswerPopup.SetActive(true);
    }

    private void ShowWrongAnswerPopup()
    {
        wrongAnswerPopup.SetActive(true);
    }
}
