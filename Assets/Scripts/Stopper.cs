using UnityEngine;
using System.Collections;

public class Stopper : BasePickup
{
    public float lifeTime = 8f;
    public float lifeTimeAfterUse = 3f;
    public LayerMask boundsLayer;
    public Transform leftBone;
    public Transform leftBoneOffseter;
    public Transform rightBone;
    public Transform rightBoneOffseter;
    public AnimationCurve animationCurve;
    public float animationLength;

    private Collider2D pickupTrigger;
    private Rigidbody2D ownerRB;
    private WallOfDeath affectedWallOfDeath = null;
    private Animator anim;
    private float leftBoneOffset;
    private float rightBoneOffset;

    private void Awake()
    {
        pickupTrigger = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        ownerRB = pickupTrigger.attachedRigidbody;
        leftBoneOffset = (leftBoneOffseter.position - leftBone.position).x;
        rightBoneOffset = (rightBoneOffseter.position - rightBone.position).x;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void AffectWall(WallOfDeath wod)
    {
        affectedWallOfDeath = wod;
        wod.Stop();
    }

    private void OnDestroy()
    {
        Player.instance.BreakEffect(transform.position);
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

    IEnumerator ExtendAnimation()
    {
        var leftHit = Physics2D.Raycast(transform.position, Vector2.left, 50, boundsLayer.value, 0);
        var rightHit = Physics2D.Raycast(transform.position, Vector2.right, 50, boundsLayer.value, 0);

        float t = 0;

        float startLeftX = leftBone.position.x;
        float leftX = leftHit.collider.bounds.max.x - leftBoneOffset;
        float startRightX = rightBone.position.x;
        float rightX = rightHit.collider.bounds.min.x - rightBoneOffset;

        while (t < animationLength)
        {


            Vector3 leftBonePos = leftBone.position;
            float lerpPos = animationCurve.Evaluate(t / animationLength);
            leftBonePos.x = Mathf.LerpUnclamped(startLeftX, leftX, lerpPos);
            leftBone.position = leftBonePos;

            Vector3 rightBonePos = rightBone.position;
            rightBonePos.x = Mathf.Lerp(startRightX, rightX, lerpPos);
            rightBone.position = rightBonePos;

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StopWall();
    }

    public void StopWall()
    {
        AffectWall(FindObjectOfType<WallOfDeath>());
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
        //anim.Play("Activate");
        StartCoroutine(ExtendAnimation());
    }
    #endregion
}
