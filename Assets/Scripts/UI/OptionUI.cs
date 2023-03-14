using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionUI : MonoBehaviour {

    public static OptionUI Instance { get; private set; }

    [SerializeField] private Button suondEffectButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI suondEffectText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Transform pressToRebindKey;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private Button moveUpButton;

    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private Button moveDownButton;

    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private Button moveLeftButton;

    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private Button moveRightButton;

    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Button interactButton;

    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private Button gamepadInteractButton;

    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private Button interactAltButton;

    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private Button gamepadInteractAltButton;

    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Button gamepadPauseButton;
    
    private Action onCloseButtonAction;

    private void Awake() {
        Instance = this;

        suondEffectButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();

            UpdateVisual();
        });

        closeButton.onClick.AddListener(() => {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        interactAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Alternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        interactAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact_Alternate); });
        pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });

        gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Interact); });
        gamepadInteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_InteractAlterante); });
        gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Pause); });
    }

    private void Start() {
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }
    
    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void UpdateVisual() {
        suondEffectText.text = "Sound Effect: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        gamepadInteractAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlterante);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);

    }

    public void Show(Action onCloseButtonAction) {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        suondEffectButton.Select();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() {
        pressToRebindKey.gameObject.SetActive(true);
    }

    private void HidePressToRebindKey() {
        pressToRebindKey.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });    
    }
}