using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TransitionScreen : MonoBehaviour
{
    bool active = true;
 
    private void Update()
    {
        if(Input.anyKeyDown && active)
        {
            active = false;
            transform.DOScale(Vector2.zero, 1f).SetEase(Ease.OutBounce).OnComplete(() => gameObject.SetActive(false));
        }
    }

}
