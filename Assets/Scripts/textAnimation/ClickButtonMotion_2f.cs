using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Image‚ðŽg‚¤‚½‚ß‚É•K—v
using TMPro;
using DG.Tweening;
public class ClickButtonMotion_2f : MonoBehaviour
{
    [SerializeField] float dispSecond;
    [SerializeField] float delaySecond;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.alpha = 0f;
        text.DOFade(1f, dispSecond).SetDelay(delaySecond);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
