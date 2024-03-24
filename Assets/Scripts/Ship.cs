using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class Ship : MonoBehaviour
{
    public int health;
    public int maxHealth; // automatically set to health at the start
    public int damage;
    public float bulletSpeed; // how fast the bullets are
    public float fireRate; // bullets shot per second 
    public Sprite bullet;
    public Sprite sprite;

    private GameObject bulletClone;
    private float index;

    public Ship(Sprite sprite) {
        this.sprite = sprite;
        health = 10;
        damage = 1;
        bulletSpeed = 1;
        fireRate = 1;

        maxHealth = health;
    }

    public Ship(int health, int damage, float bulletSpeed, float fireRate, Sprite bullet, Sprite sprite){
        this.health = health;
        this.damage = damage;
        this.bullet = bullet;
        this.bulletSpeed = bulletSpeed;
        this.fireRate = fireRate;
        this.sprite = sprite;

        maxHealth = health;
    }

    private void Start() {
        InvokeRepeating("Shoot", 1, 1/fireRate); // player will shoot every so often
    }

    public virtual void Shoot(){ // sends a raycast out, and shoots if it hits something
        if (GlobalVariables.isPaused)
            return;

        // int layerMask = ~(1 << gameObject.layer | 1 << LayerMask.NameToLayer("Bullet")); // everything but ship layer and bullet layer
        // RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, layerMask);

        Bullet bulletObject = new Bullet(bullet);
            
        bulletClone = bulletObject.Spawn("bullet", new Vector3(transform.position.x, Math.Max(transform.position.y + 0.5f, -4), 0) , quaternion.identity);
        bulletClone.GetComponent<Bullet>().damage = damage;
    }

    public virtual void Explode(){ // automatically called when the ship dies
        Destroy(gameObject);
        CancelInvoke("Shoot");
        Debug.Log("boom!");
    }

    public virtual void OnTriggerEnter2D(Collider2D other) { // when a bullet hits the ship
        if (LayerMask.LayerToName(other.gameObject.layer) == "Bullet"){ // make sure the collider is a bullet
            if (other.gameObject != bulletClone){ // prevent the ship from dying from its own bullet
                int damage = other.gameObject.GetComponent<Bullet>().damage;
                health -= damage;
                Destroy(other.gameObject); // destroy bullet
            }
        }
        
    }

    public virtual void Update() {
        if (health <= 0 && GlobalVariables.isAlive){ // explode if health is 0 or less
            Explode();
        }
    }

    public virtual GameObject Spawn(string name){
        return Spawn(name, Vector3.zero, Quaternion.identity);
    }
    public virtual GameObject Spawn(string name, Vector3 position, Quaternion rotation){ // loads the ship into the scene
        GameObject ship = new GameObject(name); // create empty game object

        // add components
        SpriteRenderer spriteRenderer = ship.AddComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = ship.AddComponent<BoxCollider2D>();

        spriteRenderer.sprite = sprite; // set sprite
        boxCollider.size = new Vector2(5,5); // set collider size  

        // set transform data
        ship.transform.localScale = new Vector3(0.13f,0.13f,0.13f);
        ship.transform.position = position;
        ship.transform.rotation = rotation;

        // more code is added in the child classes

        return ship;
    }
}
