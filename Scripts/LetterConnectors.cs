using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterConnectors : MonoBehaviour
{
    public string letter;
    public string nextLetter;

    protected static int nRemainingConnections = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (nextLetter != "") nRemainingConnections++;
    }

    public void OnConnected(Connector connector)
    {
        if (connector.side == Connector.Side.Right)
        {
            nRemainingConnections--;

            if (nRemainingConnections == 0)
            {
                // TEMP Reload scene
                //UnityEngine.SceneManagement.SceneManager.LoadScene("AttachAndSnap");

                GetComponentInParent<LettersManager>().OnWordComplete.Invoke();
            }
        }
    }

    public void OnDisconnected(Connector connector)
    {
        if (connector.side == Connector.Side.Right)
        {
            nRemainingConnections++;
            if (nRemainingConnections != 0)
            {
                GetComponentInParent<LettersManager>().OnWordIncomplete.Invoke();
            }
        }
    }
}
