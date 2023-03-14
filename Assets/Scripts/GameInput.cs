using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameInput : MonoBehaviour {

    private const string PLAYER_PREF_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public event System.EventHandler OnInteractAction;
    public event System.EventHandler OnInteractAlternateAction;
    public event System.EventHandler OnPauseAction;
    public event System.EventHandler OnBindingRebind;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlterante,
        GamePad_Pause,
    }

    private PlayerInputAction playerInputAction;

    private void Awake() {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        if (PlayerPrefs.HasKey(PLAYER_PREF_BINDINGS)) {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREF_BINDINGS));
        }

        playerInputAction.player.Enable();

        playerInputAction.player.Interact.performed += Interact_Performed;
        playerInputAction.player.InteractAlternate.performed += InteractAlternate_Preformed;
        playerInputAction.player.Pause.performed += Pause_Preformed;
    }

    private void OnDestroy() {
        
        playerInputAction.player.Interact.performed -= Interact_Performed;
        playerInputAction.player.InteractAlternate.performed -= InteractAlternate_Preformed;
        playerInputAction.player.Pause.performed -= Pause_Preformed;

        playerInputAction.Dispose();
    }

    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, System.EventArgs.Empty);
    }

    private void InteractAlternate_Preformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAlternateAction?.Invoke(this, System.EventArgs.Empty);
    }

    private void Pause_Preformed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke(this, System.EventArgs.Empty);
    }

    public Vector2 GetMovementVecotrNormalized() {
        
        Vector2 inputVector = playerInputAction.player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding) {
        
        switch (binding) {
            default:
            case Binding.Move_Up:
                return playerInputAction.player.Move.bindings[1].ToDisplayString();
            
            case Binding.Move_Down:
                return playerInputAction.player.Move.bindings[2].ToDisplayString();
            
            case Binding.Move_Left:
                return playerInputAction.player.Move.bindings[3].ToDisplayString();
            
            case Binding.Move_Right:
                return playerInputAction.player.Move.bindings[4].ToDisplayString();
            
            case Binding.Interact:
                return playerInputAction.player.Interact.bindings[0].ToDisplayString();
            
            case Binding.Interact_Alternate:
                return playerInputAction.player.InteractAlternate.bindings[0].ToDisplayString();
            
            case Binding.Pause:
                return playerInputAction.player.Pause.bindings[0].ToDisplayString();
            
            case Binding.GamePad_Interact:
                return playerInputAction.player.Interact.bindings[1].ToDisplayString();
            
            case Binding.GamePad_InteractAlterante:
                return playerInputAction.player.InteractAlternate.bindings[1].ToDisplayString();
            
            case Binding.GamePad_Pause:
                return playerInputAction.player.Pause.bindings[1].ToDisplayString();
            
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound) {
        playerInputAction.player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding) {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.player.Move;
                bindingIndex = 1;
                break;

            case Binding.Move_Down:
                inputAction = playerInputAction.player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputAction.player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputAction.player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.player.Interact;
                bindingIndex = 0;
                break;
            case Binding.Interact_Alternate:
                inputAction = playerInputAction.player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputAction.player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Interact:
                inputAction = playerInputAction.player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlterante:
                inputAction = playerInputAction.player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Pause:
                inputAction = playerInputAction.player.Pause;
                bindingIndex = 1;
                break;
        }


        inputAction
            .PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback => {
                callback.Dispose();
                playerInputAction.player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREF_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, System.EventArgs.Empty);
            })
            .Start();
    }
}