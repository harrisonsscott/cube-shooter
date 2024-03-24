using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using System;
using System.Runtime.InteropServices;

// this script handles all the ships

public class Mothership : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite playerSprite; // player ship sprite
    public Sprite enemySprite; // enemy ship sprite
    public Sprite blockSprite; // just a white square
    public Sprite coinSprite;

    [Header("Objects")]
    public GameObject fireRateButton; // upgrade button in menu
    public GameObject damageButton; // upgrade button in menu
    public List<GameObject> disableOnPlay; // shows in the meu but not the game, ex: shop
    public List<GameObject> enableOnPlay; // shows in the game but not the menu, ex: health bar
    public GameObject blockTextTemplate; // empty gameobject with TMP_TEXT
    public Sprite bullet; // player bullet GO
    public Sprite bulletRed; // enemy bullet GO
    public GameObject explosionParticle; // particle system that plays on a ship's death
    public TMP_Text scoreDisplay; // text that displays the player's high score
    public TMP_Text coinsDisplay; // text in the top right corner that displays the amount of coins the player has
    private Player player;
    private Enemy enemy;
    private List<Block> blocks;

    private GameObject playerGO; // GO stands for GameObject
    private GameObject enemyGO;
    private List<GameObject> blocksGO;
    [Header("Variables")]
    [SerializeField]
    public double coins;
    [SerializeField]
    // how scoring works: you get 1 point per block you don't destroy, but you get [row number + 1] amount of points if you do destroy it.
    public double score;
    public int level; // how many rows of blocks the player has gone through
    [SerializeField]
    public float screenWidth; // width of the camera's view in world space
    [SerializeField]
    public float screenHeight; // height of the camera's view in world space
    public float timeSinceGameStart;
    private bool hasExploded; // bool to make sure the explosion effect doesn't play twice

    private Gradient gradient;
    private GradientColorKey[] colorKeys;
    private GradientAlphaKey[] alphaKeys;

    // some of the player stats are stored in the mothership so they don't reset on death
    [SerializeField]
    private int damage;
    [SerializeField]
    private int fireRate;

    public void Start(){
        for (int i = 0; i < enableOnPlay.Count; i++)
            enableOnPlay[i].SetActive(false);
        
        for (int i = 0; i < disableOnPlay.Count; i++)
            disableOnPlay[i].SetActive(true);
    
        // implement data fetching logic here
        damage = 0;
        fireRate = 0;

        // Refresh the GUI after fetching data
        Purchase(0);
        fireRateButton.transform.GetChild(1).GetComponent<TMP_Text>().text = "Level " + fireRate;
        fireRateButton.transform.GetChild(2).GetComponent<TMP_Text>().text = "$" + GlobalFunctions.abbreviate(Math.Pow(2, fireRate));
        damageButton.transform.GetChild(1).GetComponent<TMP_Text>().text = "Level " + damage;
        damageButton.transform.GetChild(2).GetComponent<TMP_Text>().text = "$" + GlobalFunctions.abbreviate(Math.Pow(2, damage));
    }

    public void StartGame() {
        timeSinceGameStart = 0;
        score = 0;

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
        player.damage = math.max(damage, 1); // make the damage >1
        player.fireRate = math.max(fireRate, 1); // make the firerate >1
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
            timeSinceGameStart += Time.deltaTime;
            explosionParticle.transform.position = playerGO.transform.position + new Vector3(0,1,0);
            coinsDisplay.text = "Coins: " + coins;
            scoreDisplay.text = "Score: \n" + GlobalFunctions.abbreviate(score);
        } else {
            timeSinceGameStart = 0;
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
    public GameObject spawnBlock(string name, Vector3 position, Vector3 size, int health){
        GameObject block = new GameObject(name);
        GameObject text = Instantiate(blockTextTemplate);

        // add components
        SpriteRenderer spriteRenderer = block.AddComponent<SpriteRenderer>();
        Block blockComponent = block.AddComponent<Block>();
        BoxCollider2D boxCollider = block.AddComponent<BoxCollider2D>(); // the collider the player can touch
        BoxCollider2D boxCollider2 = block.AddComponent<BoxCollider2D>(); // the collider that kills the player

        boxCollider.size = new Vector2(1,1);
        boxCollider.isTrigger = false;

        boxCollider2.size = new Vector2(0.8f, 0.1f);
        boxCollider2.isTrigger = true;
        boxCollider2.offset = new Vector2(0, -0.5f + boxCollider2.size.y/2 );

        blockComponent.sprite = blockSprite;
        blockComponent.health = health;
        blockComponent.player = playerGO;
        blockComponent.coinSprite = coinSprite;

        spriteRenderer.sprite = blockSprite;
        spriteRenderer.color = gradient.Evaluate(blockComponent.health / 30f); // color based on health

        block.transform.localScale = size;
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
        if (GlobalVariables.isPaused)
            return;
        // for (int i = -(int)Mathf.Floor(screenWidth) - 2; i < screenWidth + 2; i++){
        //     GameObject blockGO = spawnBlock(
        //         "block", new Vector3(i*0.9f, screenHeight+1, 0), (int)(GlobalVariables.currentRow/5) + UnityEngine.Random.Range(1, 5));

        //     blocksGO.Add(blockGO);
        //     blocks.Add(blockGO.GetComponent<Block>());
        // }
        for (int i = 0; i < Constants.blocksPerRow; i++){
            Vector3 size = new Vector3(screenWidth/Constants.blocksPerRow*2f, screenWidth/Constants.blocksPerRow*2f, 1);
            Debug.Log(screenWidth);
            GameObject blockGO = spawnBlock(
                "block", 
                new Vector3(i * size.x - screenWidth + size.x/2, screenHeight+1, 0),
                size,
                (int)(GlobalVariables.currentRow / 2 + UnityEngine.Random.Range(1, 5)));
            blocksGO.Add(blockGO);
            blocks.Add(blockGO.GetComponent<Block>());
        }
        GlobalVariables.currentRow += 1;
    }

    public bool Purchase(double cost){ // returns false if the player doesn't have the money, and subtract the money and return true if the player does
        if (cost > coins)
            return false;
        coins -= cost;
        coinsDisplay.text = "Coins: " + coins;
        return true;
    }

    public void LevelUp(string type){ // called by the Fire Rate/Damage upgrade buttons in the menu
        double cost;
        switch (type){
            case "firerate":
                cost = Math.Pow(2, fireRate);
                if (Purchase(cost)){
                    fireRate += 1;
                    fireRateButton.transform.GetChild(1).GetComponent<TMP_Text>().text = "Level " + fireRate;
                    fireRateButton.transform.GetChild(2).GetComponent<TMP_Text>().text = "$" + GlobalFunctions.abbreviate(Math.Pow(2, fireRate));
                }
                
                break;
            case "damage":
                cost = Math.Pow(2, damage);
                if (Purchase(cost)){
                    damage += 1;
                    damageButton.transform.GetChild(1).GetComponent<TMP_Text>().text = "Level " + damage;
                    damageButton.transform.GetChild(2).GetComponent<TMP_Text>().text = "$" + GlobalFunctions.abbreviate(Math.Pow(2, damage));
                }
                break;
        }
    }
}
