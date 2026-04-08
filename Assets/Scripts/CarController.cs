using UnityEngine;

public class CarControllerFlexible : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 12f;
    public float turnSpeed = 200f;
    public float maxSpeed = 10f;
    public float sandSpeed = 4f;

    [Header("Controls")]
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private float currentMaxSpeed;
    private Rigidbody2D rb;
    private LapTimer lapTimer;

    private int sandLayer;

    public int maxCrashes = 3;
    private int crashCount = 0;
    private Vector3 lastCheckpointPos;

    public GameObject explosionPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMaxSpeed = maxSpeed;
        sandLayer = LayerMask.NameToLayer("Sand");
        lapTimer = GetComponent<LapTimer>();
    }

    void FixedUpdate()
    {
        float moveInput = 0f;
        float turnInput = 0f;

        
        if (Input.GetKey(forwardKey)) moveInput = 1f;
        if (Input.GetKey(backwardKey)) moveInput = -1f;
        if (Input.GetKey(leftKey)) turnInput = -1f;
        if (Input.GetKey(rightKey)) turnInput = 1f;

        float speed = Vector2.Dot(rb.linearVelocity, transform.up);

        
        if (moveInput > 0)
        {
            if (speed < currentMaxSpeed)
                rb.AddForce(transform.up * moveInput * acceleration);
        }

        
        else if (moveInput < 0)
        {
            if (speed > 0)
            {
                rb.AddForce(-transform.up * acceleration * 2f);
            }
            else
            {
                if (Mathf.Abs(speed) < currentMaxSpeed / 2)
                    rb.AddForce(transform.up * moveInput * acceleration);
            }
        }

        
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            rb.MoveRotation(rb.rotation - turnInput * turnSpeed * Time.fixedDeltaTime);
        }

        
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sidewaysVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + sidewaysVelocity * 0.2f;

        // slight drag
        rb.linearVelocity *= 0.99f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == sandLayer)
        {
            currentMaxSpeed = sandSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == sandLayer)
        {
            currentMaxSpeed = maxSpeed;
        }
    }

    public class LapSystem : MonoBehaviour
    {
        public int nextCheckpoint = 0;
        public int lap = 0;

        void OnTriggerEnter2D(Collider2D other)
        {
            Checkpoint cp = other.GetComponent<Checkpoint>();

            if (cp != null)
            {
                if (cp.checkpointIndex == nextCheckpoint)
                {
                    nextCheckpoint++;

                    Debug.Log("Checkpoint " + cp.checkpointIndex);

                    // If last checkpoint → lap complete
                    if (nextCheckpoint >= FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).Length)
                    {
                        nextCheckpoint = 0;
                        lap++;
                        Debug.Log("Lap: " + lap);
                    }
                }
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            crashCount++;
            Debug.Log("Crash! Count: " + crashCount);
            

            if (crashCount >= maxCrashes)
            {
                if (explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                }
                Invoke("respawn", 0.8f);
            }
        }
    }
    void respawn()
    {
        crashCount = 0;
        if (lapTimer != null)
        {
            transform.position = lapTimer.GetLastCheckpointPos();
            transform.rotation = lapTimer.GetLastCheckpointRot();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
    }

}