using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ParryPopUp : MonoBehaviour
{
    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOPunchScale(Vector3.one, 0.5f, 1, 0.2f));
        seq.Append(transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                Destroy(this.gameObject);
        }));
    }

    private void Update()
    {
        transform.position = pos;
        pos += Vector3.up * 0.1f * Time.deltaTime;
    }
}
