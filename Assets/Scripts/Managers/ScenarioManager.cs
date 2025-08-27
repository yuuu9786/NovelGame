using UnityEngine;
using System.Collections.Generic;
using System.Collections;

// �V�i���I��1�s��\������N���X���C��
[System.Serializable]
public class ScenarioLine
{
    public string type;        // "text", "bg", "char_show", "char_hide", "char_pose", "end"
    public string speakerName; // type��"text"�̏ꍇ
    public string content;     // �e�L�X�g���e�A�摜�t�@�C�����A�L�����N�^�[���Ȃ�
    public string position;    // �L�����N�^�[�̕\���ʒu ("left", "center", "right")
    public string poseName;    // �L�����N�^�[�̃|�[�Y�� (type��"char_pose"�̏ꍇ)
    public string [] ScenarioBranch;
    public string [] choiceData;
}
// ScenarioLine�N���X�͕ύX�Ȃ�

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
        // ���͑҂���ԂŃN���b�N���ꂽ��
        if (isWaitingForUserInput && Input.GetMouseButtonDown(0) && !isChoiceMode)
        {
            textDisplayController.setNextDestroy();
            isWaitingForUserInput = true;
            // �e�L�X�g���蒆�Ȃ�S���\���A�����łȂ���Ύ��̃R�}���h��
            if (textDisplayController.IsTyping())
            {
                textDisplayController.ShowAllText_Improved();
                // ShowAllText_Improved���ŃR�[���o�b�N���Ă΂�AisWaitingForUserInput���ēxtrue�ɂȂ�̂ŁA�����őҋ@
            }
            else
            {
                // ���̃R�}���h�������J�n
                ProcessCommands();
            }
        }
    }

    // �R�}���h��A���������郁�\�b�h
    public void ProcessCommands()
    {
        // �V�i���I�̍Ō�܂Ń��[�v
        while (currentLineIndex < scenarioData.Count)
        {
            ScenarioLine line = scenarioData[currentLineIndex];

            // "text"�R�}���h��������A�����𒆒f���ē��͂�҂�
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
                    isWaitingForUserInput = true; // �\��������A���͑҂���Ԃ�
                });
                currentLineIndex++;
                return; // �����Ń��\�b�h�𔲂��āAUpdate�ł̃��[�U�[���͂�҂�
            }else if (line.type == "choice")
            {
                isChoiceMode = true;
                // �I������\�����A���[�U�[�̑I����҂�
                choiceController.ShowChoices(line.ScenarioBranch, line.choiceData);
                return; // �I�������\�����ꂽ��A�����𒆒f���đ҂�
            }

            // "text"�ȊO�̃R�}���h�͑������s
            ProcessSingleCommand(line);
            currentLineIndex++;
        }

        // while���[�v�𔲂��� = �S�ẴR�}���h���������I����
        Debug.Log("�V�i���I�̍Ō�܂œ��B���܂����B");
        // �����ŃG���f�B���O������^�C�g���ւ̑J�ڂȂǂ��s��
    }

    // 1�s�̃R�}���h����������w���p�[���\�b�h
    private void ProcessSingleCommand(ScenarioLine line)
    {
        switch (line.type)
        {
            case "bg":
                // �R�[���o�b�N�͕s�v�Ȃ̂�null��n��
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
                Debug.Log("�����I�ȃV�i���I�I���R�}���h�B");
                // SceneManager.LoadScene("StartScene");
                break;
            default:
                Debug.LogWarning($"�s���ȃV�i���I�R�}���h: {line.type}");
                break;
        }
    }

    IEnumerator DelayedExecution(float delay)
    {
        // �w�肳�ꂽ�b�������ҋ@
        yield return new WaitForSeconds(delay);

        // --- �������牺�ɁA�x�����������������L�q ---
        Debug.Log("3�b�o�߁I�x�����������������s���܂��B Time: " + Time.time);

        if (scenarioData == null || scenarioData.Count == 0)
        {
            Debug.LogError("�V�i���I�f�[�^���ݒ肳��Ă��܂���I");
        }
        ProcessCommands();
    }
}