using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

    public event EventHandler OnPlateSpwaned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    [SerializeField] private float spwanPlateTimerMax;

    private float spwanPlateTimer;
    private int plateSpwanAmount;
    private int plateSpwanAmountMax = 4;


    private void Update() {
        spwanPlateTimer += Time.deltaTime;

        if (spwanPlateTimer > spwanPlateTimerMax) {
            // spwan a plate
            spwanPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && plateSpwanAmount < plateSpwanAmountMax) {
                plateSpwanAmount++;

                OnPlateSpwaned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            // Player is not carrying anything
            // player can carry the plate

            if (plateSpwanAmount > 0) {
                // Give player the plate adn removeing it from the counter
                    
                plateSpwanAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}