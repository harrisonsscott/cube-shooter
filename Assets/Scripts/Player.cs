using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Ship
{
    public Mothership mothership;
    public int coins;
    private Vector2 previousTouch; // the touch position of the last frame
    private GameObject bulletClone;
    private float index;
    public Player(Sprite sprite) : base(sprite) {
        this.sprite = sprite;
        coins = 0;

        if (Application.isEditor && Constants.debugMode){ // debug mode
            health = 1000;
            maxHealth = health;
        }
    }

    private new void Update() {
        
        base.Update();

        transform.position = new Vector3(transform.position.x, -4, transform.position.z);

        if (Input.touchCount > 0 && GlobalVariables.isAlive){
            Touch touch = Input.GetTouch(0);
            if (previousTouch == null || touch.phase == TouchPhase.Began){ // touch just started
                previousTouch = touch.position;
            } else {
                float difference = (touch.position.x - previousTouch.x) * Time.deltaTime / 8;

                float border = Camera.main.orthographicSize * Camera.main.aspect; // finds how far the ship can go without going off camera

                difference = Mathf.Clamp(difference + transform.position.x, -border + transform.localScale.x, border - transform.localScale.x); // prevent the ship from going off camera

                transform.position = new Vector3(difference, -4, 0);
            }
            previousTouch = touch.position;
        }
    }

    public override void Explode(){
        base.Explode();
        GlobalVariables.isAlive = false;

        // enable/disable UI elements depending on whether or not the player is alive
        for (int i = 0; i < mothership.enableOnPlay.Count; i++)
            mothership.enableOnPlay[i].SetActive(false);
        
        for (int i = 0; i < mothership.disableOnPlay.Count; i++)
            mothership.disableOnPlay[i].SetActive(true);
    }

    public override GameObject Spawn(string name)
    {
        return Spawn(name, Vector3.zero, Quaternion.identity);
    }

    public override GameObject Spawn(string name, Vector3 position, Quaternion rotation){
        GameObject ship = base.Spawn(name, position, rotation); // create basic ship

        Player player = ship.AddComponent<Player>(); // turn ship into player
        player.health = health;
        player.damage = damage;
        player.bulletSpeed = bulletSpeed;
        player.fireRate = fireRate;
        player.bullet = bullet;
        Debug.Log(bullet);
        player.maxHealth = maxHealth;

        Rigidbody2D rigidbody = ship.AddComponent<Rigidbody2D>();
        rigidbody.freezeRotation = true;
        
        ship.layer = LayerMask.NameToLayer("Player");

        return ship;
    }
}
