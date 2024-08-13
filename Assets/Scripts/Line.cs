using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    float _fTime = 0f;

    void Update()
    {
        _fTime += Time.deltaTime;

        if (0.2f < _fTime)
            Destroy(gameObject);
    }
}
