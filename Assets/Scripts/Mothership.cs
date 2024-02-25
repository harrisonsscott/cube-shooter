using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

// this script handles all the ships

public class Mothership : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite playerSprite; // player ship sprite
    public Sprite enemySprite; // enemy ship sprite
    public Sprite blockSprite; // just a white square
    public Sprite coinSprite;

    [Header("Objects")]
    public List<GameObject> disableOnPlay; // shows in the meu but not the game, ex: shop
    public List<GameObject> enableOnPlay; // shows in the game but not the menu, ex: health bar
    public GameObject blockTextTemplate; // empty gameobject with TMP_TEXT
    public Sprite bullet; // player bullet GO
    public Sprite bulletRed; // enemy bullet GO
    public GameObject explosionParticle; // particle system that plays on a ship's death
    public TMP_Text coinsDisplay; // text in the top right corner that displays the amount of coins the player has
    private Player player;
    private Enemy enemy;
    private List<Block> blocks;

    private GameObject playerGO; // GO stands for GameObject
    private GameObject enemyGO;
    private List<GameObject> blocksGO;
    [Header("Variables")]
    public int level; // how many rows of blocks the player has gone through
    public float screenWidth; // width of the camera's view in world space
    public float screenHeight; // height of the camera's view in world space
    private bool hasExploded; // bool to make sure the explosion effect doesn't play twice

    private Gradient gradient;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;

    public void Start(){
        // StartGame();
    }

    public void StartGame() {
        // enable/disable UI elements depending on whether or not the player is alive
        for (int i = 0; i < enableOnPlay.Count; i++)
            enableOnPlay[i].SetActive(true);
        
        for (int i = 0; i < disableOnPlay.Count; i++)
            disableOnPlay[i].SetActive(false);

        if (GlobalVariables.isAlive){ // unable to start a game if the player is still alive
            return;
        }
        CancelInvoke("spawnRow");
        hasExploded = false;

        GlobalVariables.currentRow = 0;

        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;
        
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
        player.mothership = this; // allow the player to access the mothership
        GlobalVariables.isAlive = true;

        // enemy = new Enemy(enemySprite, player){ // create new enemy object
        //     bullet = bulletRed
        // };
        // enemyGO = enemy.Spawn("Enemy"); // instantiate enemy
        // enemy = enemyGO.GetComponent<Enemy>(); // update the enemy object to the one used by the GO

        blocks = new List<Block>();
        blocksGO = new List<GameObject>();

        // load in blocks every so often
        //spawnRow();
        InvokeRepeating("spawnRow", 2f, Constants.rowTime);

    }

    private void Update() {
        if (GlobalVariables.isAlive){
            explosionParticle.transform.position = playerGO.transform.position + new Vector3(0,1,0);
            coinsDisplay.text = "Coins: " + player.coins;
        } else {
            //play explosion effect on player death
            if (explosionParticle && !explosionParticle.GetComponent<ParticleSystem>().isPlaying && !hasExploded){
                hasExploded = true;
                explosionParticle.SetActive(true);
                explosionParticle.GetComponent<ParticleSystem>().Play();
                LeanTween.value(0, 1, 1).setOnComplete(() => {
                    explosionParticle.SetActive(false);
                });
            }
        }
    }

    // instantiates a block in the world space
    public GameObject spawnBlock(string name, Vector3 position, int health){
        GameObject block = new GameObject(name);
        GameObject text = Instantiate(blockTextTemplate);

        // add components
        SpriteRenderer spriteRenderer = block.AddComponent<SpriteRenderer>();
        Block blockComponent = block.AddComponent<Block>();
        BoxCollider2D boxCollider = block.AddComponent<BoxCollider2D>();

        boxCollider.size = new Vector2(1,1);
        boxCollider.isTrigger = true;

        blockComponent.sprite = blockSprite;
        blockComponent.health = health;
        blockComponent.player = playerGO;
        blockComponent.coinSprite = coinSprite;

        spriteRenderer.sprite = blockSprite;
        spriteRenderer.color = gradient.Evaluate(blockComponent.health / 30f); // color based on health

        block.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
        block.transform.position = position;
        block.transform.parent = transform;

        text.transform.SetParent(block.transform);
        text.GetComponent<RectTransform>().localPosition = Vector3.zero;
        text.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        text.GetComponent<TMP_Text>().text = blockComponent.health + "";


        block.layer = LayerMask.NameToLayer("Block");

        return block;
    }

    // adds a row of blocks to the blocks list
    private void spawnRow(){
        for (int i = -(int)Mathf.Floor(screenWidth) - 2; i < screenWidth + 2; i++){
            GameObject blockGO = spawnBlock(
                "block", new Vector3(i*0.9f, screenHeight+1, 0), (int)(GlobalVariables.currentRow/5) + Random.Range(1, 5));

            blocksGO.Add(blockGO);
            blocks.Add(blockGO.GetComponent<Block>());
        }
        GlobalVariables.currentRow += 1;
    }
}
