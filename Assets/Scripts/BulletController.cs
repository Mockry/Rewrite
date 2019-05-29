using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 8f;

    private float lifeTime = 0.7f;
    private int damageToGive = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //moves the bullet in the direction it is facing (which is the same as the gun's direction)
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //destroys the bullet after a delay to stop them slowing the game down.
        //Also lets me limit the range of the gun to adjust the difficulty
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // runs when the bullet collides with another object's collision area
    private void OnCollisionEnter(Collision other)
    {
        //checks if the other object is an enemy and inflicts damage if it is. it also destroys the bullet
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().HurtEnemy(damageToGive);
            Destroy(gameObject);
        }

        // checks if the other object is an obstacle and destroys the bullet
        // if it is
        if (other.gameObject.tag == "Blocker")
        {
            Destroy(gameObject);
        }
    }

}
