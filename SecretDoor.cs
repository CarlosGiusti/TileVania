using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretDoor : MonoBehaviour
{
    // Config Params
    [Header("CONFIG PARAMS")]
    [SerializeField] float colorConstant;
    [Header("AUDIO PARAMS")]
    [SerializeField] AudioClip audioClip;
    [SerializeField] float audioClipVolumen;
    [SerializeField] GameObject reward;


    // States
    private bool changeTransparenci = false;
    private bool soundPlayed = false;

    //Cached References
    SpriteRenderer mySpriteRenderer;

    
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (changeTransparenci)
        {
            // Detener la sutraccion del componente a cuando es menor que cero
            if (mySpriteRenderer.color.a <= 0) { return; }
            TurnOf();
        }
        else
        {
            // Detener la adicion del componente a cuando es mayor que un
            if (mySpriteRenderer.color.a >= 1) { return; }
            TurnOn();
        }

        // Reproducir el audioclip una vez luego de abrir la puerta
        if(mySpriteRenderer.color.a < 0.5 && !soundPlayed)
        {
            AudioSource.PlayClipAtPoint(audioClip, FindObjectOfType<Camera>().transform.position, audioClipVolumen);
            soundPlayed = true;
            InstantiateReward();
        }
    }
    // Dropear objeto al abrirse
    private void InstantiateReward()
    {
        GameObject instantiatedReward = Instantiate(reward, transform.position, transform.rotation);
        instantiatedReward.transform.parent = transform.parent;
        Destroy(gameObject);
    }
    // Activar color
    private void TurnOn()
    {
        mySpriteRenderer.color += new Color(0, 0, 0, colorConstant);
    }
    // Desactivar color
    private void TurnOf()
    {
        mySpriteRenderer.color += new Color(0, 0, 0, -colorConstant);
    }
    // Activar puerta
    public void OpenSecretDoor()
    {
        changeTransparenci = true;
    }
    // Desactivar puerta
    public void CloseSecretDoor()
    {
        changeTransparenci = false;
    }


}
