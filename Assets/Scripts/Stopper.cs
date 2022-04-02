using UnityEngine;

public class Stopper : BasePickup
{
    public float lifeTime = 8f;
    public float lifeTimeAfterUse = 3f;

    private Collider2D pickupTrigger;
    private Rigidbody2D ownerRB;
    private WallOfDeath affectedWallOfDeath = null;
    private Animator anim;

    private void Awake()
    {
        pickupTrigger = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        ownerRB = pickupTrigger.attachedRigidbody;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void AffectWall(WallOfDeath wod)
    {
        Debug.Log(wod);
        affectedWallOfDeath = wod;
        wod.Stop();
    }

    private void OnDestroy()
    {
        RemoveWallEffect();
    }

    private void RemoveWallEffect()
    {
        if (affectedWallOfDeath != null)
        {
            affectedWallOfDeath.Resume();
            affectedWallOfDeath = null;
        }
    }

    #region PickUp
    public override void PickUp()
    {
        pickupTrigger.enabled = false;
        ownerRB.simulated = false;
        RemoveWallEffect();
    }
    public override void Drop()
    {
        Destroy(gameObject, lifeTimeAfterUse);
        anim.Play("Activate");
    }

    public void StopWall()
    {
        AffectWall(FindObjectOfType<WallOfDeath>());
    }
    #endregion
}
