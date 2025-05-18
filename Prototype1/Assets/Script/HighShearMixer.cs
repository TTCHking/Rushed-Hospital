using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighShearMixer : MonoBehaviour, IInteractable
{
    public CraftingRecipeSO recipeSO;
    public Transform spawnProduct;
    public List<CraftingRecipeSO> recipes;
    public int[] craftingSlotIndices; // ???? [0,1,2]
    public int outputSlotIndex; // ???? output ???? index = 4

    public InventoryManager inventoryManager;

    [SerializeField] private Canvas CraftingUI;

    public void Start()
    {
        CraftingUI.gameObject.SetActive(false);
    }
    public void Interaction(GameObject interactor)
    {
        CraftingUI.gameObject.SetActive(true);
    }

    public void ExitCraftingUI()
    {
        CraftingUI.gameObject.SetActive(false);
    }


    public void CheckRecipes()
    {
        foreach (var recipe in recipes)  // ?????????????????? recipes
        {
            bool match = true;

            // 1. ?????????????????????
            foreach (var ingredient in recipe.ingredients)
            {
                var invObj = inventoryManager.GetObjectAt(ingredient.slotIndex);
                if (invObj == null || invObj != ingredient.item)
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                // ??????????? output ???????????
                var outputObj = inventoryManager.GetObjectAt(outputSlotIndex);
                if (outputObj != null)
                {
                    Debug.LogWarning("Output slot is not empty!");
                    return;
                }

                // ????????????????????
                foreach (var ingredient in recipe.ingredients)
                {
                    inventoryManager.RemoveItemAt(ingredient.slotIndex);
                }

                // ???????????????????????? output
               
                // ????? prefab ???????????????
                if (recipe.productPrefab != null)
                {
                    Vector3 spawnCraft = spawnProduct != null ? spawnProduct.position : transform.position;
                    Instantiate(recipe.productPrefab, spawnCraft, Quaternion.identity);
                }

                Debug.Log("Crafted: " + recipe.resultItem.name);
                return;  // ????????????????????????????????????
            }
        }

        Debug.Log("No matching recipe.");
    }
}

