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
    public float reloadSpeed; // how fast the ship can shoot another bullet 
    public GameObject bullet;
    public Sprite sprite;

    private GameObject bulletClone; // clone of bullet
    private float index;

    public Ship(Sprite sprite) {
        this.sprite = sprite;
        health = 10;
        damage = 1;
        bulletSpeed = 1;
        reloadSpeed = 1;

        maxHealth = health;
    }

    public Ship(int health, int damage, float bulletSpeed, float reloadSpeed, GameObject bullet, Sprite sprite){
        this.health = health;
        this.damage = damage;
        this.bullet = bullet;
        this.bulletSpeed = bulletSpeed;
        this.reloadSpeed = reloadSpeed;
        this.sprite = sprite;

        maxHealth = health;
    }


    public int Shoot(){ // sends a raycast out, and shoots if it hits something
        int layerMask = ~(1 << gameObject.layer | 1 << LayerMask.NameToLayer("Bullet")); // everything but ship layer and bullet layer

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, layerMask);

        if (hit.collider != null){ // raycast hit something, start shooting
            bulletClone = Instantiate(bullet);
            bulletClone.transform.position = transform.position; // set bullet position to ship
            bulletClone.transform.parent = gameObject.transform;
            
            LeanTween.move(bulletClone, transform.position + transform.up * 10, 1/bulletSpeed).setOnComplete(() => {
                Destroy(bulletClone); // destroy bullet when done
            });
            return 1;
        } else {
            return 0;
        }
    }

    public void Explode(){ // automatically called when the ship dies
        Destroy(gameObject);
        Debug.Log("boom!");
    }

    private void OnTriggerEnter2D(Collider2D other) { // when a bullet hits the ship
        if (LayerMask.LayerToName(other.gameObject.layer) == "Bullet"){ // make sure the collider is a bullet
            if (other.gameObject != bulletClone){ // prevent the ship from dying from its own bullet
                int damage = other.gameObject.GetComponent<Bullet>().damage;
                health -= damage;
                Destroy(other.gameObject); // destroy bullet
            }
        }
        
    }

    public void Update() {
        if (health <= 0){ // explode if health is 0 or less
            Explode();
        }

        index += Time.deltaTime; // timer

        if (index > reloadSpeed){ // shoot every so often
            if (Shoot() == 1) // if the ship shot a bullet, reset the timer
                index = 0;
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
        ship.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        ship.transform.position = position;
        ship.transform.rotation = rotation;

        // more code is added in the child classes

        return ship;
    }
}
