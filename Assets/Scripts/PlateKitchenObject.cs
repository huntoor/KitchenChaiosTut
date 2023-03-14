using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject {

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs {
        public KitchenObjectSO kitchenObjectSO;
    }


    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSO;
    
    private List<KitchenObjectSO> KitchenObjectSOList;

    private void Awake() {
        KitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO) {
        if (KitchenObjectSOList.Contains(kitchenObjectSO)) {
            // Plate Containes the ingredient
            return false;
        } else {
            if (validKitchenObjectSO.Contains(kitchenObjectSO)) {
                KitchenObjectSOList.Add(kitchenObjectSO);

                OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                    kitchenObjectSO = kitchenObjectSO
                });

                return true;
            } else {
                // Not a valid Ingredient
                return false;
            }
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() {
        return KitchenObjectSOList;
    }
}