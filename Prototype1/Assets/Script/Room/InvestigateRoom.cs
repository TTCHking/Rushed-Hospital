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
    public bool hasaleadyinvestigate = false;

    private AskingDatabase currentAsking;

    public UIManager uIManager;
    public PatientInteract patientInteract;
    public Transform Player;
    public InvestigateRoom investigateRoom;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI diseaseText;
    public TextMeshProUGUI bloodText;
    public TextMeshProUGUI pregnantText;
    public TextMeshProUGUI questionAnswerText;
    public TextMeshProUGUI bloodTestText;
    public Sprite XrayTested;
    public Image xrayImageUI;


    public override void EnterRoom(PatientData data)
    {
        base.EnterRoom(data);
        currentPatient = data;
        questionCount = 0;
        askedQuestions.Clear();

        if (hasaleadyinvestigate) {
            if (patientInteract.warpTarget != null)
            {
                uIManager.ExitShowPatientOption();
                Player.position = patientInteract.warpTarget.position;
                Player.rotation = patientInteract.warpTarget.rotation;
                patientInteract.Hotbar.gameObject.SetActive(false);
                patientInteract.Book.gameObject.SetActive(false);

                PlayerController controller = data.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.SetCanMove(false);
                }

                if (patientInteract.investigateVCam != null)
                {
                    patientInteract.investigateVCam.Priority = 20;
                }
                uIManager.ShowAnswerCanvas.gameObject.SetActive(true);
                uIManager.infoText.gameObject.SetActive(true);

                if (data == null) return;

                nameText.text = data.patientName;
                ageText.text = data.age.ToString();
                genderText.text = data.gender;
                diseaseText.text = data.disease != null ? data.disease.diseaseName : "";
                bloodText.text = data.bloodType;
                pregnantText.text = data.isPregnant ? "True" : "False";

                if (investigateRoom.hasaleadyinvestigate) {
                    questionAnswerText.text = "";
                    var answers = data.GetAllAnswers();
                    foreach (var entry in answers)
                    {
                        questionAnswerText.text += $"{entry.Value}\n";
                    }
                }

                if (data.hasBloodTested) {
                    bloodTestText.text =
                    $"RBC: {data.rbcStatusText}\n" +
                    $"WBC: {data.wbcStatusText}\n" +
                    $"Neutrophil: {data.neutrophilStatusText}\n" +
                    $"Eosinophil: {data.eosinophilStatusText}\n" +
                    $"Basophil: {data.basophilStatusText}\n" +
                    $"Lymphocyte: {data.lymphocyteStatusText}\n" +
                    $"Monocyte: {data.monocyteStatusText}";
                }

                xrayImageUI.sprite = XrayTested;
                xrayImageUI.enabled = true;
                if (data.hasXrayTested)
                {
                    if (data.disease != null && data.disease.diseaseIcon != null)
                    {
                        XrayTested = data.disease.diseaseIcon;  // ???? Sprite ??????????? Sprite

                        // ?????????? Image component ???? xrayImageUI
                        xrayImageUI.sprite = XrayTested;
                        xrayImageUI.enabled = true;
                    }
                    else
                    {
                        // ???? UI ??????????? Sprite
                        xrayImageUI.enabled = false;
                        XrayTested = null;
                    }
                }

            }
        }



        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        Debug.Log("Diagnosing Patient: " + data.patientName);

        //????????? DiseaseData ???????????????? AskingDatabase ??????????????????????????????????????? AskingDatabase
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

    //??????????????????? Set ????????????????
    public void SetupQuestionButtons(AskingDatabase askingData)
    {
        question1Button.onClick.RemoveAllListeners();
        question2Button.onClick.RemoveAllListeners();
        question3Button.onClick.RemoveAllListeners();

        question1Button.onClick.AddListener(() => AskSpecificQuestion(1));
        question2Button.onClick.AddListener(() => AskSpecificQuestion(2));
        question3Button.onClick.AddListener(() => AskSpecificQuestion(3));
    }

    //???????????????????????????????????
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

        //Patient???????????????????????????????????
        currentPatient.SaveAnswer(questionIndex, answer);

        //?????Text????????
        answerText.text = $"\n{answer}";


        //?????????????????????? 
        if (questionCount >= 3)
        {
            Debug.Log("Aleady 3 Answer");
            PatientOption.gameObject.SetActive(true);
            QuestionText.gameObject.SetActive(false);
            hasaleadyinvestigate = true;


            if (playerController != null)
            {
                playerController.ExitInvestigate();
            }
            else
            {
                Debug.LogError("PlayerController null");
            }
        }
    }

    //????????????????????
    private string GetRandomAnswer(string[] answers)
    {
        if (answers == null || answers.Length == 0) return "???????????";
        return answers[Random.Range(0, answers.Length)];
    }

    public void CheckDisease(DiseaseData correctDisease)
    {
        if (currentPatient == null || currentPatient.disease == null)
        {
            Debug.LogWarning("No patient or disease data to check.");
            return;
        }

        if (currentPatient.disease == correctDisease)
        {
            Debug.Log("Correct Diagnosis");
            // ????????????????????? ????????????
        }
        else
        {
            Debug.Log("Wrong Diagnosis");
            // ????????????????????
        }
    }
}
