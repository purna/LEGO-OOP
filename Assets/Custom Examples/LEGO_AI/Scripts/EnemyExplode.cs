using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.LEGO.Minifig;

public class EnemyExplode : MonoBehaviour
{
    public AudioClip explodeAudioClip;
    protected AudioSource audioSource;

    [SerializeField, HideInInspector]
    protected Transform leftArmTip = null;
    [SerializeField, HideInInspector]
    protected Transform rightArmTip = null;
    [SerializeField, HideInInspector]
    protected Transform leftLegTip = null;
    [SerializeField, HideInInspector]
    protected Transform rightLegTip = null;
    [SerializeField, HideInInspector]
    protected Transform head = null;

    protected Minifig minifig;

    protected Animator animator;

    Vector3 externalMotion;

    float externalRotation;
     protected float rotateSpeed;

      protected Vector3 moveDelta = Vector3.zero;

    protected bool exploded;

    protected virtual void Awake()
    {
        minifig = GetComponent<Minifig>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

 protected virtual void Update()
        {
            if (exploded)
            {
                return;
            }
        }


   protected void HandleExplode()
        {
            const float horizontalVelocityTransferRatio = 0.35f;
            const float verticalVelocityTransferRatio = 0.1f;
            const float angularVelocityTransferRatio = 1.0f;

            var transferredSpeed = Vector3.Scale(moveDelta + externalMotion, new Vector3(horizontalVelocityTransferRatio, verticalVelocityTransferRatio, horizontalVelocityTransferRatio));
            var transferredAngularSpeed = (rotateSpeed + externalRotation) * angularVelocityTransferRatio;

            if (explodeAudioClip)
            {
               audioSource.PlayOneShot(explodeAudioClip);
            }

            MinifigExploder.Explode(minifig, leftArmTip, rightArmTip, leftLegTip, rightLegTip, head, transferredSpeed, transferredAngularSpeed);
        }

        public virtual void Explode()
        {
            if (!exploded)
            {
                exploded = true;
                animator.enabled = false;

                HandleExplode();
            }
        }

         protected virtual void OnValidate()
        {

            if (!leftArmTip || !rightArmTip || !leftLegTip || !rightLegTip || !head)
            {
                var rigTransform = transform.GetChild(1);
                if (rigTransform)
                {
                    FindJointReferences(rigTransform);
                }
                else
                {
                    Debug.LogError("Failed to find Minifigure rig.");
                }
            }
        }


        protected void FindJointReferences(Transform parent)
        {
            foreach (Transform child in parent)
            {
                switch (child.name)
                {
                    case "wrist_L":
                        leftArmTip = child;
                        break;
                    case "wrist_R":
                        rightArmTip = child;
                        break;
                    case "foot_L":
                        leftLegTip = child;
                        break;
                    case "foot_R":
                        rightLegTip = child;
                        break;
                    case "head":
                        head = child;
                        break;
                }

                FindJointReferences(child);
            }
        }
}
