using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    // Config Params
    [Header("CONFIG PARAMS")]
    [SerializeField] bool movingCanon;
    [SerializeField] CanonProjectile projectile;
    [SerializeField] GameObject gun;

    [Header("STATIC CANON")]
    [SerializeField] Vector2 CanonProjectileSpeed;    
    [SerializeField] float minShootCounterStatic;
    [SerializeField] float maxShootCounterStatic;
    [SerializeField] float startShootCounterStatic;
    float shootCounterStatic;

    [Header("MOVING CANON")]
    [SerializeField] float shootCounter;
    [SerializeField] float MovCanonProjectileSpeed;
    private float starShootCounter;

    //States 
    private bool shooting = false;


    // Cached References
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        // Guardar referencia del tiempo de disparo
        shootCounterStatic = startShootCounterStatic;

        // Si el cañon es estatico, desactivar el Circlecollider2D
        if (!movingCanon)
        {            
            GetComponent<CircleCollider2D>().enabled = false;
        }
        else
        {
            starShootCounter = shootCounter;
            GetComponent<CircleCollider2D>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Si el cañon es movil, apuntar al player y empezar conteo de disparo
        if (movingCanon)
        {
            AimPlayer();
            ShootingCounterDown();
        }
        // si es estatico, empezar conteo de disparo
        else
        {
            StaticCanonShootingCounterDown();
        }

    }
    
    // apuntar constantemente a la direccion del player
    private void AimPlayer()
    {
        transform.right = player.transform.position - gun.transform.position;        
    }
    
    // Conteo de disparo de cañon estatico
    private void StaticCanonShootingCounterDown()
    {
        shootCounterStatic -= Time.deltaTime;

        if (shootCounterStatic <= Mathf.Epsilon)
        {
            Shoot();
            // Reestablecer el shootCounterStatic a su valor inicial
            shootCounterStatic = UnityEngine.Random.Range(minShootCounterStatic, maxShootCounterStatic);
        }
    }

    // Conteo de disparo de cañon movil
    private void ShootingCounterDown()
    {
        if (shooting)
        {            
            shootCounter -= Time.deltaTime;
        }
        if (shootCounter <= Mathf.Epsilon)
        {
            Shoot();
            shootCounter = starShootCounter;
       }
             
    }

    private void Shoot()
    {
        // Si es movil
        if (movingCanon)
        {
            CanonProjectile projectileShooted = Instantiate(projectile, gun.transform.position, Quaternion.identity);
            var targetPos = player.transform.position - gun.transform.position;
            projectileShooted.GetComponent<Rigidbody2D>().velocity = targetPos * MovCanonProjectileSpeed;
        }
        // Si es estatico
        else
        {
            CanonProjectile projectileShooted = Instantiate(projectile, gun.transform.position, Quaternion.identity);
            projectileShooted.GetComponent<Rigidbody2D>().velocity = CanonProjectileSpeed;
        }
    }
   
    // si el player entra en la zona del canon, baja el shootercounter a 1
    // y cambia el estado de shooting
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (movingCanon)
        {
            if (other.tag == "Player")
            {
                // Shooting activa el conteo de disparo de cañon movil
                shooting = true;
                // 0.05f conteo para el primer disparo solamente
                shootCounter = 0.5f;
            }
        }       
    }
   
    // Si el player sale de la zona del cañon, sube el shootercounter a 3
    // y camia ele stado de shooting
    private void OnTriggerExit2D(Collider2D other)
    {
        if (movingCanon)
        {
            if (other.tag == "Player")
            {
                // para cancelar el conteo de disparo
                shooting = false;                
            }
        }
        
    }
}
