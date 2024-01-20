using UnityEngine;

public class Block : MonoBehaviour {
    public int health;

    public Block(){
        health = 5;
    }

    public Block(int health){
        this.health = health;
    }

    public void Explode(){ // automatically called when block dies
        Destroy(gameObject);
    }

    public GameObject Spawn(string name0){
        return Spawn(name, Vector3.zero, Quaternion.identity);
    }

    public GameObject Spawn(string name, Vector3 position, Quaternion rotation){ // loads the block into the scene
        GameObject block = new GameObject(); // empty gameobject
        
        return block;
    }

    private void Update() {
        if (health <= 0){ // explode if health is 0 or less
            Explode();
        }
    }
}