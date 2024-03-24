using UnityEngine;
using UnityEngine.XR;

public class Coin : MonoBehaviour {
    int value = 0; // how many coins the coin is worth
    Sprite sprite;

    public Coin(Sprite sprite){
        value = 1;
        this.sprite = sprite;
    }

    public Coin(int value, Sprite sprite){
        this.value = value;
        this.sprite = sprite;
    }

    private void Update() {
        if (!GlobalVariables.isAlive){ // disappear when the player dies
            Destroy(gameObject);
            return;
        }
        if (!GlobalVariables.isPaused){
            transform.position -= new Vector3(0, Time.deltaTime * Constants.gravity / 2, 0); // slowly descend (slower than gravity to make them easier to collect)
        }
    }

    public void Collect(GameObject player){ // coin moves off the screen and is given to the player
        // Destroy(gameObject.GetComponent<CircleCollider2D>()); // remove collider to prevent calling this function more than once
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        float screenHeight = Camera.main.orthographicSize;

        float tweenTime = Mathf.Sqrt(screenWidth + screenHeight)/4; // tween moves a uniform amount every second
        LeanTween.move(gameObject, new Vector3(screenWidth,screenHeight,0), tweenTime).setOnComplete(() => {
            player.GetComponent<Player>().mothership.coins += value;
            Destroy(gameObject);
        });
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player"){ // is the collision the player
            Collect(other.gameObject);
        }
        
    }

    public GameObject Spawn(Vector3 position){
        GameObject coin = new GameObject("coin");
        Coin self = coin.AddComponent<Coin>();
        SpriteRenderer spriteRenderer = coin.AddComponent<SpriteRenderer>();
        CircleCollider2D collider = coin.AddComponent<CircleCollider2D>();

        coin.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        coin.transform.position = position;
        self.value = value;
        spriteRenderer.sprite = sprite;
        collider.radius = 0.16f;
        collider.isTrigger = true;

        coin.layer = LayerMask.NameToLayer("Coin");

        return coin;
    }
}