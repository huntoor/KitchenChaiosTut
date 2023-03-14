using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button retryButton;

    private float recipesDelivered;

    private void Awake() {
        retryButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);

            Time.timeScale = 1f;
        });
    }

    private void Start() {
        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;

        Hide();
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e) {
        if (KitchenGameManager.Instance.IsGameOver()) {
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipeAmount().ToString();

            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}