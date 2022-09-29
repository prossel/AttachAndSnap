using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public enum ZoneType
    {
        Near, Snap
    }
    public ZoneType zoneType;

    Connector connector;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        connector = GetComponentInParent<Connector>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter on " + name + " with other " + other.name);

        if (IsMatchingZone(other))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            if (zoneType == ZoneType.Snap && connector.side == Connector.Side.right)
            {
                connector.letterConnectors.OnConnected(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsMatchingZone(other))
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            if (zoneType == ZoneType.Snap && connector.side == Connector.Side.right)
            {
                connector.letterConnectors.OnConnected(false);
            }
        }
    }

    private bool IsMatchingZone(Collider other)
    {
        // check other is also a near zone
        Zone otherZone = other.GetComponent<Zone>();
        if (otherZone == null) return false;

        // Check same type
        if (zoneType != otherZone.zoneType) return false;

        // check if side and letters match
        //Debug.Log("OnTriggerEnter on " + letterCon.letter + " with other " + otherZone.letterCon.letter);
        LetterConnectors lc = connector.letterConnectors;
        LetterConnectors lcOther = otherZone.connector.letterConnectors;
        return
            connector.side == Connector.Side.right && otherZone.connector.side == Connector.Side.left && lc.nextLetter == lcOther.letter ||
            connector.side == Connector.Side.left && otherZone.connector.side == Connector.Side.right && lc.letter == lcOther.nextLetter;
    }

}
