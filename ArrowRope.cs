using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRope : MonoBehaviour
{
    // Config Params
    private Vector3 targetPosition;
    LineRenderer arrowLineRenderer;
    private Vector3 originalPosition;
    private Vector3 limitPosition;
    [Header("CONFIG PARAMS")]
    [SerializeField] float maxDistance = 5;

    // Cached References
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        player = FindObjectOfType<Player>();
        arrowLineRenderer = GetComponent<LineRenderer>();
        arrowLineRenderer.enabled = true;
        Destroy(gameObject, 1f);
    }
    // Update is called once per frame
    void Update()
    {
        DestroyArrow();
        DrawingArrowLine();
        DestroyArrowByLimit();
    }

    private void DestroyArrowByLimit()
    {
        // Alcance negativo y positivo de la flecha
        Vector3 minLimitPos = new Vector3(originalPosition.x - maxDistance, originalPosition.y - maxDistance);
        Vector3 maxLimitPos = new Vector3(originalPosition.x + maxDistance, originalPosition.y + maxDistance);

        // Si pasa ese alcance, se destruye
        if (transform.position.x < minLimitPos.x || transform.position.y < minLimitPos.y
           || transform.position.x > maxLimitPos.x || transform.position.y > maxLimitPos.y)
        {
            Destroy(gameObject);
        }
    }

    // Si la linea esta activa, darle posicion de inicio y final constantemente.
    private void DrawingArrowLine()
    {
        if (arrowLineRenderer == true)
        {
            // vector3(0.3,-0.22,0) es la posicion exacta de "Gun" con respecto a el player
            // se multiplica el factor de X por la escala del jugador para move el origen de la arrowline
            // a la direccion correcta.
            arrowLineRenderer.SetPosition(0, player.transform.position + new Vector3(0.3f * player.transform.localScale.x,-0.22f,0));
            arrowLineRenderer.SetPosition(1, transform.position);
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
        if(targetPosition == transform.position)
        {
            Destroy(gameObject);
        }
    }
    // trasladar el player a la posicion objetivo
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hook")
        {
            player.Hook(transform.position);
            Destroy(gameObject);
        }
        if(other.tag == "GamplingHook")
        {
            player.GamplingHook(transform.position);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
