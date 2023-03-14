using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {

    public static DeliveryManager Instance { get; private set;}

    public event EventHandler onRecipeSpwaned;
    public event EventHandler onRecipeComplete;
    public event EventHandler onRecipeSuccess;
    public event EventHandler onRecipeFail;

    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> waitingRecipeSOList;
    private float spwanRecipeTimer; 
    private float spwanRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipeAmount;

    private void Awake() {
        Instance = this;

        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        spwanRecipeTimer -= Time.deltaTime;

        if (spwanRecipeTimer <= 0) {
            spwanRecipeTimer = spwanRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax) {
                // can spwan a new recipe 
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);

                onRecipeSpwaned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
        for (int i = 0; i < waitingRecipeSOList.Count; i++) {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count) {
                // Number of ingerdients on plate is the same as in the recipe
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList) {
                    // Cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList()) {
                        // Cycling through all ingredients in the plate
                        if (plateKitchenObjectSO == recipeKitchenObjectSO) {
                            ingredientFound = true;
                            break;
                        }
                    }
                    if (!ingredientFound) {
                        plateContentMatchesRecipe = false;
                    }
                }
                if (plateContentMatchesRecipe) {
                    // Player Deliverd the correct recipe
                    successfulRecipeAmount++;

                    waitingRecipeSOList.RemoveAt(i);

                    onRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    onRecipeComplete?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // Player didnt deliver correct recipe
        Debug.Log("Player didnt deliver correct recipe");
        onRecipeFail?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipeAmount() {
        return successfulRecipeAmount;
    }
}