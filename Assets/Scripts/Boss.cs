using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// add script to mothership

public class Boss : MonoBehaviour
{
    [Header("GameObjects")]
    // for progress bar indicating how far away the boss is
    public GameObject progressRed;
    private GameObject progressGreen; // child of progressRed

    [Header("Variables")]
    public int rowsUntilBoss; // how many rows it take for one boss to spawn, ex: 10 means once every 10 rows
    public float timeUntilBoss;

    [Header("Other")]
    private Mothership mothership; // reference to the mothership class


    private void Start() {
        mothership = GetComponent<Mothership>();

        progressGreen = progressRed.transform.GetChild(0).gameObject;
    }

    private void Update() {
        timeUntilBoss = Mathf.Max(0, rowsUntilBoss * Constants.rowTime - mothership.timeSinceGameStart);
        progressGreen.GetComponent<RectTransform>().offsetMax = new Vector2(
            1 - timeUntilBoss / (rowsUntilBoss * Constants.rowTime) * progressRed.GetComponent<RectTransform>().sizeDelta.x, 0);

        if (timeUntilBoss <= 0){
            SpawnBoss();
        }
        
    }

    public void SpawnBoss(){
        if (mothership.spawnBoss)
            return;

        // spawn boss if there isn't already one
        if (FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length == 0){
            mothership.spawnBoss = true;
            mothership.isBattlingBoss = true;
        }
    }
}
