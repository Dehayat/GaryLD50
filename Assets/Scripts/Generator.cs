using System;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject[] items;
    public float spawnInterval = 3;
    public AnimationCurve intervalCurve;

    [Header("Generator Limits")]
    public float x1;
    public float y1, x2, y2;


    public bool isGenerating = false;

    public float overlapTestSize = 1f;

    private float lastSpawn = 0f;

    private void Update()
    {
        if (!isGenerating) return;
        float currentSpawnInteval = spawnInterval * intervalCurve.Evaluate(Time.time);
        if (Time.time >= lastSpawn + currentSpawnInteval)
        {
            lastSpawn = Time.time;
            DropItem();
        }
    }

    private void DropItem()
    {
        Vector3 itemPosition = GeneratePosition();
        //Vector3 itemPosition = new Vector3(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2), 0);
        GameObject.Instantiate(items[UnityEngine.Random.Range(0, items.Length)], itemPosition, Quaternion.identity);
    }
    private Vector3 GeneratePosition()
    {
        int maxTries = 10;
        int tries = 0;
        Vector3 pos = Vector3.zero;
        while (tries < maxTries)
        {
            pos = new Vector3(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2), 0);
            if (Physics2D.OverlapBox(new Vector2(pos.x, pos.y), new Vector2(overlapTestSize, overlapTestSize), 0) == null)
            {
                break;
            }
            tries++;
        }
        return pos;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3((x1 + x2) / 2, (y1 + y2) / 2), new Vector3(x2 - x1, y2 - y1));
    }
#endif
}
