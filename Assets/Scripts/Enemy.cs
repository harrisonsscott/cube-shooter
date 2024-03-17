using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    Player player; // reference to player 
    public Enemy(Sprite sprite, Player player) : base(sprite){
        this.sprite = sprite;
        this.player = player;
    }

    private new void Update() {
        base.Update(); // retain Ship.cs's update function
        // transform.position -= new Vector3(0, Constants.gravity, 0);
    }

    public void Follow(){
        // automatically follow the player
        // transform.position = Vector3.Lerp(transform.position, player.transform.position, 0.1f); // follow player
    }
    

    public override GameObject Spawn(string name){
        return Spawn(name, Vector3.zero, Quaternion.Euler(0,0,180));
    }

    public override GameObject Spawn(string name, Vector3 position, Quaternion rotation){
        GameObject ship = base.Spawn(name, position, rotation); // create basic ship

        Enemy enemy = ship.AddComponent<Enemy>(); // turn the ship into an enemy
        enemy.health = health;
        enemy.damage = damage;
        enemy.bulletSpeed = bulletSpeed;
        enemy.fireRate = fireRate;
        enemy.bullet = bullet;

        ship.layer = LayerMask.NameToLayer("Enemy");

        return ship;
    }

}
