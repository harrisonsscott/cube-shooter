using System;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    public Sprite sprite;

    public Bullet(Sprite sprite){
        damage = 1;

        this.sprite = sprite;
    }

    public Bullet(int damage, Sprite sprite){
        this.damage = damage;
        this.sprite = sprite;
    }

    private void Update() {
        if (!GlobalVariables.isPaused)
            transform.position += new Vector3(0, Mathf.Abs(Time.deltaTime*5f), 0);
    
        // destroy when the game ends or the bullet is out of bounds
        if (!GlobalVariables.isAlive || Math.Abs(transform.position.y) > Camera.main.orthographicSize * 2){
            Destroy(gameObject);
        }
    }

    public GameObject Spawn(){
        return Spawn("bullet", Vector3.zero, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Destroy(gameObject);
    }

    public GameObject Spawn(string name, Vector3 position, Quaternion rotation){
        GameObject bullet = new GameObject(name);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

        SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        Bullet bullet1 = bullet.AddComponent<Bullet>();
        bullet1.damage = damage;

        CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
        collider.excludeLayers = 1 << LayerMask.NameToLayer("Coin") | 1 << LayerMask.NameToLayer("Player"); // exclude coin layer
        collider.radius = 1.28f;

        Rigidbody2D rigidbody = bullet.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        bullet.layer = LayerMask.NameToLayer("Bullet");

        return bullet;
    }    
}