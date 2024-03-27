using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Block : MonoBehaviour {
    public int health;
    public Sprite sprite;
    public Sprite coinSprite;
    public GameObject player;
    public Color color;
    private bool isExploding; // prevents the block from playing multiple death animations

    public Block(Sprite sprite, GameObject player, Sprite coinSprite, Color color){
        this.sprite = sprite;
        this.player = player;
        this.coinSprite = coinSprite;
        this.color = color;

        health = 5;
    }

    public Block(int health, Sprite sprite, GameObject player, Sprite coinSprite, Color color){
        this.health = health;
        this.sprite = sprite;
        this.player = player;
        this.coinSprite = coinSprite;
        this.color = color;
    }

    public void Explode(){ // automatically called when block dies
        if (isExploding)
            return;
        
        isExploding = true;

        Audio audio = FindAnyObjectByType<Audio>();
        audio.Play(audio.pop);

        Coin coin = new Coin(1, coinSprite); // spawn coin on death
        coin.Spawn(transform.position);

        // play a death animation (Explosion)
        Mothership mothership = FindAnyObjectByType<Mothership>();
        GameObject particleClone = Instantiate(mothership.explosionParticle2, transform.parent);

        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[]{
            new GradientColorKey(color, 0),
            new GradientColorKey(color - new Color(0.1f, 0.1f, 0.1f), 1)
        }, new GradientAlphaKey[]{
            new GradientAlphaKey(1, 0),
            new GradientAlphaKey(1, 1)
        });

        particleClone.transform.position = transform.position;
        particleClone.SetActive(true);
        particleClone.GetComponent<ParticleSystem>().Play();
        
        var col = particleClone.GetComponent<ParticleSystem>().colorOverLifetime;
        col.enabled = true;
        col.color = grad;

        LeanTween.value(0, 1, 1).setOnComplete(() => {
            if (!GlobalVariables.isPaused){
                particleClone.gameObject.transform.position -= new Vector3(0, Time.deltaTime * Constants.gravity, 0); // slowly descend
            }
            Destroy(particleClone);
            Destroy(gameObject);
        });

        gameObject.SetActive(false);

    }

    // functions for spawning in blocks is in Mothership.cs
    // Block inherits from MonoBehavior so you can't directly create a new Block object

    private void OnTriggerEnter2D(Collider2D other) { // when a bullet hits the ship
        if (LayerMask.LayerToName(other.gameObject.layer) == "Bullet"){ // make sure the collider is a bullet
            int damage = other.gameObject.GetComponent<Bullet>().damage;
            health -= damage;
            Destroy(other.gameObject); // destroy bullet
        } else if (LayerMask.LayerToName(other.gameObject.layer) == "Player"){ // harm player on touch
            Player player = other.gameObject.GetComponent<Player>();
            player.health -= player.maxHealth/2;
            Explode();
        }
        
    }

    private void Update() {
        if (!GlobalVariables.isAlive){
            Destroy(gameObject);
            return;
        }
        if (!GlobalVariables.isPaused)
            transform.position -= new Vector3(0, Time.deltaTime * Constants.gravity, 0); // slowly descend

        if (health <= 0){ // explode if health is 0 or less
            player.GetComponent<Player>().mothership.score += GlobalVariables.currentRow + 1;
            Explode();
        }

        if (transform.position.y < -Camera.main.orthographicSize){ // destroy the block when it's passed the player's screen
            player.GetComponent<Player>().mothership.score += 1;
            Destroy(gameObject);
        }

        transform.GetChild(0).GetComponent<TMP_Text>().text = health + ""; // update the health display
    }
}