using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighShearMixer : MonoBehaviour, IInteractable
{
    public CraftingRecipeSO recipeSO;
    public Transform spawnProduct;
    public List<CraftingRecipeSO> recipes;
    public int[] craftingSlotIndices; 
    public int outputSlotIndex; 

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
        foreach (var recipe in recipes) 
        {
            bool match = true;

            
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
               
                var outputObj = inventoryManager.GetObjectAt(outputSlotIndex);
                if (outputObj != null)
                {
                    Debug.LogWarning("Output slot is not empty!");
                    return;
                }

               
                foreach (var ingredient in recipe.ingredients)
                {
                    inventoryManager.RemoveItemAt(ingredient.slotIndex);
                }

                if (recipe.productPrefab != null)
                {
                    Vector3 spawnCraft = spawnProduct != null ? spawnProduct.position : transform.position;
                    Instantiate(recipe.productPrefab, spawnCraft, Quaternion.identity);
                }

                Debug.Log("Crafted: " + recipe.resultItem.name);
                return;  
            }
        }

        Debug.Log("No matching recipe.");
    }
}

