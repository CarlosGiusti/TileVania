using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickeableArrow : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] private int arrowValue = 1;
    [Header("AUDIO")]
    [SerializeField] private AudioClip arrowPickUpSound;
    [SerializeField] float ArrowPickUpSoundVolumen;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<GameSesion>().AddArrows(arrowValue);
            AudioSource.PlayClipAtPoint(arrowPickUpSound, FindObjectOfType<Camera>().transform.position, ArrowPickUpSoundVolumen);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<GameSesion>().AddArrows(arrowValue);
            AudioSource.PlayClipAtPoint(arrowPickUpSound, FindObjectOfType<Camera>().transform.position, ArrowPickUpSoundVolumen);
            Destroy(gameObject);
        }
    }
}
