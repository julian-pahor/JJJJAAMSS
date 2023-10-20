using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TransitionScreen : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(Vector2.zero,1f).SetEase(Ease.OutBounce).OnComplete( ()=> gameObject.SetActive(false));
    }

}
