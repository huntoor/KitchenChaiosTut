using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

    public event EventHandler<IHasProgress.OnProgressChangeEventArgs> OnProgressChange;
    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public class OnStateChangeEventArgs : EventArgs {
        public State state;
    }

    public enum State {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;


    private void Start() {
        state = State.Idle;
    }

    private void Update() {        
        if (HasKitchenObject()) {
            // There is an object on top of the sotve
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
                        // Object is fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        state = State.Fried;

                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        burningTimer = 0f;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs{
                            state = state
                        });
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = burningTimer /  burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax) {
                        // Object is fried
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                        state = State.Burned;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs{
                            state = state
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });   
                    }
                    break;
                case State.Burned:
                    state = State.Idle;
                    break;
            }
        }
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            // there is nothing on top

            if (player.HasKitchenObject()) {
                // player have somthing that we can put on top
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    // Player Carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this); // set the object on counter

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs{
                            state = state
                    });

                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            } else {
                // Player doesnt have anything
            }
        } else {
            // there is something on top 
            if (player.HasKitchenObject()) {
                // player is carrying object
                // Check if it is a plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject1)) {
                    // Player is carrying a plate
                    PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        // Can add ingredient to plate
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;

                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs{
                            state = state
                        });

                        OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                            progressNormalized = 0f
                        });

                    } else {
                        // Ingredient is already on the plate
                    }
                }

            } else {
                // Player is not carrying anything
                // Now player can carry the object on top
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChange?.Invoke(this, new OnStateChangeEventArgs{
                    state = state
                });

                OnProgressChange?.Invoke(this, new IHasProgress.OnProgressChangeEventArgs {
                    progressNormalized = 0f
                });   
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        } else {
            return null;
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
            foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
            foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried() {
        return state == State.Fried;
    }
}
