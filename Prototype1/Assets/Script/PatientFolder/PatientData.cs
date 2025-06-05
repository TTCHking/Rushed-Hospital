using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatientData : MonoBehaviour
{
    public string patientName;
    public int age;
    public string ageGroup;
    public string gender;
    public bool isPregnant;
    public string bloodType;
    public DiseaseData disease;
    public ObjectSO correctMedicine;    

    public string rbcStatusText;
    public string wbcStatusText;
    public string neutrophilStatusText;
    public string eosinophilStatusText;
    public string basophilStatusText;
    public string lymphocyteStatusText;
    public string monocyteStatusText;
    public bool hasBloodTested = false;
    public bool hasXrayTested = false;
    public Sprite xrayImage;

    public void Start()
    {
        AssignCorrectMedicine();
    }
    public DiseaseData GetDiseaseData()
    {
        return disease; 
    }

   
    private Dictionary<int, string> savedAnswers = new Dictionary<int, string>();

    
    public void SaveAnswer(int questionIndex, string answer)
    {
        if (!savedAnswers.ContainsKey(questionIndex))
        {
            savedAnswers[questionIndex] = answer;
        }
    }

    //????????????????????????????? Dictionary
    public Dictionary<int, string> GetAllAnswers()
    {
        return savedAnswers;
    }

    //
    public string GetAnswer(int questionIndex)
    {
        return savedAnswers.ContainsKey(questionIndex) ? savedAnswers[questionIndex] : null;
    }

    public BloodStatus GetRBCStatus()
    {
        return disease != null ? disease.RBCStatus : BloodStatus.Unknown;
    }

    public BloodStatus GetWBCStatus()
    {
        return disease != null ? disease.WBCStatus : BloodStatus.Unknown;
    }

    public BloodStatus GetNeutrophilStatus()
    {
        return disease != null ? disease.NeutrophilStatus : BloodStatus.Unknown;
    }

    public BloodStatus GetEosinophilStatus()
    {
        return disease != null ? disease.EosinophilStatus : BloodStatus.Unknown;
    }

    public BloodStatus GetBasophilStatus()
    {
        return disease != null ? disease.BasophilStatus : BloodStatus.Unknown;
    }

    public BloodStatus GetLymphocyteStatus()
    {
        return disease != null ? disease.LymphocyteStatus : BloodStatus.Unknown;
    }

    public BloodStatus GetMonocyteStatus()
    {
        return disease != null ? disease.MonocyteStatus : BloodStatus.Unknown;
    }

    public void UpdateBloodStatusTexts()
    {
        rbcStatusText = BloodStatusToString(GetRBCStatus());
        wbcStatusText = BloodStatusToString(GetWBCStatus());
        neutrophilStatusText = BloodStatusToString(GetNeutrophilStatus());
        eosinophilStatusText = BloodStatusToString(GetEosinophilStatus());
        basophilStatusText = BloodStatusToString(GetBasophilStatus());
        lymphocyteStatusText = BloodStatusToString(GetLymphocyteStatus());
        monocyteStatusText = BloodStatusToString(GetMonocyteStatus());
    }

    public void UpdateDiseaseIcon()
    {
        xrayImage = disease != null ? disease.diseaseIcon : null;
    }

    private string BloodStatusToString(BloodStatus status)
    {
        switch (status)
        {
            case BloodStatus.N: return "Normal";
            case BloodStatus.H: return "High";
            case BloodStatus.L: return "Low";
            default: return "Unknown";
        }
    }

    public void AssignCorrectMedicine()
    {
        if (disease != null && disease.CorrectMedicine is ObjectSO medicine)
        {
            correctMedicine = medicine;
            Debug.Log($"{patientName} assigned correct medicine: {correctMedicine.name}");
        }
        else
        {
            Debug.LogWarning("CorrectMedicine in DiseaseData is missing or not ObjectSO");
        }
    }
}
