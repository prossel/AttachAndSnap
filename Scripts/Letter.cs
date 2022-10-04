using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Letter : MonoBehaviour
{
    public UnityEvent OnCollisionEnterWithLetter;
    public UnityEvent OnCollisionEnterWithOtherObject;

    Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody?.GetComponent<Letter>() != null)
        {
            Debug.Log("Letter collision with Letter (" + collision.collider.name + ")");
            OnCollisionEnterWithLetter.Invoke();
        }
        else
        {
            Debug.Log("Letter collision with other");
            OnCollisionEnterWithOtherObject.Invoke();
        }
    }
}
