using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubDisease Database", menuName = "ScriptableObject/ParasiteDiseaseDatabase")]
public class AllParasiteDisease : ScriptableObject
{
    public List<DiseaseData> allParasiteDiseases;
}
