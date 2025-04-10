using System;
using UnityEngine;

public class enemiesScript : MonoBehaviour
{
    private Rigidbody2D myRigidBodyShip;
    private float velocityEnemy = 5;
    public AudioSource explosionSound;
    public LogicScript logic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logic = GameObject.FindWithTag("Logic").GetComponent<LogicScript>();
        GameObject ship = GameObject.FindWithTag("ship");
        if (ship != null)
        {
            myRigidBodyShip = ship.GetComponent<Rigidbody2D>();
        }
        else
        {
            myRigidBodyShip = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myRigidBodyShip == null)
        {
            return;
        }
        Vector3 shipPosition = myRigidBodyShip.position;

        Vector2 direction = new Vector2(shipPosition.x - transform.position.x, shipPosition.y - transform.position.y);

        transform.up = direction;

        transform.position = Vector2.MoveTowards(transform.position, shipPosition, velocityEnemy * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Bullet")){
            logic.addScore(1);
        }
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("ship"))
        {
            if(explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound.clip, transform.position);
            }
            
            Destroy(gameObject);
        }
    }
}
