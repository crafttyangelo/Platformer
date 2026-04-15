using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Direction the user wants to move in
    private Vector2 direction = Vector2.zero;

    private Rigidbody2D rb;

    private Vector2 velocity;

    private RaycastHit2D left_ground_check;

    private RaycastHit2D right_ground_check;

    private bool jump_check;

    public float jump_force = 15;

    public float max_speed = 6.7f;

    public float acceleration = 1.5f;

    public float friction = 0.75f;

    public float gravity = 0.5f;

    public float max_fall_speed = 9.8f;   

    public LayerMask mask;
 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal Movement
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;
        }
        else
        {
            direction.x = 0;
        }

        // Pressing jump key
        if (Input.GetKey(KeyCode.Space))
        {
            jump_check = true;
        }
        else
        {
            jump_check = false;
        }

        // Fix player's movement during diagonal movement
        direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        float ground_offset = 0f;

        // Accelerate player
        velocity.x += acceleration * direction.x;

        // Cap speed to max_speed
        velocity.x = Mathf.Clamp(velocity.x, -max_speed, max_speed);

        // Move player
        rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime) - new Vector2(0, ground_offset));

        // Friction
        if(direction.x == 0)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, friction);
        }

        // Perform raycast
        left_ground_check = Physics2D.Raycast(rb.position + new Vector2(-0.5f, 0), Vector2.down, 1, mask);
        right_ground_check = Physics2D.Raycast(rb.position + new Vector2(0.5f, 0), Vector2.down, 1, mask);

        // If player is touching ground
        if (left_ground_check || right_ground_check)
        {
            velocity.y = 0;

            // Fixes player position to set to ground
            ground_offset = Mathf.Max(left_ground_check.distance, right_ground_check.distance) - 0.5f;

            if(jump_check)
            {
                velocity.y = jump_force;
            }
        }
        else // Player is in the air
        {
            velocity.y -= gravity;
        }

        // Apply gravity to velocity
        velocity.y -= gravity;

        // Cap fall speed
        velocity.y = Mathf.Max(velocity.y, -max_fall_speed);
    }
        private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetComponent<Rigidbody2D>().position + new Vector2(-0.5f, 0), Vector2.down);
        Gizmos.DrawRay(GetComponent<Rigidbody2D>().position + new Vector2(0.5f, 0), Vector2.down);        
    }
    
}