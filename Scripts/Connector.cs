using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public enum Side
    {
        left, right
    }

    public Side side;

    internal LetterConnectors letterConnectors;

    // Start is called before the first frame update
    void Start()
    {
        letterConnectors = GetComponentInParent<LetterConnectors>();
    }

}
