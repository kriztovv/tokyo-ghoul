using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
public class ObjectLerp : MonoBehaviour
{

    public float moveDistance = 10f;
    public float lerpSpeed = 5f;

    private Vector3 originalLocalPosition;
    private Vector3 targetLocalPosition;

    private bool isMoving = false;

    void Start()
    {
        originalLocalPosition = transform.localPosition;
        targetLocalPosition = originalLocalPosition + transform.forward * moveDistance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            StartCoroutine(MoveObjectCoroutine());
        }
    }

    IEnumerator MoveObjectCoroutine()
    {
        isMoving = true;
        yield return MoveObject(targetLocalPosition);
        yield return MoveObject(originalLocalPosition);
        isMoving = false;
    }

    IEnumerator MoveObject(Vector3 target)
    {
        Vector3 startingPosition = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < lerpSpeed)
        {
            transform.localPosition = Vector3.Lerp(startingPosition, target, elapsedTime / lerpSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = target;
    }
}
