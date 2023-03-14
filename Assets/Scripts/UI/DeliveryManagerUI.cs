using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake() {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.onRecipeSpwaned += DeliveryManager_OnRecipeSpwaned;
        DeliveryManager.Instance.onRecipeComplete += DeliveryManager_onRecipeComplete;
        
        UpdateTemplate();
    }

    private void DeliveryManager_OnRecipeSpwaned(object sender, System.EventArgs e) {
        UpdateTemplate();
    }

    private void DeliveryManager_onRecipeComplete(object sender, System.EventArgs e) {
        UpdateTemplate();
    }

    private void UpdateTemplate() {
        foreach (Transform child in container) {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }
        
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }

}