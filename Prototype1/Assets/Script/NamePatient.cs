using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/namePatient")]
public class NameDatabase : ScriptableObject
{
    public List<string> patientMaleName;
    public List<string> patientFemaleName;
    public int patientAge;
    public List<string> patientGender;
    public bool isPregnant;
    public List<string> patientBloodType;
}
