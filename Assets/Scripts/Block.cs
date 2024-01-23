using TMPro;
using UnityEngine;

public class Block : MonoBehaviour {
    public int health;
    public Sprite sprite;
    public GameObject player;

    public Block(Sprite sprite, GameObject player){
        this.sprite = sprite;
        this.player = player;

        health = 5;
    }

    public Block(int health, Sprite sprite, GameObject player){
        this.health = health;
        this.sprite = sprite;
        this.player = player;
    }

    public void Explode(){ // automatically called when block dies
        Destroy(gameObject);
    }

    // functions for spawning in blocks is in Mothership.cs
    // Block inherits from MonoBehavior so you can't directly create a new Block object

    private void OnTriggerEnter2D(Collider2D other) { // when a bullet hits the ship
        if (LayerMask.LayerToName(other.gameObject.layer) == "Bullet"){ // make sure the collider is a bullet
            int damage = other.gameObject.GetComponent<Bullet>().damage;
            health -= damage;
            Destroy(other.gameObject); // destroy bullet
        }
        
    }

    private void Update() {
        if (!GlobalVariables.isAlive){
            Destroy(gameObject);
            return;
        }
        transform.position -= new Vector3(0, Time.deltaTime * Constants.gravity, 0); // slowly descend

        if (health <= 0){ // explode if health is 0 or less
            Explode();
        }

        if (transform.position.y < -10){ // destroy the block when it's passed the player's screen
            Destroy(gameObject);
        }

        if (transform.position.y < player.transform.position.y + 1){ // destroy the player if the block's too low
            player.GetComponent<Player>().Explode();
        }

        transform.GetChild(0).GetComponent<TMP_Text>().text = health + ""; // update the health display
    }
}