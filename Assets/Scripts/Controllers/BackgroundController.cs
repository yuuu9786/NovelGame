using UnityEngine;
using UnityEngine.UI; // Image���g�����߂ɕK�v
using System; // Action���g�����߂ɕK�v
using System.Collections; // Coroutine���g�����߂ɕK�v
using DG.Tweening;

public class BackgroundController : MonoBehaviour
{
    public Image backgroundImage;
    public Image blackImage;
    public float fadeDuration = 0.5f; // �t�F�[�h����

    // �A�Z�b�g�̃p�X�i��: "Arts/Backgrounds/forest"�j
    // ���ScriptableObject��Addressables�ɐ؂�ւ��邱�Ƃ𐄏�
    public Sprite[] backgroundSprites; // Inspector����ݒ�

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
            Debug.LogError($"�w�i�摜 '{bgName}' ��������܂���B");
            onCompleted?.Invoke();
            return;
        }

        onBackgroundChanged = onCompleted;
        StartCoroutine(FadeBackground(targetSprite)); // �t�F�[�h�C��/�A�E�g�𔺂��؂�ւ�
    }

    IEnumerator FadeBackground(Sprite newSprite)
    {
        // ���݂̔w�i���t�F�[�h�A�E�g
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
        backgroundImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f); // ���S�ɓ�����

        // �V�����w�i�ɐ؂�ւ�
        backgroundImage.sprite = newSprite;

        // �V�����w�i���t�F�[�h�C��
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            backgroundImage.color = new Color(255f * timer / fadeDuration, 255f * timer / fadeDuration, 255f * timer / fadeDuration, 1f);
            blackImage.color = new Color(0f, 0f, 0f, 1 - alpha);
            yield return null;
        }
        backgroundImage.color = new Color(startColor.r, startColor.g, startColor.b, 1f); // ���S�ɕs������
        isSwitchingBg = false;
        onBackgroundChanged?.Invoke(); // ����������ʒm
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