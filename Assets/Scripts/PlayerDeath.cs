using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public Transform[] bones;
    public float explosionForce = 10f;
    public LayerMask layer;
    public AnimationCurve squishCurve;
    public float squishDuration = 0.5f;
    public GameObject explosion;
    public AudioClip explodeSound;

    void Start()
    {
        StartCoroutine(SquishBody());
    }

    IEnumerator SquishBody()
    {
        float t = 0f;
        while (t < squishDuration)
        {
            t += Time.deltaTime;
            var scale = transform.localScale;
            scale.x = squishCurve.Evaluate(t / squishDuration);
            transform.localScale = scale;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(ExplodeBones());
        GetComponent<AudioSource>().PlayOneShot(explodeSound);
        GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.18f);
    }

    IEnumerator ExplodeBones()
    {
        foreach (var bone in bones)
        {
            bone.parent = null;
            bone.localScale = Vector3.one;
            bone.rotation = Quaternion.identity;
            var go = bone.gameObject;
            var rb = go.AddComponent<Rigidbody2D>();
            Vector3 forceDir = bone.position - transform.position;
            forceDir = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            forceDir.Normalize();
            rb.AddForce(forceDir * explosionForce, ForceMode2D.Impulse);
            go.layer = layer.value;
        }

        yield return null;
    }
}
