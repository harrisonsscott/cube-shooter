using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script handles all the ships

public class Mothership : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite playerSprite; // player ship sprite
    public Sprite enemySprite; // enemy ship sprite

    [Header("Objects")]
    public GameObject bullet; // player bullet GO
    public GameObject bulletRed; // enemy bullet GO
    public HealthBar healthBar; // background element of the health bar

    [SerializeField]
    private Player player;
    [SerializeField]
    private Enemy enemy;

    private GameObject playerGO;
    private GameObject enemyGO;
    private void Start() {
        player = new Player(playerSprite){ // create new player object
            bullet = bullet
        };
        playerGO = player.Spawn("Player", new Vector3(2, -4, 0), Quaternion.identity); // instantiate the player
        player = playerGO.GetComponent<Player>(); // update the player object to the one used by the GO

        enemy = new Enemy(enemySprite, player){ // create new enemy object
            bullet = bulletRed
        };
        enemyGO = enemy.Spawn("Enemy"); // instantiate enemy
        enemy = enemyGO.GetComponent<Enemy>(); // update the enemy object to the one used by the GO

    }

    private void Update() {
        healthBar.Refresh(player.health / (float)player.maxHealth);
    }
}
