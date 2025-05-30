using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    private Image flashImage;

    void Awake()
    {
        flashImage = GetComponent<Image>();
        SetAlpha(0f); // Start fully transparent
    }

    // Change here: now returns IEnumerator so caller can wait for it
    public IEnumerator FlashAndFreeze()
    {
        yield return FlashFreezeCoroutine();
    }

    private IEnumerator FlashFreezeCoroutine()
    {
        // Freeze game time
        Time.timeScale = 0f;

        int flashCount = 3;
        float flashDuration = 0.2f; // seconds per flash (on or off)

        for (int i = 0; i < flashCount; i++)
        {
            SetAlpha(1f); // flash on
            yield return new WaitForSecondsRealtime(flashDuration);

            SetAlpha(0f); // flash off
            yield return new WaitForSecondsRealtime(flashDuration);
        }

        // Unfreeze game time
        Time.timeScale = 1f;
    }

    private void SetAlpha(float alpha)
    {
        Color c = flashImage.color;
        c.a = alpha;
        flashImage.color = c;
    }
}
