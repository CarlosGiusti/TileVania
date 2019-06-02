using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //Config Params
    private Vector3 targetPosition;
    private Vector3 originalPosition;
    private Vector3 limitPosition;
    [Header("CONFIG PARAMS")]
    [SerializeField] float maxDistance = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        DestroyArrow();
        DestroyArrowByLimit();
    }

    private void DestroyArrowByLimit()
    {
        // Alcance negativo y positivo de la flecha
        Vector3 minLimitPos = new Vector3(originalPosition.x - maxDistance, originalPosition.y - maxDistance);
        Vector3 maxLimitPos = new Vector3(originalPosition.x + maxDistance, originalPosition.y + maxDistance);

        // Si pasa ese alcance, se destruye
        if(transform.position.x < minLimitPos.x || transform.position.y < minLimitPos.y
           || transform.position.x > maxLimitPos.x || transform.position.y > maxLimitPos.y)
        {
            Destroy(gameObject);
        }
        
    }

    // Añadir una posicion objetivo           
    public void AddTargetPosition(Vector3 shootPosition)
    {
        targetPosition = shootPosition;
    }
    
    // Si la posicion objetivo es alcanzada, destruir el gameobject
    private void DestroyArrow()
    {
        if (targetPosition == transform.position)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si hace contacto con un enemigo, tarda 0,05 para destruirse
        // dandole tiempo al enemigo de reconcer el contacto y destruirse
        if (other.tag == "Enemy")
        {
            Destroy(gameObject,0.05f);
        }
        else if(other.tag == "Boss")
        {
            Destroy(gameObject, 0.05f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
