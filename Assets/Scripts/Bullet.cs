using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    public Sprite sprite;
    public Vector3 direction;
    public int layerOwner;

    public Bullet(Sprite sprite){
        damage = 1;
        direction = new Vector3(0, 1, 0);
        this.sprite = sprite;
    }

    public Bullet(int damage, Sprite sprite, Vector3 direction){
        this.damage = damage;
        this.sprite = sprite;
        this.direction = direction;
    }

    private void Update() {
        if (!GlobalVariables.isPaused)
            transform.position += Vector3.Scale(new Vector3(0, Time.deltaTime*5f, 0), direction);
    
        // destroy when the game ends or the bullet is out of bounds
        if (!GlobalVariables.isAlive || Math.Abs(transform.position.y) > Camera.main.orthographicSize * 2){
            Destroy(gameObject);
        }
    }

    public GameObject Spawn(){
        return Spawn("bullet", Vector3.zero, Quaternion.identity, new Vector3(0,1,0), LayerMask.NameToLayer("Player"));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }

    public GameObject Spawn(string name, Vector3 position, Quaternion rotation, Vector3 direction, int layerOwner){
        GameObject bullet = new GameObject(name);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

        SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        Bullet bullet1 = bullet.AddComponent<Bullet>();
        bullet1.damage = damage;
        bullet1.direction = direction;
        bullet1.layerOwner = layerOwner;

        CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
        collider.excludeLayers = 1 << LayerMask.NameToLayer("Coin") | 1 << layerOwner; // exclude coin layer
        collider.radius = 1.28f;
        collider.isTrigger = true;

        // CircleCollider2D collider2 = Instantiate(collider);
        // collider2.isTrigger = true;

        Rigidbody2D rigidbody = bullet.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        bullet.layer = LayerMask.NameToLayer("Bullet");

        return bullet;
    }    
}