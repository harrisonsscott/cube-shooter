using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// this script handles all the ships

public class Mothership : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite playerSprite; // player ship sprite
    public Sprite enemySprite; // enemy ship sprite
    public Sprite blockSprite; // just a white square

    [Header("Objects")]
    public GameObject bullet; // player bullet GO
    public GameObject bulletRed; // enemy bullet GO
    public HealthBar healthBar; // background element of the health bar
    private Player player;
    private Enemy enemy;
    private List<Block> blocks;

    private GameObject playerGO; // GO stands for GameObject
    private GameObject enemyGO;
    private List<GameObject> blocksGO;
    [Header("Variables")]
    public int level; // how many rows of blocks the player has gone through
    public float screenWidth; // how far can an object deviate from 0 without going off camera

    private Gradient gradient;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;
    private void Start() {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;

        // gradient for coloring blocks
        gradient = new Gradient();
        colorKeys = new GradientColorKey[3];
        alphaKeys = new GradientAlphaKey[3];

        // blend color from green to yellow to red
        colorKeys[0] = new GradientColorKey(Color.green, 0);
        colorKeys[1] = new GradientColorKey(Color.yellow, 0.5f);
        colorKeys[2] = new GradientColorKey(Color.red, 1);

        // set the alpha for every color key to 1
        for (int i = 0; i < colorKeys.Length; i++){
            alphaKeys[i] = new GradientAlphaKey(1, colorKeys[i].time);
        }

        gradient.SetKeys(colorKeys, alphaKeys);


        player = new Player(playerSprite){ // create new player object
            bullet = bullet
        };
        playerGO = player.Spawn("Player", new Vector3(2, -4, 0), Quaternion.identity); // instantiate the player
        player = playerGO.GetComponent<Player>(); // update the player object to the one used by the GO

        // enemy = new Enemy(enemySprite, player){ // create new enemy object
        //     bullet = bulletRed
        // };
        // enemyGO = enemy.Spawn("Enemy"); // instantiate enemy
        // enemy = enemyGO.GetComponent<Enemy>(); // update the enemy object to the one used by the GO

        blocks = new List<Block>();
        blocksGO = new List<GameObject>();

        // instatiate all the blocks
        for (int i = -(int)Mathf.Floor(screenWidth) - 2; i < screenWidth + 2; i++){
            GameObject blockGO = spawnBlock("block", new Vector3(i*0.9f, 0, 0));
            blockGO.layer =  LayerMask.NameToLayer("Block");

            blocksGO.Add(blockGO);
            blocks.Add(blockGO.GetComponent<Block>());
        }

    }

    private void Update() {
        healthBar.Refresh(player.health / (float)player.maxHealth); // update the player's health bar
    }

    // instantiates a block in the world space
    public GameObject spawnBlock(string name, Vector3 position){
        GameObject block = new GameObject(name);

        // add components
        SpriteRenderer spriteRenderer = block.AddComponent<SpriteRenderer>();
        Block blockComponent = block.AddComponent<Block>();
        BoxCollider2D boxCollider = block.AddComponent<BoxCollider2D>();

        boxCollider.size = new Vector2(1,1);

        blockComponent.sprite = blockSprite;
        blockComponent.health = 65;

        spriteRenderer.sprite = blockSprite;
        spriteRenderer.color = gradient.Evaluate(blockComponent.health / 100f); // color based on health

        block.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        block.transform.position = position;
        block.transform.parent = transform;

        return block;
    }
}
