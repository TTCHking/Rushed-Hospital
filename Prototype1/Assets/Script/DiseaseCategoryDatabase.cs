using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiseaseCategory Database", menuName = "ScriptableObject/DiseaseCategoryDatabase")]
public class DiseaseCategoryDatabase : ScriptableObject
{
    public AllBoneDisease boneDiseaseDatabase;
    public AllVirusDisease virusDiseaseDatabase;
    public AllParasiteDisease parasiteDiseaseDatabase;
}
