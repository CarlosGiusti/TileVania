using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] PlatformWave platformWave;
    [SerializeField] float moveSpeed;
    private List<Transform> wayPoints;
    private int wayPointIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        // Obtener la lista de la wave config seleccionada
        wayPoints = platformWave.GetPlatformPath();
        transform.position = wayPoints[wayPointIndex].transform.position;
        // Obtener el movespeed de la wave config seleccionada
        moveSpeed = platformWave.GetMoveSpeed();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (wayPointIndex <= wayPoints.Count - 1)
        {
            // Target Position = la posicion del index actual en la lista waypoints
            var targetPosition = wayPoints[wayPointIndex].transform.position;
            var movementPerFrame = moveSpeed * Time.deltaTime;
            // Moverse a la target posicion
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementPerFrame);
            // Si llegaste a esa posicion aumentar el index de la lista 
            if (transform.position == targetPosition)
            {
                wayPointIndex++;
            }
        }
        /*Si ya te moviste al ultimo index de la lista, resetear el index a 0 
         * para mantener un movimiento perpetuo*/
        else
        {
            wayPointIndex = 0;
        }
    }
}
