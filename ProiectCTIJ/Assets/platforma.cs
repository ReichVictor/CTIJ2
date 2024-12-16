using System.Collections;
using UnityEngine;

public class MoveLeftRight : MonoBehaviour
{
    public float moveDistance = 2f; 
    public float interval = 1f;    
    public float speed = 1f;       

    private Vector3 initialPosition;
    private bool movingRight = true;

    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        while (true)
        {
            
            Vector3 targetPosition = initialPosition + (movingRight ? Vector3.right : Vector3.left) * moveDistance;

            
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            
            movingRight = !movingRight;

            
            yield return new WaitForSeconds(interval);
        }
    }
}
