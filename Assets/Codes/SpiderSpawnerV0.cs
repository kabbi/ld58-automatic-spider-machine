using System.Collections;
using UnityEngine;

public class SpiderSpawnerV0 : MonoBehaviour
{
    public float interval;
    public float pauseTime;
    public GameObject prefab;
    public Transform spawnPoint;

    void Start()
    {
        StartCoroutine(Run());
    }

    public void FeedMoney()
    {
        StopAllCoroutines();
        StartCoroutine(Feed());
    }

    IEnumerator Run()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    IEnumerator Feed()
    {
        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(Run());
    }
}
