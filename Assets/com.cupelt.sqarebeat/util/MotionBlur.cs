using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionBlur : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 8f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        GetComponent<RectTransform>().anchoredPosition = target.GetComponent<RectTransform>().anchoredPosition;
        Vector3 desiredPosition = target.localScale + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.localScale, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.localScale = smoothedPosition;

        if (target.Equals(transform))
            return;

        Color fixedColor = target.GetComponent<Image>().color;
        fixedColor.a = fixedColor.a * 0.75f;
        GetComponent<Image>().color = fixedColor;
    }
}
