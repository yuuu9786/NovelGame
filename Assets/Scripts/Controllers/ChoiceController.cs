using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChoiceController : MonoBehaviour
{
    public GameObject choicePanel; // 選択肢ボタンの親パネル
    public GameObject choiceButtonPrefab; // 選択肢ボタンのプレハブ
    private int count = 0;
    public void ShowChoices(String [] ScenarioBranch, String[] choiceData)
    {
        BackgroundController backgroundController = GetComponent<BackgroundController>();
        backgroundController.SetBackgroundToDark();
        // 既存のボタンをクリア
        foreach (Transform child in choicePanel.transform)
        {
            Destroy(child.gameObject);
        }
        // 渡されたデータに基づいてボタンを生成
        foreach (string choice in choiceData)
        {
            GameObject buttonGO = Instantiate(choiceButtonPrefab, choicePanel.transform);
            String branch = ScenarioBranch[count];
            // ボタンのテキストを設定
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choice;
            }
            Debug.Log(count + "の数");
            // ボタンのクリックイベントを設定
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                Debug.Log("ボタン押された、メソッド呼び出す");
                // このボタンのメソッド
                OnChoiceButtonClick(branch);
            });
            Image image = button.GetComponent<Image>();
            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
            float delay = (float) ((2 + count * 0.5) / 10);
            image.DOFade(0.82f, 0.15f).SetDelay(delay);
            text.DOFade(1f, 0.15f).SetDelay(delay);
            count++;
        }
        choicePanel.SetActive(true);
    }

    // 選択肢ボタンがクリックされたときに呼ばれる
    private void OnChoiceButtonClick(string branch)
    {
        TextDisplayController textDisplayController = GetComponent<TextDisplayController>();
        textDisplayController.setNextDestroy();
        count = 0;
        HideChoices(); // 選択肢を隠す
        BackgroundController backgroundController = GetComponent<BackgroundController>();
        backgroundController.SetBackgroundToTransparent();
        ScenarioManager scenarioManager = GetComponent<ScenarioManager>();
        ScenarioReader scenarioReader = GetComponent<ScenarioReader>();
        scenarioManager.scenarioData = scenarioReader.LoadScenario(branch); // 新しいシナリオをロード
        scenarioManager.currentLineIndex = 0;
        scenarioManager.ProcessCommands();
        scenarioManager.isChoiceMode = false;
        Debug.Log("シナリオロード完了");
    }

    // 選択肢を非表示にする
    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }
}
