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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConnected(bool connected = true)
    {
        nRemainingConnections += connected ? -1 : 1;

        if (nRemainingConnections == 0)
        {
            // TEMP Reload scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("AttachAndSnap");
        }
    }
}
