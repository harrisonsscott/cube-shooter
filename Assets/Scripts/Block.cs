using UnityEngine;

public class Block : MonoBehaviour {
    public int health;
    public Sprite sprite;

    public Block(Sprite sprite){
        this.sprite = sprite;

        health = 5;
    }

    public Block(int health, Sprite sprite){
        this.health = health;
        this.sprite = sprite;
    }

    public void Explode(){ // automatically called when block dies
        Destroy(gameObject);
    }

    // functions for spawning in blocks is in Mothership.cs
    // Block inherits from MonoBehavior so you can't directly create a new Block object

    private void Update() {
        transform.position -= new Vector3(0, Time.deltaTime * Constants.gravity, 0);
        // if (health <= 0){ // explode if health is 0 or less
        //     Explode();
        // }

        if (transform.position.y < -10){ // destroy the block when it's passed the player's screen
            Destroy(gameObject);
        }
    }
}