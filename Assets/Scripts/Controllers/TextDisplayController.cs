using UnityEngine;
using TMPro;
using System;
using System.Collections; // Coroutine���g�����߂ɕK�v
using DG.Tweening;
using UnityEngine.UI;

public class TextDisplayController : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI mainText2;
    public GameObject nextMark;
    private GameObject nextMark_ins = null;
    public Image textBack;
    public Transform canvasTransform;
    public GameObject textBoxObject; // �e�L�X�g�{�b�N�X���̂̕\���E��\���𐧌�
    public float textSpeed_def = 0.05f;
    private float textSpeed;
    private Coroutine typeTextCoroutine;
    private Action onTextCompleted;
    void Start()
    {
        textSpeed = textSpeed_def;
        // �ŏ��̓e�L�X�g�{�b�N�X���\���ɂ��Ă���
        if (textBoxObject != null) textBoxObject.SetActive(false);
    }

    // ��������G�t�F�N�g
    private IEnumerator TypeTextEffect(string fullText)
    {
        Debug.Log("�\���J�n");
        mainText.text = "";
        mainText2.text = "";
        foreach (char c in fullText)
        {
            mainText.text += c;
            mainText.DOFade(1f, 0.1f);
            yield return new WaitForSeconds(textSpeed);
            mainText.DOFade(0f, 0f);
            mainText2.text += c;
        }
        typeTextCoroutine = null; // �R���[�`���I��
        Debug.Log("�\���I��");
        nextMark_ins = Instantiate(nextMark, canvasTransform);

        onTextCompleted?.Invoke(); // �\��������ʒm
    }
    public void setNextDestroy()
    {
        if (nextMark_ins != null)
        {
            Destroy(nextMark_ins);
            Debug.Log("�Ō�ɐ��������I�u�W�F�N�g���폜���܂����I");
        }
        else
        {
            Debug.Log("�폜����I�u�W�F�N�g������܂���B");
        }
    }
    // �������蒆������
    public bool IsTyping()
    {
        return typeTextCoroutine != null;
    }

    // �S���𑦎��\������
    public void ShowAllText()
    {
        if (IsTyping())
        {
            StopCoroutine(typeTextCoroutine);
            typeTextCoroutine = null;
            // ���݂̃e�L�X�g�̑S�����擾���ĕ\��
            // �i�R���[�`���̈������璼�ڎ��̂�����̂ŁA�����H�v���K�v�j
            // ���̗�ł͒P�����̂��߁AScenarioManager���ōēxDisplayLine���ĂԂ��A
            // �ʂ̕ϐ��ɑS����ێ����Ă����K�v������܂��B
            // �����ScenarioManager���Ő��䂷��̂ŁA�����ł̓R���[�`����~�̂݁B
            // ���ۂ�mainText.text = fullText; �̂悤�ȏ��������܂��B
            // �����ǂ������̂��߂ɁADisplayLine�őS����ێ�����悤�ɏC�����܂��B
        }
    }

    // ShowAllText�����P�����o�[�W����
    private string currentFullText; // �S����ێ�����ϐ�

    public void DisplayLine_Improved(float delay, string speaker, string content, Action onCompleted)
    {
        StartCoroutine(DelayedExecution(delay, speaker, content, onCompleted));
    }

    IEnumerator DelayedExecution(float delay, string speaker, string content, Action onCompleted)
    {

        mainText.text = "";
        mainText2.text = "";
        if (speaker.Equals("narration"))
        {
            nameText.text = "";
            textBack.DOFade(0f, 0.5f);
         /*   CharacterController characterController = GetComponent<CharacterController>();
            characterController.GetComponent<Image>().c;
            characterController.GetComponent<Image>().DOFade(0f, 0.5f);*/
        }
        else 
        {
            textBack.DOFade(0.27f, 0.5f).SetDelay(0.5f);
        }

        // �w�肳�ꂽ�b�������ҋ@
        yield return new WaitForSeconds(delay);

        if (textBoxObject != null) textBoxObject.SetActive(true);

        //�i���[�V�����̎��̓e�L�X�g�𒆉��ɔz�u�A����ȊO�͉��ɔz�u
        if (speaker.Equals("narration"))
        {
            mainText.transform.SetPositionAndRotation(new Vector3(960f, 540f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            mainText2.transform.SetPositionAndRotation(new Vector3(960f, 540f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            mainText.alignment = TextAlignmentOptions.Center;
            mainText2.alignment = TextAlignmentOptions.Center;
            nameText.text = "";
            mainText2.text = currentFullText = content.Replace("\\n", "\n");
            mainText2.alpha = 0f;
            mainText2.DOFade(1f, 1f);
            if (typeTextCoroutine != null)
            {
                StopCoroutine(typeTextCoroutine);
            }
            onTextCompleted = onCompleted;
            onTextCompleted?.Invoke(); // �\��������ʒm
        }
        else
        {
            mainText.transform.SetPositionAndRotation(new Vector3(960f, 124.2755f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            mainText2.transform.SetPositionAndRotation(new Vector3(960f, 124.2755f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            mainText.alignment = TextAlignmentOptions.TopLeft;
            mainText2.alignment = TextAlignmentOptions.TopLeft;
            nameText.text = "�y" + speaker + "�z";
            textSpeed = textSpeed_def;

            currentFullText = content.Replace("\\n", "\n");

            onTextCompleted = onCompleted;


            if (typeTextCoroutine != null)
            {
                StopCoroutine(typeTextCoroutine);
            }
            typeTextCoroutine = StartCoroutine(TypeTextEffect(currentFullText));
        }
    }

    public void ShowAllText_Improved()
    {
        if (IsTyping())
        {
            StopCoroutine(typeTextCoroutine);
            typeTextCoroutine = null;
            mainText.text = currentFullText; // �ێ����Ă������S����\��
            onTextCompleted?.Invoke(); // �\��������ʒm
        }
    }
}