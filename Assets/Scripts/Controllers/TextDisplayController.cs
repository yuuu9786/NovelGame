using UnityEngine;
using TMPro;
using System;
using System.Collections; // Coroutineを使うために必要
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
    public GameObject textBoxObject; // テキストボックス自体の表示・非表示を制御
    public float textSpeed_def = 0.05f;
    private float textSpeed;
    private Coroutine typeTextCoroutine;
    private Action onTextCompleted;
    void Start()
    {
        textSpeed = textSpeed_def;
        // 最初はテキストボックスを非表示にしておく
        if (textBoxObject != null) textBoxObject.SetActive(false);
    }

    // 文字送りエフェクト
    private IEnumerator TypeTextEffect(string fullText)
    {
        Debug.Log("表示開始");
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
        typeTextCoroutine = null; // コルーチン終了
        Debug.Log("表示終了");
        nextMark_ins = Instantiate(nextMark, canvasTransform);

        onTextCompleted?.Invoke(); // 表示完了を通知
    }
    public void setNextDestroy()
    {
        if (nextMark_ins != null)
        {
            Destroy(nextMark_ins);
            Debug.Log("最後に生成したオブジェクトを削除しました！");
        }
        else
        {
            Debug.Log("削除するオブジェクトがありません。");
        }
    }
    // 文字送り中か判定
    public bool IsTyping()
    {
        return typeTextCoroutine != null;
    }

    // 全文を即時表示する
    public void ShowAllText()
    {
        if (IsTyping())
        {
            StopCoroutine(typeTextCoroutine);
            typeTextCoroutine = null;
            // 現在のテキストの全文を取得して表示
            // （コルーチンの引数から直接取るのが難しいので、少し工夫が必要）
            // この例では単純化のため、ScenarioManager側で再度DisplayLineを呼ぶか、
            // 別の変数に全文を保持しておく必要があります。
            // 今回はScenarioManager側で制御するので、ここではコルーチン停止のみ。
            // 実際はmainText.text = fullText; のような処理をします。
            // →より良い実装のために、DisplayLineで全文を保持するように修正します。
        }
    }

    // ShowAllTextを改善したバージョン
    private string currentFullText; // 全文を保持する変数

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

        // 指定された秒数だけ待機
        yield return new WaitForSeconds(delay);

        if (textBoxObject != null) textBoxObject.SetActive(true);

        //ナレーションの時はテキストを中央に配置、それ以外は下に配置
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
            onTextCompleted?.Invoke(); // 表示完了を通知
        }
        else
        {
            mainText.transform.SetPositionAndRotation(new Vector3(960f, 124.2755f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            mainText2.transform.SetPositionAndRotation(new Vector3(960f, 124.2755f, 0f), new Quaternion(0f, 0f, 0f, 0f));
            mainText.alignment = TextAlignmentOptions.TopLeft;
            mainText2.alignment = TextAlignmentOptions.TopLeft;
            nameText.text = "【" + speaker + "】";
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
            mainText.text = currentFullText; // 保持しておいた全文を表示
            onTextCompleted?.Invoke(); // 表示完了を通知
        }
    }
}