using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubDisease Database", menuName = "ScriptableObject/VirusDiseaseDatabase")]
public class AllVirusDisease : ScriptableObject
{
    public List<DiseaseData> allVirusDiseases;
}
