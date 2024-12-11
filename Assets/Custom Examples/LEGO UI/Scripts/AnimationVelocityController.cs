using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationVelocityController : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;

    // Animator parameter names
    private static readonly int VelocityParam = Animator.StringToHash("Velocity");

    void Awake()
    {
        // Get the required components
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on the GameObject.");
        }

        // Initialize the last position to the current position
        lastPosition = transform.position;
    }

    void Update()
    {
        if (animator == null) return;

        // Calculate the velocity based on the change in position
        Vector3 currentPosition = transform.position;
        Vector3 displacement = currentPosition - lastPosition;
        float speed = displacement.magnitude / Time.deltaTime;

        // Update the last position
        lastPosition = currentPosition;

        // Clamp the speed between 0 and 1
        speed = Mathf.Clamp(speed, 0f, 1f);

        // Map the speed to idle (0.2 and below) and walking (0.3 and above)
        if (speed <= 0.2f)
        {
            speed = 0f; // Idle
        }
        else if (speed >= 0.3f)
        {
            speed = 1f; // Walking
        }

        // Set the velocity parameter in the animator
        animator.SetFloat(VelocityParam, speed);
    }
}