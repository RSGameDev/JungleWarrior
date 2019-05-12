using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class performs the instantiating of the bullet prefabs.
public class Projectile : MonoBehaviour {


    public GameObject bulletPrefab;     // Reference to the bulletPrefab GameObject.
    public Transform bulletSpawn;       // Reference to the bulletSpawn Transform.
    public int speed;                   // The speed of which the projectile travels.
        

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    //Used for when the Player shoots a bullet.
    public void PlayerFire() {

        //Setting a reference
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation); 
        bullet.gameObject.tag = "Player bullet";                                       // Assign the bullet a tag that it is a Player bullet.
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * speed;  // Allow the bullet to travel in a forward trajectory.
        Destroy(bullet, 5.0f);                                                         // Destroy the bullet after five seconds.
    }

    //Used for when the Enemy shoots a bullet.
    public void EnemyFire() {

        //Setting a reference
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.gameObject.tag = "Enemy bullet";                                        // Assign the bullet a tag that it is an Enemy bullet.
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * speed;  // Allow the bullet to travel in a forward trajectory.
        Destroy(bullet, 5.0f);                                                         // Destroy the bullet after five seconds.
    }
}

