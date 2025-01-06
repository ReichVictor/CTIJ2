using UnityEngine;

public class Door : MonoBehaviour
{
    public float moveDistance = 3f; // Distanța pe care se deplasează ușa
    public float moveSpeed = 2f;    // Viteza de mișcare a ușii

    private Vector3 initialPosition; // Poziția inițială a ușii
    private bool isActivated = false;
    private float deactivateTime;
    private Rigidbody2D rb;

    private void Start()
    {
        initialPosition = transform.position; // Salvează poziția inițială
        rb = GetComponent<Rigidbody2D>(); // Obține componenta Rigidbody2D
    }

    public void ActivateDoor(float duration)
    {
        isActivated = true;
        deactivateTime = Time.time + duration;
    }

    private void FixedUpdate()
    {
        if (isActivated)
        {
            if (Time.time <= deactivateTime)
            {
                // Mișcă ușa în sus
                Vector3 targetPosition = initialPosition + Vector3.up * moveDistance;
                rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime));
            }
            else
            {
                // Revino la poziția inițială
                rb.MovePosition(Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime));
                if (transform.position == initialPosition)
                {
                    isActivated = false; // Oprește mișcarea când revine la poziția inițială
                }
            }
        }
    }
}
