using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFliying : MonoBehaviour
{
    // Config Params
    [Header("CONFIG PARAMS")]
    [SerializeField] EnemyWaveConfig enemyWaveConfig;
    [SerializeField] GameObject fireBall;
    [SerializeField] float maxTimeBetweenShots;
    [SerializeField] float minTimeBetweenShots;
    [SerializeField] float timeToDie = 0.15f;
    [SerializeField] GameObject dropeableObject;
    [Header("AUDIO")]
    [SerializeField] AudioClip deadSound;
    [SerializeField] float deadClipVolumen;

    private List<Transform> wayPoints;
    private int wayPointIndex = 0;
    private float moveSpeed;            
    float shootCounter;
    
    //Cached References
    BoxCollider2D myBoddyCollider2D;

    void Start()
    {
        // Asignar conteo de disparo en los valores dados como minimos y maximos
        shootCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        // Obtener movespeed de la wavecongif seleccionada
        moveSpeed = enemyWaveConfig.GetMoveSpeed();
        // Obtener la lista de wayoints de la waveconfig seleccionada
        wayPoints = enemyWaveConfig.GetWayPoints();

        myBoddyCollider2D = GetComponent<BoxCollider2D>();
        // Moverse a la primera posicion de la lista waypoints
        transform.position = wayPoints[wayPointIndex].transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        ShooterCounterDown();
        Move();
        Dead();
    }
    // Conteo de disparo
    private void ShooterCounterDown()
    {
        shootCounter -= Time.deltaTime;
        if(shootCounter <= Mathf.Epsilon)
        {
            Shoot();
            shootCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Shoot()
    {
        Instantiate(fireBall, transform.position, transform.rotation);
    }

    private void Move()
    {
        if(wayPointIndex <= wayPoints.Count - 1)
        {
            // Target posicion = posicion del indice actual de la lista waypoints
            var targetPosition = wayPoints[wayPointIndex].transform.position;
            float movemetPerFrame = moveSpeed * Time.deltaTime;
            
            // Flip Sprite dependiendo de la ubicacion del target con respecto a la mia
            if (targetPosition.x > transform.position.x)
            {
                transform.localScale = new Vector2(-1f, 1f);
            }
            else if(targetPosition.x < transform.position.x)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
            // Moverse hacia la posicion target
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movemetPerFrame);
            // si llegue a la posicion objetivo, aumentar el indice de la lista waypoints
            if (transform.position == targetPosition)
            {
                wayPointIndex++;
            }           
        }
        // Si waypont index excede el numero de posiciones en la lista, restaurar a 0 para lograr un loop
        else
        {
            wayPointIndex = 0;
        }
    }

    private void Dead()
    {
        if (myBoddyCollider2D.IsTouchingLayers(LayerMask.GetMask("Arrow")))
        {
            GetComponent<Animator>().SetTrigger("Dead");
            StartCoroutine(EnemyDead());
        }
    }
    private IEnumerator EnemyDead()
    {
        AudioSource.PlayClipAtPoint(deadSound, FindObjectOfType<Camera>().transform.position, deadClipVolumen);
        moveSpeed = 0;
        // Si hay un objeto dropeable, instanciarlo 
        if (dropeableObject != null)
        {
            GameObject dropedHook = Instantiate(dropeableObject, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(timeToDie);
        Destroy(gameObject);
    }
}
