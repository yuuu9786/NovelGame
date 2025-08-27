using UnityEngine;
using UnityEngine.UI; // Imageを使うために必要
using System; // Actionを使うために必要
using System.Collections.Generic; // Dictionaryを使うために必要
using DG.Tweening;
using Cysharp.Threading.Tasks;

// キャラクターごとの情報（名前、各種ポーズのSprite）を保持するクラス
[System.Serializable]
public class CharacterData
{
    public string characterName;
    // 各ポーズ名とそれに対応するSpriteのリスト
    // 例: "normal" -> sprite_akane_normal, "smile" -> sprite_akane_smile
    public List<PoseSprite> poses;
}
[System.Serializable]
public class PoseSprite
{
    public string poseName;
    public Sprite sprite;
}
public class CharacterController : MonoBehaviour
{
    public Image characterImageCenter;
    public bool isSwitchingPose = false;

    public List<CharacterData> allCharacters;

    private Dictionary<string, Image> activeCharacters = new Dictionary<string, Image>();

    void Start()
    {
        characterImageCenter.gameObject.SetActive(true);
        characterImageCenter.color = new Color(1, 1, 1, 0);
    }

    // ... GetImageByPosition, GetCharacterData, GetPoseSprite は前回と同じ ...

    // ShowCharacterを修正
    public void ShowCharacter(string charName, string position, string poseName, Action onCompleted)
    {
        Image targetImage = GetImageByPosition(position);
        if (targetImage == null) { onCompleted?.Invoke(); return; }

        CharacterData charData = GetCharacterData(charName);
        if (charData == null) { Debug.LogError($"キャラクターデータ '{charName}' が見つかりません。"); onCompleted?.Invoke(); return; }

        Sprite targetPoseSprite = GetPoseSprite(charData, poseName);
        if (targetPoseSprite == null) { Debug.LogError($"キャラクター '{charName}' のポーズ '{poseName}' が見つかりません。"); onCompleted?.Invoke(); return; }

        targetImage.sprite = targetPoseSprite;

        // ★★★★★ 修正点: アルファ値を1にして不透明にする ★★★★★
        targetImage.DOFade(1f, 0.1f).SetDelay(0.4f);

        // アクティブなキャラクターリストに追加
        if (activeCharacters.ContainsKey(charName))
        {
            activeCharacters[charName] = targetImage;
        }
        else
        {
            activeCharacters.Add(charName, targetImage);
        }

        Debug.Log($"キャラクター '{charName}' を位置 '{position}' にポーズ '{poseName}' で表示しました。");
        onCompleted?.Invoke();
    }

    // HideCharacterを修正（アルファ値を0に戻す）
    public void HideCharacter(string charName, Action onCompleted)
    {
        if (activeCharacters.TryGetValue(charName, out Image targetImage))
        {
            // ★★★★★ 修正点: アルファ値を0にして透明にする ★★★★★
            targetImage.DOFade(0f, 0.1f).SetDelay(0.4f);
           //targetImage.sprite = null;
            activeCharacters.Remove(charName);
            Debug.Log($"キャラクター '{charName}' を非表示にしました。");
        }
        else
        {
            Debug.LogWarning($"非表示にしようとしたキャラクター '{charName}' は表示されていませんでした。");
        }
        onCompleted?.Invoke();
    }

    // ChangePoseは前回と同じでOK
    public async void ChangePose(string charName, string poseName, Action onCompleted)
    {
        if (activeCharacters.TryGetValue(charName, out Image targetImage))
        {
            CharacterData charData = GetCharacterData(charName);
            if (charData == null) { Debug.LogError($"ポーズ変更対象のキャラクターデータ '{charName}' が見つかりません。"); onCompleted?.Invoke(); return; }
            isSwitchingPose = true;
            Sprite targetPoseSprite = GetPoseSprite(charData, poseName);
            if (targetPoseSprite == null) { Debug.LogError($"キャラクター '{charName}' のポーズ '{poseName}' が見つかりません。"); onCompleted?.Invoke(); return; }
            targetImage.DOFade(0f, 0.3f).SetEase(Ease.Linear);
            await UniTask.Delay(300);
            targetImage.sprite = targetPoseSprite;
            targetImage.DOFade(1f, 0.3f).SetEase(Ease.Linear);
            Debug.Log($"キャラクター '{charName}' のポーズを '{poseName}' に変更しました。");
            isSwitchingPose = false;
        }
        else
        {
            Debug.LogWarning($"ポーズ変更しようとしたキャラクター '{charName}' は表示されていませんでした。");
        }
        onCompleted?.Invoke();
    }

    // ... その他のメソッド ...
    private Image GetImageByPosition(string position)
    {
        switch (position.ToLower())
        {
            case "center": return characterImageCenter;
            default:
                Debug.LogWarning($"未知のキャラクター位置: {position}");
                return null;
        }
    }

    private CharacterData GetCharacterData(string charName)
    {
        return allCharacters.Find(c => c.characterName == charName);
    }

    private Sprite GetPoseSprite(CharacterData charData, string poseName)
    {
        if (charData == null || charData.poses == null) return null;
        return charData.poses.Find(p => p.poseName == poseName)?.sprite;
    }
}