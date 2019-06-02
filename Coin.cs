using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //Config Params
    [SerializeField] int coinValue = 1;
    [SerializeField] AudioClip coinPickUpSound;
    [SerializeField] float coinPickUpSoundVolumen;


    void OnTriggerEnter2D(Collider2D other)
    {
        // Agregar moneda al contador del gameseion, reproducir sonido y destruir moneda
        FindObjectOfType<GameSesion>().AddToScore(coinValue);
        AudioSource.PlayClipAtPoint(coinPickUpSound, FindObjectOfType<Camera>().transform.position,coinPickUpSoundVolumen);
        Destroy(gameObject);
    }
}
