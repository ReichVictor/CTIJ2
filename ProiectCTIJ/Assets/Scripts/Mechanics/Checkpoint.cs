using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Respawn respawn;
    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        // Get the Respawn component from the Respawn GameObject
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Respawn>();

        // Get the SpriteRenderer component of this checkpoint

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers == null)
        {
            Debug.LogWarning("No SpriteRenderer found on the Checkpoint object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Set this checkpoint as the respawn point
            respawn.SpawnPoint = this.gameObject;

            // Change this checkpoint's sprite color to green

            if (spriteRenderers != null)
            {
                foreach (var renderer in spriteRenderers)
                {
                    renderer.color = Color.green;
                }
            }

        }
    }
}
