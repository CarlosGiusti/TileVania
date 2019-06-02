using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] float moveSpeedY;
    [SerializeField] float minMovementX;
    [SerializeField] float maxMovementX;
    [SerializeField] float rotationSpeed = 100;
    [SerializeField] GameObject fireBallVFX;
    private float moveSpeedX;


    // Cached References
    Rigidbody2D myRigidBody2D;
    void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        // Velocidad en X, valor random entre los valores minimos y maximos de Y asignados
        moveSpeedX = UnityEngine.Random.Range(minMovementX, maxMovementX);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        SetUpRotation();
    }

    private void SetUpRotation()
    {
        myRigidBody2D.MoveRotation(myRigidBody2D.rotation + rotationSpeed * Time.deltaTime);
    }

    private void Move()
    {
        myRigidBody2D.velocity = new Vector2(moveSpeedX, -moveSpeedY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // fireBall VFX
            GameObject hitFireBallVFX = Instantiate(fireBallVFX, transform.position, Quaternion.identity);
            // duracion del VFX de la explocion de la bola, suficiente
            // para que la antorcha reconozca la las particulas del VFX
            Destroy(hitFireBallVFX, 0.1f);
            // duracion para que no se destruya la fireball 
            // antes que el player reconozca el contacto
            Destroy(gameObject, 0.05f);
        }
        else
        {
            GameObject hitFireBallVFX = Instantiate(fireBallVFX, transform.position, Quaternion.identity);
            Destroy(hitFireBallVFX, 0.1f);
            Destroy(gameObject);
        }       
    }
}
