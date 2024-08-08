using UnityEngine;

public class SphereController : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the sphere

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from WASD keys
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create a movement vector
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        // Set the velocity directly to the Rigidbody
        rb.velocity = movement * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cube"))
        {
            // Implement physical response here
            Debug.Log("Sphere hit a cube!");

            Rigidbody cubeRb = collision.gameObject.GetComponent<Rigidbody>();
            if (cubeRb != null)
            {
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection = -forceDirection.normalized;
                cubeRb.AddForce(forceDirection * speed, ForceMode.Impulse);
            }
        }
    }
}
