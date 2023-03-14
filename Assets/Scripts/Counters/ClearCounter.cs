using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

    [SerializeField] private KitchenObjectSO kitchenObjectSO ;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // There is no KitchenObject on top 

            if (player.HasKitchenObject()) {
                // Player has an object
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                // Player doesnt have an object
                
            }

        } else {
            // There is a KitchenObject on top 
            if (player.HasKitchenObject()) {
                // player is carrying object
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    // Player is carrying a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        // Can add ingredient to plate
                        GetKitchenObject().DestroySelf();
                    } else {
                        // Ingredient is already on the plate
                    }
                } else {
                    // Player is not carrying a plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            } else {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        
    }



    // End Interface Implementation
}

