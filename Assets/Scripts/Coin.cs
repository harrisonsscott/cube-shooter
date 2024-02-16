using UnityEngine;

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
        transform.position -= new Vector3(0, Time.deltaTime * Constants.gravity, 0); // slowly descend
    }

    public GameObject Spawn(Vector3 position){
        GameObject coin = new GameObject("coin");
        Coin self = coin.AddComponent<Coin>();
        SpriteRenderer spriteRenderer = coin.AddComponent<SpriteRenderer>();

        coin.transform.position = position;
        self.value = value;
        spriteRenderer.sprite = sprite;

        return coin;
    }
}