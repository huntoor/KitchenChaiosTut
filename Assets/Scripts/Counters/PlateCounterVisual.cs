using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour {

    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private Transform counterTopPoint;

    private List<GameObject> platesVisualGameObjectList;

    public void Awake() {
        platesVisualGameObjectList = new List<GameObject>();
    }
    
    public void Start() {
        platesCounter.OnPlateSpwaned += PlatesCounter_OnplatesSpawn;
        platesCounter.OnPlateRemoved += PlatesCounter_OnplatesRemove;
    }

    public void PlatesCounter_OnplatesSpawn(object sender, System.EventArgs e) {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

        float plateYOffset = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateYOffset * platesVisualGameObjectList.Count, 0);
        platesVisualGameObjectList.Add(plateVisualTransform.gameObject);

    }

    public void PlatesCounter_OnplatesRemove(object sender, System.EventArgs e) {
        GameObject plateGameObject = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];

        platesVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
}