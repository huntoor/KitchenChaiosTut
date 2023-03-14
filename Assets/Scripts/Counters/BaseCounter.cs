using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKichenObjectParent {

    public static event EventHandler onAnyObjectPlacedHere;

    public static void ResetStaticData() {
        onAnyObjectPlacedHere = null;
    }


    [SerializeField] private Transform counterTopPoint;


    private KitchenObject kitchenObject;

    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(Player player) {
        // Debug.LogError("BaseCounter.InteractAlternate()");
    }

    // Interface Fumctions
    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            onAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public bool HasKitchenObject() {
        return (kitchenObject != null);
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }
    // End Interface Implementation

}
