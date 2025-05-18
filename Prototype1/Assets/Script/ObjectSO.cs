using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/item")]
public class ObjectSO : ScriptableObject { 
    
    public Transform Prefab;
    public Sprite sprite;
    private string objectname;

    [Header("Boiling Result")]
    public GameObject boilingResultPrefab;

    
}

    

