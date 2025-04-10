using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f; // Bullet speed

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f; // Life time of bullet after shooting

    private Rigidbody2D myRigidBodyBullet;
    public AudioSource bulletSound;

    private void Start()
    {
        myRigidBodyBullet = GetComponent<Rigidbody2D>();
        bulletSound.Play();
        Destroy(gameObject, lifeTime); // Destroy after x seconds of it spawning
    }

    private void FixedUpdate()
    {
        myRigidBodyBullet.linearVelocity = transform.up * speed; // Makes bullet always fly straight forward
    }
}
