using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixpos : MonoBehaviour
{
    public Vector3 pos;

    // Start is called before the first frame update
    void Awake()
    {
        pos = transform.position;
    }

    private void LateUpdate()
    {
        transform.position = pos;
    }
}
