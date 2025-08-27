using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Image‚ðŽg‚¤‚½‚ß‚É•K—v
using DG.Tweening;

public class TextMotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text text = GetComponent<Text>();
        text.DOFade(1f, 2f);
        transform.DOLocalMoveY(20f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
