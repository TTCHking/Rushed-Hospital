using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Asking")]
public class AskingDatabase : ScriptableObject
{
    [Header("When did these symthoms start?")]
    public string[] answerofquestion1;

    [Header("Do you feel any pain anywhere?")]
    public string[] answerofquestion2;

    [Header("Do you have any history of injuruies or accident relate to this?")]
    public string[] answerofquestion3;






}
    
