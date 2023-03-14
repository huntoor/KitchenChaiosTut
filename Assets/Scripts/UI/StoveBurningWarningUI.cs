using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurningWarningUI : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    private void Start() {
        stoveCounter.OnProgressChange += stoveCounter_OnProgressChange;

        Hide();
    }

    private void stoveCounter_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
            float burnShowProgressAmount = 0.5f;
            bool show = stoveCounter.IsFried() && (e.progressNormalized >= burnShowProgressAmount);

            if (show) {
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