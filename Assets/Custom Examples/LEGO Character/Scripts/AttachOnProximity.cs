using UnityEngine;

public class AttachOnProximity : MonoBehaviour
{
    public GameObject objectToAttach; // The object to attach
    public string handTag = "RightHand"; // Tag of the hand object
    public float attachDistance = 1.0f; // Maximum distance for attachment
    public bool attachDuringAnimation = true; // Set true to check if animation is playing

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Find the child object with the specified tag
        Transform hand = FindChildWithTag(transform, handTag);

        if (hand != null)
        {
            // Check distance between the object and the hand
            float distance = Vector3.Distance(hand.position, objectToAttach.transform.position);

            // Check if animation is playing (if required)
            bool isPlaying = animator != null && animator.GetCurrentAnimatorStateInfo(0).length > 0;

            // Attach the object if conditions are met
            if (distance <= attachDistance && (!attachDuringAnimation || isPlaying))
            {
                AttachToHand(hand.gameObject);
            }
        }
    }

    private Transform FindChildWithTag(Transform parent, string tag)
    {
        // Iterate through all child objects to find the one with the specified tag
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
                return child;
        }
        return null;
    }

    private void AttachToHand(GameObject hand)
    {
        // Attach the object to the hand
        objectToAttach.transform.SetParent(hand.transform);

        // Optional: Set position and rotation to match hand
        objectToAttach.transform.localPosition = Vector3.zero;
        objectToAttach.transform.localRotation = Quaternion.identity;

        Debug.Log($"{objectToAttach.name} attached to {hand.name}");
    }
}
