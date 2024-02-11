using Unity.VisualScripting.Dependencies.Sqlite;
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
        transform.position += new Vector3(0, 0.1f, 0);
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
        collider.radius = 1.28f;

        bullet.AddComponent<Rigidbody2D>();

        bullet.layer = LayerMask.NameToLayer("Bullet");
        
        return bullet;
    }    
}