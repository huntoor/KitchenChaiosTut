using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour {

    public static KitchenGameManager Instance { get; private set; }

    private enum State {
        waitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    public event EventHandler OnStateChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private State state;
    // private float waitingTimer = 0.5f;
    private float countDownTimer = 3f;
    private float gamePlayTimer;
    private float gamePlayTimerMax = 300f;
    private bool isGamePaused = false;

    private void Awake() {
        state = State.waitingToStart;

        Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
        TogglePauseGame();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (state == State.waitingToStart) {
            state = State.CountdownToStart;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update() {
        switch (state) {
            case State.waitingToStart:
                break;

            case State.CountdownToStart:
                countDownTimer -= Time.deltaTime;
                if (countDownTimer < 0f) {
                    state = State.GamePlaying;

                    gamePlayTimer = gamePlayTimerMax;

                    OnStateChange?.Invoke(this, EventArgs.Empty);

                }
                break;

            case State.GamePlaying:
                gamePlayTimer -= Time.deltaTime;
                if (gamePlayTimer < 0f) {
                    state = State.GameOver;

                    OnStateChange?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                break;

        }
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive() {
        return state == State.CountdownToStart;
    }

    public float GetToCountDownTimer() {
        return countDownTimer;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetGamePlayTimerNormalized() {
        return 1 - (gamePlayTimer / gamePlayTimerMax);
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;
        if (isGamePaused) {
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}