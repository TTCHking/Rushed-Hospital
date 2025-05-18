using System.Collections.Generic;
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

    public TextMeshProUGUI questionAnswerText;

    private PatientMovement selectedPatient;

    public void Start()
    {
        ShowOption.gameObject.SetActive(false);
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

    public void ShowPatientOption() { 
        ShowOption.gameObject.SetActive(true);
        ToolBar.gameObject.SetActive(false);
    }

    public void ExitShowPatientOption() {
        ShowOption.gameObject.SetActive(false);
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
    }

}