using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiveCounterSound : MonoBehaviour {

    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warningSoundTimer;
    bool playWarningSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
        stoveCounter.OnProgressChange += StoveCounter_OnPrgressChange;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e) {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound) {
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }

    private void StoveCounter_OnPrgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        float burnShowProgressAmount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void Update() {

        if (playWarningSound) {   
            warningSoundTimer -= Time.deltaTime;

            if (warningSoundTimer <= 0) {
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}