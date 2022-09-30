using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterConnectors : MonoBehaviour
{
    public string letter;
    public string nextLetter;

    protected static int nRemainingConnections = 0;

    internal Connector connectorLeft;
    internal Connector connectorRight;
    protected bool selected;
    protected bool connectedRight;
    protected PointableUnityEventWrapper pointableEvents;

    // Start is called before the first frame update
    void Start()
    {
        if (nextLetter != "") nRemainingConnections++;
    }

    private void OnEnable()
    {
        pointableEvents = GetComponentInParent<PointableUnityEventWrapper>();
        pointableEvents.WhenSelect.AddListener(OnSelectLetter);
        pointableEvents.WhenUnselect.AddListener(OnUnselectLetter);
    }

    private void OnDisable()
    {
        pointableEvents.WhenSelect.RemoveListener(OnSelectLetter);
        pointableEvents.WhenUnselect.RemoveListener(OnUnselectLetter);
    }

 
    void OnSelectLetter()
    {
        //Debug.Log("OnSelectLetter frame " + Time.frameCount, this);
        selected = true;
        
        // If connected and both letters selected, need to disconnect
        if (connectedRight && connectorRight.snappableTo.letterConnectors.selected)
        {
            Disconnect();
        }
    }

    void OnUnselectLetter()
    {
        //Debug.Log("OnUnselectLetter frame " + Time.frameCount, this);
        selected = false;
        if (connectorRight.snappableTo != null) { 
            Connect();
        }
        if (connectorLeft.snappableTo != null)
        {
            connectorLeft.snappableTo.letterConnectors.Connect();
        }
    }

    public void OnSnappable(bool snappable)
    {
        if (snappable)
        {
            // May already connect if one of the letter is not selected
            if (!selected || !connectorRight.snappableTo.letterConnectors.selected) {
                Connect();
            }
        }
        else
        {
            Disconnect();
        }
    }
    

    void Connect()
    {
        if (!connectedRight) {
            connectedRight = true;

            nRemainingConnections--;

            connectorRight.OnConnected();

            if (nRemainingConnections == 0)
            {
                // TEMP Reload scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("AttachAndSnap");
            }
        }

    }

    void Disconnect ()
    {
        if (connectedRight)
        {
            connectedRight = false;
            nRemainingConnections++;
            
            connectorRight.OnDisconnected();

        }
    }
}
