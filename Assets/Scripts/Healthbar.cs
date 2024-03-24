using UnityEngine;

// place this script in the healthbarRed game object

public class Healthbar : MonoBehaviour {
    private GameObject healthbarGreen; // child of gameObject
    private Player player;
    private float ratio;

    private void Start() {
        healthbarGreen = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update() {
        if (GlobalVariables.isAlive && !player){
            player = FindAnyObjectByType<Player>();
        }

        ratio = player.health / (float)player.maxHealth;

        healthbarGreen.GetComponent<RectTransform>().offsetMax = new Vector2(
            -((1-ratio) * gameObject.GetComponent<RectTransform>().sizeDelta.x), 0);
    }
}