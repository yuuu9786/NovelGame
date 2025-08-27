using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理機能を使うために必要

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// 指定された名前のシーンをロードします。
    /// このメソッドはUIボタンのOnClickイベントから呼び出すことを想定しています。
    /// </summary>
    /// <param name="sceneName">ロードするシーンの名前</param>
    public void LoadScene(string sceneName)
    {
        // SceneManager.LoadScene() メソッドを使ってシーンをロードします。
        SceneManager.LoadScene(sceneName);

        // デバッグログを出力（任意）
        Debug.Log($"シーンをロードしました: {sceneName}");
    }
}