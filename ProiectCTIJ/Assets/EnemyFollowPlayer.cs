using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// A controller for enemies. The enemy chases the player only on the X-axis when within a detection range.
    /// When the player is out of range, the enemy patrols left and right.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class EnemyFollower : MonoBehaviour
    {
        public Transform player; // Referința către Player
        public float detectionRange = 5f; // Raza de detecție pentru Player
        public float chaseSpeed = 3f; // Viteza de urmărire a inamicului
        public float pushDistance = 1f; // Distanța cu care Player-ul este împins
        public float pushDuration = 0.2f; // Durata împingerii

        // Parametrii pentru patrulare
        public float patrolDistance = 3f; // Distanța patrulării stânga-dreapta
        public float patrolSpeed = 2f; // Viteza patrulării

        private Vector2 patrolStartPosition; // Poziția de start a patrulării
        private bool movingRight = true; // Direcția curentă a patrulării

        internal Collider2D _collider;
        private Vector2 movement;

        void Awake()
        {
            _collider = GetComponent<Collider2D>();

            // Găsește Player-ul automat dacă nu este setat
            if (player == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
                else
                {
                    Debug.LogError("Player nu a fost găsit! Asigură-te că obiectul Player are tag-ul 'Player'.");
                }
            }

            // Salvează poziția de start pentru patrulare
            patrolStartPosition = transform.position;
        }

        void Update()
        {
            if (player != null && Mathf.Abs(player.position.x - transform.position.x) <= detectionRange)
            {
                // Urmărirea Player-ului doar pe axa X
                float directionX = Mathf.Sign(player.position.x - transform.position.x);
                movement = new Vector2(directionX, 0); // Mișcare doar pe axa X
            }
            else
            {
                // Mișcare de patrulare
                Patrol();
            }
        }

        void FixedUpdate()
        {
            // Mișcă inamicul în direcția calculată
            if (movement != Vector2.zero)
            {
                Vector2 targetPosition = new Vector2(
                    transform.position.x + movement.x * chaseSpeed * Time.fixedDeltaTime,
                    transform.position.y // Menține poziția pe axa Y
                );

                transform.position = targetPosition;
            }
        }

        void Patrol()
        {
            // Calculează limitele patrulării
            float leftLimit = patrolStartPosition.x - patrolDistance;
            float rightLimit = patrolStartPosition.x + patrolDistance;

            // Verifică direcția de mișcare și ajustează
            if (movingRight)
            {
                movement = new Vector2(1, 0); // Mișcare spre dreapta
                transform.position += new Vector3(patrolSpeed * Time.deltaTime, 0, 0);

                if (transform.position.x >= rightLimit)
                {
                    movingRight = false; // Schimbă direcția spre stânga
                }
            }
            else
            {
                movement = new Vector2(-1, 0); // Mișcare spre stânga
                transform.position -= new Vector3(patrolSpeed * Time.deltaTime, 0, 0);

                if (transform.position.x <= leftLimit)
                {
                    movingRight = true; // Schimbă direcția spre dreapta
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Detectează coliziunea cu Player-ul
            if (collision.gameObject.CompareTag("Player"))
            {
                // Calculează direcția de împingere pe axa X
                Vector2 pushDirection = new Vector2(
                    Mathf.Sign(collision.transform.position.x - transform.position.x), // Direcția pe axa X
                    0 // Nicio mișcare pe axa Y
                );

                // Aplică efectul de împingere manual
                StartCoroutine(PushPlayer(collision.transform, pushDirection));
                Debug.Log("Player împins de inamic!");
            }
        }

        private System.Collections.IEnumerator PushPlayer(Transform playerTransform, Vector2 pushDirection)
        {
            Vector3 startPosition = playerTransform.position;
            Vector3 targetPosition = startPosition + (Vector3)(pushDirection * pushDistance);

            float elapsed = 0f;

            while (elapsed < pushDuration)
            {
                playerTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / pushDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            playerTransform.position = targetPosition;
        }

        void OnDrawGizmosSelected()
        {
            // Desenează raza de detecție pentru debug
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), detectionRange);

            // Desenează limitele patrulării pentru debug
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(
                new Vector2(patrolStartPosition.x - patrolDistance, transform.position.y),
                new Vector2(patrolStartPosition.x + patrolDistance, transform.position.y)
            );
        }
    }
}
