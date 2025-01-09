using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        public float maxSpeed = 7;             // Viteza maximă de deplasare
        public float jumpTakeOffSpeed = 7;     // Viteza maximă de salt
        public float detectionRange = 10f;     // Distanta la care inamicii detectează jucătorul
        public Transform player;               // Referința la jucător (Player)

        public Vector2 move;                   // Direcția de mișcare
        public bool jump;                      // Dacă trebuie să sară
        public bool stopJump;                  // Dacă trebuie să oprească saltul

        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        private bool isPlayerInRange = false;

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            // Verificăm distanța față de Player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRange)
            {
                isPlayerInRange = true;
                // Dacă Player-ul este în raza de detectare, se mișcă spre acesta
                if (player.position.x > transform.position.x)
                {
                    move.x = 1;  // Mărește viteza pe axa X spre dreapta
                    spriteRenderer.flipX = false;  // Se asigură că personajul se îndreaptă corect
                }
                else if (player.position.x < transform.position.x)
                {
                    move.x = -1;  // Mărește viteza pe axa X spre stânga
                    spriteRenderer.flipX = true;   // Se asigură că personajul se îndreaptă corect
                }
            }
            else
            {
                isPlayerInRange = false;
                // Mișcare alternativă stânga-dreapta (îți păstrezi mișcarea alternativă)
                move.x = Mathf.Sin(Time.time) > 0 ? 1 : -1;
            }

            // Aplică mișcarea și saltul
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            // Actualizează animările și mișcarea
            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
            targetVelocity = move * maxSpeed;
        }
    }
}
