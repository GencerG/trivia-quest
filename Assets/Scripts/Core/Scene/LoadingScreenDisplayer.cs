using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingScreenDisplayer : MonoBehaviour
{
    [SerializeField] private Image _loadingImage;

    private const float SPEED = 2f;

    public IEnumerator FadeIn()
    {
        var color = _loadingImage.color;

        while (color.a <= 1f)
        {
            color.a += Time.deltaTime * SPEED;
            _loadingImage.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        var color = _loadingImage.color;

        while (color.a >= 0f)
        {
            color.a -= Time.deltaTime * SPEED;
            _loadingImage.color = color;
            yield return null;
        }
    }
}
