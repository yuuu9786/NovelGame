using UnityEngine;
using UnityEngine.UI; // Imageを使うために必要
using System; // Actionを使うために必要
using System.Collections; // Coroutineを使うために必要
using DG.Tweening;

public class BackgroundController : MonoBehaviour
{
    public Image backgroundImage;
    public Image blackImage;
    public float fadeDuration = 0.5f; // フェード時間

    // アセットのパス（例: "Arts/Backgrounds/forest"）
    // 後でScriptableObjectやAddressablesに切り替えることを推奨
    public Sprite[] backgroundSprites; // Inspectorから設定

    private Action onBackgroundChanged;
    public bool isSwitchingBg = false;

    public void SetBackground(string bgName, Action onCompleted)
    {
        isSwitchingBg = true;
        Color p = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, 0f);
        blackImage.color = p;
        Sprite targetSprite = null;
        foreach (var sprite in backgroundSprites)
        {
            if (sprite.name == bgName)
            {
                targetSprite = sprite;
                break;
            }
        }

        if (targetSprite == null)
        {
            Debug.LogError($"背景画像 '{bgName}' が見つかりません。");
            onCompleted?.Invoke();
            return;
        }

        onBackgroundChanged = onCompleted;
        StartCoroutine(FadeBackground(targetSprite)); // フェードイン/アウトを伴う切り替え
    }

    IEnumerator FadeBackground(Sprite newSprite)
    {
        // 現在の背景をフェードアウト
        float timer = 0f;
        Color startColor = backgroundImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, timer / fadeDuration);
            backgroundImage.color = new Color(255f * timer / fadeDuration, 255f * timer / fadeDuration, 255f * timer / fadeDuration, 1f);
            blackImage.color = new Color(0f, 0f, 0f, 1 - alpha);
            yield return null;
        }
        backgroundImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f); // 完全に透明に

        // 新しい背景に切り替え
        backgroundImage.sprite = newSprite;

        // 新しい背景をフェードイン
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            backgroundImage.color = new Color(255f * timer / fadeDuration, 255f * timer / fadeDuration, 255f * timer / fadeDuration, 1f);
            blackImage.color = new Color(0f, 0f, 0f, 1 - alpha);
            yield return null;
        }
        backgroundImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f); // 完全に不透明に
        isSwitchingBg = false;
        onBackgroundChanged?.Invoke(); // 処理完了を通知
    }

    public void SetBackgroundToDark()
    {
        blackImage.DOFade(0.33f, 1f).SetEase(Ease.OutQuint);
    }

    public void SetBackgroundToTransparent()
    {
        blackImage.DOFade(0f, 0f);
    }
}