using UnityEngine;
using UnityEngine.UI;

public class ProgressBarV0 : MonoBehaviour
{
    public Image bar;
    public float progress;

    void Update()
    {
        bar.fillAmount = progress;
    }
}
