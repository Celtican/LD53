using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 2f;

    [Tooltip("How much damage this enemy can take before it dies.")]
    public float health = 1;

    // [Tooltip("How much money the player is awarded with when this enemy dies.")]
    // public float moneyReward = 10f;

    [Tooltip("The sound effect that plays when this enemy is damaged (but does not die).")]
    public AudioClip soundOnDamage;

    [Tooltip("The sound effect that plays when this enemy dies.")]
    public AudioClip soundOnDeath;

    // This is a reference to the audio source attached to this game object.
    private AudioSource audioSource;

    public UnityEvent onDie = new UnityEvent();

    // Start is called before the first frame update
    private void Start()
    {
        // Get the attached audio source if we have one. GetComponent() is an expensive function, so we prefer to call it only once.
        audioSource = GetComponent<AudioSource>();
        
        // If we don't have an audio source but we do have either a sound on damage or a sound on death...
        if (audioSource == null && (soundOnDamage != null || soundOnDeath != null))
        {
            // ... then warn the dev.
            Debug.LogWarning(name + " has an assigned sound effect but it does not have an attached Audio Source!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // For context: because the enemy's target is their parent, and because a game object's localPosition is the
        // distance from its parent, the localPosition is therefor the distance (and direction) to the enemy's target.

        // Get how much we'll move this frame.
        bool isPosOutOfBounds = CameraController.instance.IsPosOutOfBounds(transform.position, 1);
        float distanceToMove = (isPosOutOfBounds
            ? 1
            : movementSpeed * Time.deltaTime);
        
        // If we are close to the target...
        if (distanceToMove >= transform.localPosition.magnitude)
        {
            // ... then get the next target.
            Transform nextTarget = GetNextTarget();

            // If we have a next target...
            if (nextTarget != null)
            {
                // ... Then set our position to our current target and give us the next target.
                Vector3 newPosition = transform.parent.position;
                transform.SetParent(nextTarget);
                transform.position = newPosition;
                
                // And then rotate towards the new target.
                float angle = Vector3.SignedAngle(transform.up, transform.position - transform.parent.position,
                    transform.forward) - 90;
                // if (isPosOutOfBounds)
                // {
                    transform.Rotate(0, 0, angle);
                // }
                // else
                // {
                //     Tween tween = transform.DORotate(new Vector3(0, 0, angle + transform.rotation.z), 1/(movementSpeed*2));
                //     onDie.AddListener(() => tween.Kill());
                // }
            }
            
            // And then exit the function (do nothing else).
            return;
        }

        transform.localPosition -= transform.localPosition.normalized * distanceToMove;
    }

    private Transform GetNextTarget()
    {
        // Get the enemy's target's (which is their parent) order in the hierarchy
        int currentTargetIndex = transform.parent.GetSiblingIndex();

        // If our current target is the last in the hierarchy...
        if (transform.parent.parent.childCount == currentTargetIndex + 1)
        {
            // ... then don't return a target. (we don't have one!)
            return null;
        }
        else
        {
            // Otherwise (If we do have a next target),
            // the next target is their current target's next sibling (wow this whole script sounds weird out-of-context)
            return transform.parent.parent.GetChild(currentTargetIndex + 1);
        }
    }

    // This function deals damage to the enemy. Projectiles call this function on collision.
    public void TakeDamage(float damage)
    {
        // Reduce our health by the amount of damage taken.
        health -= damage;
        // If we have 0 or less health...
        if (health <= 0)
        {
            // ... destroy this game object.
            Destroy(gameObject);
            
            // // And if we have a MoneyController in the scene...
            // if (MoneyController.instance != null)
            // {
            //     // ... then add money to the player's account.
            //     MoneyController.instance.Gain(moneyReward);
            // }
            
            // And if we have an attached audio source and sound on death...
            if (audioSource != null && soundOnDeath != null)
            {
                // ... then play the damage sound effect!
                // PlayClipAtPoint is an easy (but messy) way of playing a sound effect even after the object is destroyed.
                AudioSource.PlayClipAtPoint(soundOnDeath, new Vector3(0, 0, -10), audioSource.volume);
            }
        }
        else
        {
            // Otherwise, if we survive the hit, and we have an attached audio source and sound on damage...
            if (audioSource != null && soundOnDamage != null)
            {
                // ... then play the damage sound effect!
                audioSource.PlayOneShot(soundOnDamage);
            }
        }
    }

    private void OnDestroy()
    {
        onDie.Invoke();
    }
}
