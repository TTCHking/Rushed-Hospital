using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestigateRoom : Room
{
    private PatientData currentPatient;
    private int questionCount = 0;
    private HashSet<int> askedQuestions = new HashSet<int>();

    public Canvas PatientOption;
    public Canvas QuestionText;
    public PlayerController playerController;

    [Header("Question UI")]
    public Button question1Button;
    public Button question2Button;
    public Button question3Button;
    public TextMeshProUGUI answerText;

    private AskingDatabase currentAsking;

    public override void EnterRoom(PatientData data)
    {
        base.EnterRoom(data);
        currentPatient = data;
        questionCount = 0;
        askedQuestions.Clear();

        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        Debug.Log("Diagnosing Patient: " + data.patientName);

        // ??? DiseaseData ??? AskingDatabase
        DiseaseData diseaseData = data.GetDiseaseData();
        if (diseaseData != null && diseaseData.Asking is AskingDatabase askingData)
        {
            currentAsking = askingData;
            SetupQuestionButtons(askingData);
        }
        else
        {
            Debug.LogWarning("Missing DiseaseData or AskingDatabase");
        }
    }

    public void SetupQuestionButtons(AskingDatabase askingData)
    {
        question1Button.onClick.RemoveAllListeners();
        question2Button.onClick.RemoveAllListeners();
        question3Button.onClick.RemoveAllListeners();

        question1Button.onClick.AddListener(() => AskSpecificQuestion(1));
        question2Button.onClick.AddListener(() => AskSpecificQuestion(2));
        question3Button.onClick.AddListener(() => AskSpecificQuestion(3));
    }

    private void AskSpecificQuestion(int questionIndex)
    {
        if (questionCount >= 3 || askedQuestions.Contains(questionIndex)) return;

        askedQuestions.Add(questionIndex);
        questionCount++;

        string answer = "???????????";
        switch (questionIndex)
        {
            case 1:
                answer = GetRandomAnswer(currentAsking.answerofquestion1);
                break;
            case 2:
                answer = GetRandomAnswer(currentAsking.answerofquestion2);
                break;
            case 3:
                answer = GetRandomAnswer(currentAsking.answerofquestion3);
                break;
        }

        currentPatient.SaveAnswer(questionIndex, answer);

        answerText.text = $"\n{answer}";

       

        if (questionCount >= 4)
        {
            Debug.Log("?????? 3 ?????????");
            PatientOption.gameObject.SetActive(true);
            QuestionText.gameObject.SetActive(false);

            if (playerController != null)
            {
                playerController.ExitInvestigate();
            }
            else
            {
                Debug.LogError("playerController ????????? assign!");
            }
        }
    }

    private string GetRandomAnswer(string[] answers)
    {
        if (answers == null || answers.Length == 0) return "???????????";
        return answers[Random.Range(0, answers.Length)];
    }
}
