using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Door door;  // Referință către Door-ul asociat
    public float activationTime = 5f; // Timpul cât ușa rămâne activă

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifică dacă Player a atins presiunea
        {
            Debug.Log("Player a atins PressurePlate");
            door.ActivateDoor(activationTime); // Activează ușa
        }
    }
}
