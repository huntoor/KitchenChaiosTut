using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter {

    public static event System.EventHandler onAnyObjectTrashed;
    new public static void ResetStaticData() {
            onAnyObjectTrashed = null;
    }

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            // Player is carrying an object 
            // we can distroy the object
            player.GetKitchenObject().DestroySelf();

            onAnyObjectTrashed?.Invoke(this, System.EventArgs.Empty);
        } else {
            // Player is not carrying anything
        }
    }
}