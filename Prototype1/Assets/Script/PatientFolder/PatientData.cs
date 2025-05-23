using System.Collections;
using System.Collections.Generic;
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

    public DiseaseData GetDiseaseData()
    {
        return disease; // ????????????????????????? diseaseData ???? private
    }

    private Dictionary<int, string> savedAnswers = new Dictionary<int, string>();

    // ??????????????????????????????????????????
    public void SaveAnswer(int questionIndex, string answer)
    {
        if (!savedAnswers.ContainsKey(questionIndex))
        {
            savedAnswers[questionIndex] = answer;
        }
    }

    // ????????????????? (UIManager)
    public Dictionary<int, string> GetAllAnswers()
    {
        return savedAnswers;
    }

    // Optional: ??????????????????????
    public string GetAnswer(int questionIndex)
    {
        return savedAnswers.ContainsKey(questionIndex) ? savedAnswers[questionIndex] : null;
    }
}
