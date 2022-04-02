using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{

    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private BasePickup currentPickup;

    private BasePickup currentItem;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    public void MoveInput(CallbackContext input)
    {
        var movement = input.ReadValue<Vector2>();
        rb.velocity = movement * moveSpeed;
        if (movement.sqrMagnitude > Mathf.Epsilon)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }

    public void ActionInput(CallbackContext input)
    {
        if (input.performed)
        {
            if (currentItem != null)
            {
                DropCurrentItem();
            }
            if (currentPickup != null)
            {
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

}
