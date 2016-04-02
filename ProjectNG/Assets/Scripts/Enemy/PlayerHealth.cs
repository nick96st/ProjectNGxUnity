using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{    
    public float health;
    public float damage;
    Zombie target;

    void Awake() { target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Zombie>(); }

    public void GetHit(float damageValue)
    {
        damageValue = (this.health - damageValue < 0) ? this.health : damageValue;
        this.health -= damageValue;

        //Debug.Log("Player health: " + this.health);
    }

	// Update is called once per frame
	void Update ()
    {
        if (this.health < 0) this.health = 0;

        // Test attacking the enemy...
        if(Input.GetKeyDown(KeyCode.F))
        {
            target.GetHit(this.damage);
        }
	}
}
