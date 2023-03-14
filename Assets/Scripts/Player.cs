using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKichenObjectParent {

    public static Player Instance{ get; private set; }

    // you can also us this ->
    // private static Player instance;
    // public static Player Instance {
    //     get {
    //         return instance;
    //     }
    //     private set {
    //         instance = value;
    //     }
    // }

    // Event for changing the color of counter when seleceted
    public event EventHandler onPlayerPickSomething;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArgs: EventArgs {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 11f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObectHoldPoint;



    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one player");
        }

        Instance = this;
    }
    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }
    
    private void Update() {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteraction() {
        Vector2 inputVector = gameInput.GetMovementVecotrNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float interactionDistance = 2f;

        if (!(moveDir == Vector3.zero)) {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit rayCastHit, interactionDistance, countersLayerMask)) {
            if (rayCastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                // baseCounter.Interact();
                if (baseCounter !=  selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVecotrNormalized();


        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // trying to move in the X direction
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = (moveDir.x < -0.5f || moveDir.x > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if(canMove) {
                // Can move only on the X;
                moveDir = moveDirX;
            } else {
                // trying to move in the Z direction
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = (moveDir.z < -0.5f || moveDir.z > 0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    // Can move only on Y
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }


        float rotationSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);

        // check if player is moving
        isWalking = moveDir != Vector3.zero;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArgs {
            selectedCounter = selectedCounter
        });

                    
    }


    // Implementing IKitchenObjectParent function
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            onPlayerPickSomething?.Invoke(this, EventArgs.Empty);
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
    // End Implementation
}

