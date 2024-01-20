using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;

    public Bullet(){
        damage = 1;
    }

    public Bullet(int damage){
        this.damage = damage;
    }    
}