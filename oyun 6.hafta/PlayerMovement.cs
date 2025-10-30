using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 50f;
    Rigidbody rb;

    ScoreManager scoreManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        rb.AddForce(movement*moveSpeed);

        
    }
    private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Pickup"))
            {
                Destroy(other.gameObject);
                scoreManager.CollectPickup();
            }
        }
}
