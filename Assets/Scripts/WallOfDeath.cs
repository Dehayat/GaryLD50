using System;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    public float maxMoveSpeed = 5f;
    public float moveSpeed = 5f;
    public Generator generator;
    public AnimationCurve speedCurve;

    private Collider2D col2D;
    private bool isMoving = false;
    private float startTime;
    private int stops = 0;

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
            float currentTime = Time.time - startTime;
            float currentMoveSpeed = Mathf.Clamp(moveSpeed, 0, maxMoveSpeed) * speedCurve.Evaluate(currentTime);
            transform.Translate(Vector3.right * currentMoveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Slow(float slowAmount)
    {
        moveSpeed -= slowAmount;
    }

    public void StartWall()
    {
        startTime = Time.time;
        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
        stops++;
    }
    public void Resume()
    {
        stops--;
        if (stops == 0)
        {
            isMoving = true;
        }
    }
}
