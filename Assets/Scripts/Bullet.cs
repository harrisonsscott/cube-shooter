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
        transform.position += new Vector3(0, Time.deltaTime*2f, 0);
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
        collider.excludeLayers = 1 << LayerMask.NameToLayer("Coin"); // exclude coin layer
        collider.radius = 1.28f;

        Rigidbody2D rigidbody = bullet.AddComponent<Rigidbody2D>();
        rigidbody.gravityScale = 0;

        bullet.layer = LayerMask.NameToLayer("Bullet");

        return bullet;
    }    
}