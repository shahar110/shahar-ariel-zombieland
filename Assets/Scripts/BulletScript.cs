using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour 
{

    [Range(5, 100)]
    [Tooltip("After how long time should the bullet prefab be destroyed?")]
    public float destroyAfter;
    [Tooltip("If enabled the bullet destroys on impact")]
    public bool destroyOnImpact = false;
    [Tooltip("Minimum time after impact that the bullet is destroyed")]
    public float minDestroyTime;
    [Tooltip("Maximum time after impact that the bullet is destroyed")]
    public float maxDestroyTime;

    private void Start () 
    {
        StartCoroutine (DestroyAfter());
    }

    private void OnCollisionEnter (Collision collision) 
    {
        // If destroy on impact is false, start 
        // coroutine with random destroy timer
        if (!destroyOnImpact) 
            StartCoroutine (DestroyTimer ());
        // destroy bullet on impact
        else 
            Destroy (gameObject);
    }

    private IEnumerator DestroyTimer () 
    {
        yield return new WaitForSeconds
            (Random.Range(minDestroyTime, maxDestroyTime));
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfter () 
    {
        yield return new WaitForSeconds (destroyAfter);
        Destroy (gameObject);
    }
}