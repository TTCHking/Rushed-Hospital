using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Disease Database", menuName = "ScriptableObject/DiseaseDatabase")]
public class DiseaseDatabase : ScriptableObject
{
    public List<DiseaseData> allDiseases;
}
