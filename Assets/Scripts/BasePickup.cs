using UnityEngine;

public class BasePickup : MonoBehaviour
{
    public virtual void PickUp()
    {
    }

    public virtual void Drop()
    {
    }

    public virtual GameObject GetOwner()
    {
        return gameObject;
    }
}
