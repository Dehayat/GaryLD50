using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float moveSpeed = 5f;
    public GameObject destructionEffect;
    [Header("Sounds")]
    public AudioClip pickUpSound;
    public AudioClip dropSound;
    public AudioClip extendSound;
    public AudioClip breakSound;

    private Rigidbody2D rb;
    private BasePickup currentPickup;

    private BasePickup currentItem;
    private Animator anim;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    public void MoveInput(CallbackContext input)
    {
        var movement = input.ReadValue<Vector2>();
        rb.velocity = movement * moveSpeed;
        if (movement.sqrMagnitude > Mathf.Epsilon)
        {
            anim.SetBool("walk", true);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            anim.SetBool("walk", false);
            audioSource.Stop();
        }
    }

    public void ActionInput(CallbackContext input)
    {
        if (input.performed)
        {
            if (currentItem != null)
            {
                audioSource.PlayOneShot(dropSound);
                if (currentItem.GetComponent<Stopper>() != null)
                {
                    audioSource.PlayOneShot(extendSound);
                }
                DropCurrentItem();
            }
            else if (currentPickup != null)
            {
                audioSource.PlayOneShot(pickUpSound);
                PickUpCurrentPickup();
            }
        }
    }

    private void PickUpCurrentPickup()
    {
        currentItem = currentPickup;
        currentItem.GetOwner().transform.parent = transform;
        currentPickup = null;
        currentItem.PickUp();
    }

    private void DropCurrentItem()
    {
        currentItem.GetOwner().transform.parent = null;
        currentItem.Drop();
        currentItem = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var trigger = collision.gameObject.GetComponent<Trigger>();
        if (trigger != null)
        {
            currentPickup = trigger.pickUp;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var trigger = collision.gameObject.GetComponent<Trigger>();
        if (trigger != null && trigger.pickUp == currentPickup)
        {
            currentPickup = null;
        }
    }

    public void BreakEffect(Vector3 position)
    {
        GameObject.Instantiate(destructionEffect, position, Quaternion.identity);
        audioSource.PlayOneShot(breakSound);
    }

}
