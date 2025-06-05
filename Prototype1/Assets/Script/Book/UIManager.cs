using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject BookUI;
    public GameObject NavUI;
    public GameObject ShowOption;
    public GameObject ToolBar;


    public TextMeshProUGUI nameText;
    public TextMeshProUGUI ageText;
    public TextMeshProUGUI genderText;
    public TextMeshProUGUI diseaseText;
    public TextMeshProUGUI bloodText;
    public TextMeshProUGUI pregnantText;
    public GameObject PatientCanvas;
    public TextMeshProUGUI bloodTestText;
    public Sprite XrayTested;
    public Image xrayImageUI;
    public GameObject infoText;

    public static UIManager Instance;


    public TextMeshProUGUI questionAnswerText;

    private PatientMovement selectedPatient;

    public int HospitalReputation;
    public TextMeshProUGUI HospitalReputationText;
    public int Token;
    public TextMeshProUGUI Token_Text;

    public Canvas ShowAnswerCanvas;

    public void Start()
    {
        ShowOption.gameObject.SetActive(false);
        PatientCanvas.gameObject.SetActive(false);
        ShowAnswerCanvas.gameObject.SetActive(false);
        HospitalReputation = 100;
        HospitalReputationText.text = HospitalReputation.ToString();
        Token = 2000;
        Token_Text.text = Token.ToString();
    }

    public void Update()
    {
        if (HospitalReputation <= 100) {
            HospitalReputation += 0;
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    public void OnBookButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnGameResumePress()
    {
        BookUI.SetActive(false);
        NavUI.SetActive(false);
    }

    public void OnEnterBookPress()
    {
        BookUI.SetActive(true);
    }

    public void OnEnterNav()
    {
        NavUI.SetActive(true);
    }

    public void OnEnterExit()
    {
        BookUI.SetActive(false);
        NavUI.SetActive(false);
    }

    public void ShowPatientOption()
    {
        ShowOption.gameObject.SetActive(true);
        ToolBar.gameObject.SetActive(false);
    }

    public void ExitShowPatientOption()
    {
        ShowOption.gameObject.SetActive(false);
        ToolBar.gameObject.SetActive(true);
        selectedPatient.MoveRandomly();

        ShowOption.gameObject.SetActive(false);
        ToolBar.gameObject.SetActive(true);

        if (selectedPatient != null)
        {
            // Become MoveRandomly Again
            selectedPatient.SetIsInteract(false);

            selectedPatient.StartCoroutine(selectedPatient.MoveRandomly());
        }
    
}



    public void SendSelectedPatientToRoom(Transform roomTransform)
    {
        if (selectedPatient != null)
        {
            selectedPatient.MoveToTargetRoom(roomTransform); // ????????????????????????????
            ShowOption.gameObject.SetActive(false);
            ToolBar.gameObject.SetActive(true);
        }
    }

    public void SelectPatient(PatientMovement patient)
    {
        selectedPatient = patient;
    }

    public void ShowPatientInfo(PatientData data)
    {
        if (data == null) return;

        nameText.text = data.patientName;
        ageText.text = data.age.ToString();
        genderText.text = data.gender;
        diseaseText.text = data.disease != null ? data.disease.diseaseName : "";
        bloodText.text = data.bloodType;
        pregnantText.text = data.isPregnant ? "True" : "False";
      

        questionAnswerText.text = "";
        var answers = data.GetAllAnswers();
        foreach (var entry in answers)
        {
            questionAnswerText.text += $"{entry.Value}\n";
        }


        if (data.hasBloodTested)
        {
            bloodTestText.text =
                $"RBC: {data.rbcStatusText}\n" +
                $"WBC: {data.wbcStatusText}\n" +
                $"Neutrophil: {data.neutrophilStatusText}\n" +
                $"Eosinophil: {data.eosinophilStatusText}\n" +
                $"Basophil: {data.basophilStatusText}\n" +
                $"Lymphocyte: {data.lymphocyteStatusText}\n" +
                $"Monocyte: {data.monocyteStatusText}";
        }
        else
        {
            bloodTestText.text = "No result";
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
        else
        {
            xrayImageUI.enabled = false;
            XrayTested = null;
        }





    }

   
}