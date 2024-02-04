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

    public GameObject Spawn(){
        return Spawn("bullet", Vector3.zero, Quaternion.identity);
    }

    public GameObject Spawn(string name, Vector3 position, Quaternion rotation){
        GameObject bullet = new GameObject(name);
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;

        SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        Bullet bullet1 = bullet.AddComponent<Bullet>();
        bullet1.damage = damage;
        
        return bullet;
    }    
}