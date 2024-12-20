using System;  // For general C# functionality
using System.Collections;  // For IEnumerator and Coroutines
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class Health : MonoBehaviour
    {
        public int maxHP = 100;
        public bool IsAlive => currentHP > 0;

        public int currentHP;
        public HealthBar HealthBar;

        public float invincibilityTime = 1f;  // Duration of invincibility after taking damage
        private float lastHitTime = 0f;       // Last time the player was hit
        private SpriteRenderer spriteRenderer; // To change the player's sprite color
        private Rigidbody2D rb;               // Reference to Rigidbody2D to apply force

        void Awake()
        {
            currentHP = maxHP;
            HealthBar.SetMaxHealth(maxHP);
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();  
        }
        
        public void Increment()
        {
            currentHP = Mathf.Clamp(maxHP, 0, maxHP);
            HealthBar.SetHealth(currentHP);
        }

        public void Decrement()
        {
            if (Time.time - lastHitTime < invincibilityTime)
            {
                Debug.Log("Player is invincible. No damage taken.");
                return;
            }

            currentHP = Mathf.Clamp(currentHP - 20, 0, maxHP);
            Debug.Log($"Health after decrement: {currentHP}");
            HealthBar.SetHealth(currentHP);
            lastHitTime = Time.time;

            if (currentHP <= 0)
            {
                Debug.Log("Player health reached zero.");
                var ev = Schedule<HealthIsZero>();
                ev.health = this;
            }
        }



        // Flash the player when invincible
        private IEnumerator FlashInvincibility()
        {

            float flashDuration = invincibilityTime;  
            float flashInterval = 0.1f;  

            Color flashColor = new Color(0f, 1f, 1f, 0.5f); 

            while (Time.time - lastHitTime < flashDuration)
            {
                spriteRenderer.color = (spriteRenderer.color == Color.cyan) ? flashColor : Color.cyan;
                
                yield return new WaitForSeconds(flashInterval);
            }
            
            spriteRenderer.color = Color.cyan;
        }


        public void Die()
        {
            if (currentHP > 0)
            {
                Decrement();
            }
        }

    }
}
