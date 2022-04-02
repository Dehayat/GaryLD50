using UnityEngine;

public class Slower : BasePickup
{
    public float slowAmount = 0.02f;
    public float lifeTime = 5f;

    private Collider2D pickupTrigger;
    private Rigidbody2D ownerRB;
    private WallOfDeath affectedWallOfDeath = null;

    private void Awake()
    {
        pickupTrigger = GetComponent<Collider2D>();
        ownerRB = pickupTrigger.attachedRigidbody;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var wallOfDeath = collision.gameObject.GetComponent<WallOfDeath>();
        if (affectedWallOfDeath == null && wallOfDeath != null)
        {
            AffectWall(wallOfDeath);
        }
    }

    private void AffectWall(WallOfDeath wod)
    {
        affectedWallOfDeath = wod;
        wod.Slow(slowAmount);
    }

    private void OnDestroy()
    {
        RemoveWallEffect();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var wallOfDeath = collision.gameObject.GetComponent<WallOfDeath>();
        if (wallOfDeath != null && wallOfDeath == affectedWallOfDeath)
        {
            RemoveWallEffect();
        }
    }

    private void RemoveWallEffect()
    {
        if (affectedWallOfDeath != null)
        {
            affectedWallOfDeath.Slow(-slowAmount);
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
        pickupTrigger.enabled = true;
        ownerRB.simulated = true;
    }
    #endregion
}
