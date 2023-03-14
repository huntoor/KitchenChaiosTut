using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeliveryResultUI : MonoBehaviour {

    private const string POPUP = "PopUp";

    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Color successColor;
    [SerializeField] private Color failColor;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failSprite;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        DeliveryManager.Instance.onRecipeSuccess += DeliveryManager_onRecipeSuccess;
        DeliveryManager.Instance.onRecipeFail += DeliveryManager_onRecipeFail;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_onRecipeSuccess(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        messageText.text = "Delivery\nSuccess";
    }

    private void DeliveryManager_onRecipeFail(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
        animator.SetTrigger(POPUP);
        backgroundImage.color = failColor;
        iconImage.sprite = failSprite;
        messageText.text = "Delivery\nFailed";
    }
}