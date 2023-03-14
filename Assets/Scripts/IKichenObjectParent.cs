using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKichenObjectParent {
    public Transform GetKitchenObjectFollowTransform();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public bool HasKitchenObject();

    public void ClearKitchenObject();

}