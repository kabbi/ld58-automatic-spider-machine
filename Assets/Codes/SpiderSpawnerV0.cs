using System.Collections;
using UnityEngine;

public class SpiderSpawnerV0 : MonoBehaviour
{
    public float interval;
    public float pauseTime;
    public GameObject prefab;
    public GameObject pausedIndicator;
    public Transform spawnPoint;
    public Transform indicator;
    public float indicatorAngleRange = 85 * 2;
    private float indicatorStartingAngle;
    private float nextTriggerTime;
    private bool paused;
    private AudioSource audioSource;
    public AudioClip soundEffect;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Run());
        indicatorStartingAngle = indicator.localEulerAngles.z;
    }

    void Update()
    {
        if (paused)
        {
            indicator.localEulerAngles = new Vector3(0, 0, indicatorStartingAngle);
        }
        if (!paused)
        {
            float angle = 1 - (nextTriggerTime - Time.time) / interval;
            indicator.localEulerAngles = new Vector3(0, 0, indicatorStartingAngle + angle * indicatorAngleRange);
        }
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
            audioSource.PlayOneShot(soundEffect);
            nextTriggerTime = Time.time + interval;
            yield return new WaitForSeconds(interval);
            Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    IEnumerator Feed()
    {
        paused = true;
        pausedIndicator.SetActive(true);
        yield return new WaitForSeconds(pauseTime);
        pausedIndicator.SetActive(false);
        paused = false;

        StartCoroutine(Run());
    }
}
