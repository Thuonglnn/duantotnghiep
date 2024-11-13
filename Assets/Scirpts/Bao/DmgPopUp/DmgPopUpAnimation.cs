using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgPopUpAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightcurve;

    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    private void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, 1 + heightcurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }
}
