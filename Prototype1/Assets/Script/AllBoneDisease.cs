using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubDisease Database", menuName = "ScriptableObject/BoneDiseaseDatabase")]
public class AllBoneDisease : ScriptableObject
{
    public List<DiseaseData> allBoneDiseases;
}
