using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Disease")]
public class DiseaseData : ScriptableObject
{
    public string diseaseName;
    public DiseaseType diseaseType;
    [TextArea] public string description;
    public Sprite diseaseIcon;
    public ScriptableObject Asking;
    public ScriptableObject CorrectMedicine;

    public BloodStatus RBCStatus;
    public BloodStatus WBCStatus;
    public BloodStatus NeutrophilStatus;
    public BloodStatus EosinophilStatus;
    public BloodStatus BasophilStatus;
    public BloodStatus LymphocyteStatus;
    public BloodStatus MonocyteStatus;
}
public enum BloodStatus
{
    Unknown, //????? 
    N, // Normal
    H, // High
    L  // Low
}
public enum DiseaseType
{
    Bone,
    Parasite,
    Virus
}

