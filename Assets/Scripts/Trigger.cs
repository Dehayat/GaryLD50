using UnityEngine;

public class Trigger : MonoBehaviour
{
    public BasePickup pickUp;

    private void Awake()
    {
        pickUp = GetComponentInParent<BasePickup>();
    }
}
