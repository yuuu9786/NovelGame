using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChoiceController : MonoBehaviour
{
    public GameObject choicePanel; // �I�����{�^���̐e�p�l��
    public GameObject choiceButtonPrefab; // �I�����{�^���̃v���n�u
    private int count = 0;
    public void ShowChoices(String [] ScenarioBranch, String[] choiceData)
    {
        BackgroundController backgroundController = GetComponent<BackgroundController>();
        backgroundController.SetBackgroundToDark();
        // �����̃{�^�����N���A
        foreach (Transform child in choicePanel.transform)
        {
            Destroy(child.gameObject);
        }
        // �n���ꂽ�f�[�^�Ɋ�Â��ă{�^���𐶐�
        foreach (string choice in choiceData)
        {
            GameObject buttonGO = Instantiate(choiceButtonPrefab, choicePanel.transform);
            String branch = ScenarioBranch[count];
            // �{�^���̃e�L�X�g��ݒ�
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choice;
            }
            Debug.Log(count + "�̐�");
            // �{�^���̃N���b�N�C�x���g��ݒ�
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                Debug.Log("�{�^�������ꂽ�A���\�b�h�Ăяo��");
                // ���̃{�^���̃��\�b�h
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

    // �I�����{�^�����N���b�N���ꂽ�Ƃ��ɌĂ΂��
    private void OnChoiceButtonClick(string branch)
    {
        TextDisplayController textDisplayController = GetComponent<TextDisplayController>();
        textDisplayController.setNextDestroy();
        count = 0;
        HideChoices(); // �I�������B��
        BackgroundController backgroundController = GetComponent<BackgroundController>();
        backgroundController.SetBackgroundToTransparent();
        ScenarioManager scenarioManager = GetComponent<ScenarioManager>();
        ScenarioReader scenarioReader = GetComponent<ScenarioReader>();
        scenarioManager.scenarioData = scenarioReader.LoadScenario(branch); // �V�����V�i���I�����[�h
        scenarioManager.currentLineIndex = 0;
        scenarioManager.ProcessCommands();
        scenarioManager.isChoiceMode = false;
        Debug.Log("�V�i���I���[�h����");
    }

    // �I�������\���ɂ���
    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }
}
