using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonProjectile : MonoBehaviour
{
    // Config Params
    [Header("CONFIG PARAMS")]
    [SerializeField] GameObject fireBallVFX;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float lifeTime = 3;

    // Cached References
    Rigidbody2D myRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        //Destruir el proyectil si lleva 3 seg en el aire
        Destroy(gameObject, lifeTime);
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetUpRotation();
    }
    // Rotacion del proyectil en el aire
    private void SetUpRotation()
    {
        myRigidbody2D.MoveRotation(myRigidbody2D.rotation + rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // fireBall VFX
            GameObject hitFireBallVFX = Instantiate(fireBallVFX, transform.position, Quaternion.identity);
            
            /* duracion del VFX de la explocion de la bola, suficiente
             para que la antorcha reconozca la las particulas del VFX
             El VFX activa las antorchas al estar en contacto con ellas*/
            Destroy(hitFireBallVFX, 0.1f);
            
            /* duracion para que no se destruya la fireball 
            antes que el player reconozca el contacto*/
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
