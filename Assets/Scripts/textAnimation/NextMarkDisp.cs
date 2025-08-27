using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextMarkDisp : MonoBehaviour
{
    public void Start()
    {
        transform.DOMove(new Vector3(0f, -10f, 0f), 0.75f).SetRelative().SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
