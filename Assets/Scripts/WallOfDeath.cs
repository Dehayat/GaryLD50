using System;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    public float maxMoveSpeed = 5f;
    public float moveSpeed = 5f;
    public Generator generator;

    private Collider2D col2D;
    private bool isMoving = true;

    private void Awake()
    {
        col2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        generator.x1 = col2D.bounds.max.x + 1f;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            float currentMoveSpeed = Mathf.Clamp(moveSpeed, 0, maxMoveSpeed);
            transform.Translate(Vector3.right * currentMoveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Slow(float slowAmount)
    {
        moveSpeed -= slowAmount;
    }

    public void Stop()
    {
        isMoving = false;
    }
    public void Resume()
    {
        isMoving = true;
    }
}
