using UnityEngine;

public class TutorialControllerV0 : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage = 0;

    void Start()
    {
        pages[currentPage].SetActive(true);
        Time.timeScale = 0;
    }

    public void NextPage()
    {
        currentPage += 1;
        if (currentPage >= pages.Length)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        pages[currentPage - 1].SetActive(false);
        pages[currentPage].SetActive(true);
    }
}
