using System.Collections;
using System.Collections.Generic;
using System.IO; // �t�@�C���ǂݍ��݂ɕK�v
using UnityEngine;

public class ScenarioReader : MonoBehaviour
{
    public string startFileName = "prologue.json";
    private List<ScenarioLine> currentScenario; // ���݂̃V�i���I�f�[�^
    private bool isWaitingForUserInput = false;

    // JSON�t�@�C���S�̂ɑΉ�����N���X
    [System.Serializable]
    public class ScenarioDataFile
    {
        public List<ScenarioLine> lines;
    }

    // ... Update���\�b�h�͑O��Ɠ��l ...

    // �V�i���I�t�@�C�������[�h���郁�\�b�h
    public List<ScenarioLine> LoadScenario(string fileName)
    {
        // �t�@�C���p�X���\�z
        string filePath = Path.Combine(Application.streamingAssetsPath, "Scenarios", fileName);

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            ScenarioDataFile dataFile = JsonUtility.FromJson<ScenarioDataFile>(jsonText);

            // �����̃V�i���I�ɃA�y���h�i�ǉ��j���邩�A���S�ɒu�������邩
            // ����͕���Ńt�@�C�����؂�ւ��̂ŁA�u�������i=�j��OK
            this.currentScenario = dataFile.lines;
            Debug.Log($"�V�i���I '{fileName}' ��Ԃ�l�Ƃ��ĕԋp���܂��B");
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
            Debug.LogError($"�V�i���I�t�@�C����������܂���: {filePath}");
            currentScenario = new List<ScenarioLine>(); // ��̃��X�g�ŃG���[��h��
            return currentScenario;
        }
    }
}
