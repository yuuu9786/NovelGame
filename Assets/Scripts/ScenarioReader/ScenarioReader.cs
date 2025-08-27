using System.Collections;
using System.Collections.Generic;
using System.IO; // ファイル読み込みに必要
using UnityEngine;

public class ScenarioReader : MonoBehaviour
{
    public string startFileName = "prologue.json";
    private List<ScenarioLine> currentScenario; // 現在のシナリオデータ
    private bool isWaitingForUserInput = false;

    // JSONファイル全体に対応するクラス
    [System.Serializable]
    public class ScenarioDataFile
    {
        public List<ScenarioLine> lines;
    }

    // ... Updateメソッドは前回と同様 ...

    // シナリオファイルをロードするメソッド
    public List<ScenarioLine> LoadScenario(string fileName)
    {
        // ファイルパスを構築
        string filePath = Path.Combine(Application.streamingAssetsPath, "Scenarios", fileName);

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            ScenarioDataFile dataFile = JsonUtility.FromJson<ScenarioDataFile>(jsonText);

            // 既存のシナリオにアペンド（追加）するか、完全に置き換えるか
            // 今回は分岐でファイルが切り替わるので、置き換え（=）でOK
            this.currentScenario = dataFile.lines;
            Debug.Log($"シナリオ '{fileName}' を返り値として返却します。");
            List<ScenarioLine> lines = dataFile.lines;
            List<ScenarioLine> currentScenario = lines;
            foreach(ScenarioLine b in currentScenario) {
                Debug.Log(b.type);
                Debug.Log(b.speakerName);
                Debug.Log(b.content);
                Debug.Log(b.position);
            }
            return currentScenario;
        }
        else
        {
            Debug.LogError($"シナリオファイルが見つかりません: {filePath}");
            currentScenario = new List<ScenarioLine>(); // 空のリストでエラーを防ぐ
            return currentScenario;
        }
    }
}
