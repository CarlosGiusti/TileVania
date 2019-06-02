using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickeableArmor : MonoBehaviour
{
    [Header("CONFIG PARAMS")]
    [SerializeField] int value;
    [Header("AUDIO")]
    [SerializeField] AudioClip armorClip;
    [SerializeField] float armorClipVolumen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            FindObjectOfType<Player>().AddArmor(value);
            FindObjectOfType<GameSesion>().AddArmor(value);
            AudioSource.PlayClipAtPoint(armorClip, FindObjectOfType<Camera>().transform.position, armorClipVolumen);
            Destroy(gameObject);
        }        
    }
}
