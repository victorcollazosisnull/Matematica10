using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlechaController : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("Diana"))
        {
            Destroy(collision.gameObject); 
            Destroy(gameObject); 
            if (gameManager != null)
            {
                gameManager.DestroyDiana();
            }
        }
    }
}