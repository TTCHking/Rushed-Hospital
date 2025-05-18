using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObject/CraftingRecipe")]
public class CraftingRecipeSO : ScriptableObject
{
    [System.Serializable]
    public class Ingredient
    {
        public ObjectSO item;
        public int slotIndex; // ???? 0=????, 1=????, 2=???
    }

    public List<Ingredient> ingredients;
    public ObjectSO resultItem;
    public GameObject productPrefab;
}
