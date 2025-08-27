using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// シナリオの1行を表現するクラスを修正
[System.Serializable]
public class ScenarioLine
{
    public string type;        // "text", "bg", "char_show", "char_hide", "char_pose", "end"
    public string speakerName; // typeが"text"の場合
    public string content;     // テキスト内容、画像ファイル名、キャラクター名など
    public string position;    // キャラクターの表示位置 ("left", "center", "right")
    public string poseName;    // キャラクターのポーズ名 (typeが"char_pose"の場合)
    public string [] ScenarioBranch;
    public string [] choiceData;
}
// ScenarioLineクラスは変更なし

public class ScenarioManager : MonoBehaviour
{
    public TextDisplayController textDisplayController;
    public BackgroundController backgroundController;
    public CharacterController characterController;
    public ChoiceController choiceController;
    public ScenarioReader scenarioReader;
    public List<ScenarioLine> scenarioData;

    public int currentLineIndex = 0;
    private bool isWaitingForUserInput = false;
    public bool isChoiceMode = false;
    private float textDelay= 0f;
    void Start()
    {
        StartCoroutine(DelayedExecution(2.0f));
        scenarioData = scenarioReader.LoadScenario("prologue.json");
    }

    void Update()
    {
        // 入力待ち状態でクリックされたら
        if (isWaitingForUserInput && Input.GetMouseButtonDown(0) && !isChoiceMode)
        {
            textDisplayController.setNextDestroy();
            isWaitingForUserInput = true;
            // テキスト送り中なら全文表示、そうでなければ次のコマンドへ
            if (textDisplayController.IsTyping())
            {
                textDisplayController.ShowAllText_Improved();
                // ShowAllText_Improved内でコールバックが呼ばれ、isWaitingForUserInputが再度trueになるので、ここで待機
            }
            else
            {
                // 次のコマンド処理を開始
                ProcessCommands();
            }
        }
    }

    // コマンドを連続処理するメソッド
    public void ProcessCommands()
    {
        // シナリオの最後までループ
        while (currentLineIndex < scenarioData.Count)
        {
            ScenarioLine line = scenarioData[currentLineIndex];

            // "text"コマンドが来たら、処理を中断して入力を待つ
            if (line.type == "text")
            {
                if (backgroundController.isSwitchingBg)
                {
                    textDelay = 1f;
                }
                else if (characterController.isSwitchingPose)
                {
                    textDelay = 0.6f;
                }
                else
                {
                    textDelay = 0f;
                }
                textDisplayController.DisplayLine_Improved(textDelay, line.speakerName, line.content, () =>
                {
                    isWaitingForUserInput = true; // 表示完了後、入力待ち状態へ
                });
                currentLineIndex++;
                return; // ここでメソッドを抜けて、Updateでのユーザー入力を待つ
            }else if (line.type == "choice")
            {
                isChoiceMode = true;
                // 選択肢を表示し、ユーザーの選択を待つ
                choiceController.ShowChoices(line.ScenarioBranch, line.choiceData);
                return; // 選択肢が表示されたら、処理を中断して待つ
            }

            // "text"以外のコマンドは即時実行
            ProcessSingleCommand(line);
            currentLineIndex++;
        }

        // whileループを抜けた = 全てのコマンドを処理し終えた
        Debug.Log("シナリオの最後まで到達しました。");
        // ここでエンディング処理やタイトルへの遷移などを行う
    }

    // 1行のコマンドを処理するヘルパーメソッド
    private void ProcessSingleCommand(ScenarioLine line)
    {
        switch (line.type)
        {
            case "bg":
                // コールバックは不要なのでnullを渡す
                backgroundController.SetBackground(line.content, null);
                break;
            case "char_show":
                characterController.ShowCharacter(line.content, line.position, "normal", null);
                break;
            case "char_hide":
                characterController.HideCharacter(line.content, null);
                break;
            case "char_pose":
                characterController.ChangePose(line.content, line.poseName, null);
                break;
            case "end":
                Debug.Log("明示的なシナリオ終了コマンド。");
                // SceneManager.LoadScene("StartScene");
                break;
            default:
                Debug.LogWarning($"不明なシナリオコマンド: {line.type}");
                break;
        }
    }

    IEnumerator DelayedExecution(float delay)
    {
        // 指定された秒数だけ待機
        yield return new WaitForSeconds(delay);

        // --- ここから下に、遅延させたい処理を記述 ---
        Debug.Log("3秒経過！遅延させた処理を実行します。 Time: " + Time.time);

        if (scenarioData == null || scenarioData.Count == 0)
        {
            Debug.LogError("シナリオデータが設定されていません！");
        }
        ProcessCommands();
    }
}