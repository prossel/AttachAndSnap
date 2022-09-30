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

        if (IsMatchingZone(other))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            if (zoneType == ZoneType.Snap && connector.side == Connector.Side.Right)
            {
        Debug.Log("OnTriggerEnter on " + name + " with other " + other.name);
                connector.SnapTo(other.GetComponent<Zone>().connector);
                //connector.letterConnectors.SetSnappable(other.GetComponent<Zone>().connector, true);

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

            if (zoneType == ZoneType.Snap && connector.side == Connector.Side.Right)
            {
        Debug.Log("OnTriggerExit on " + name + " with other " + other.name);
                connector.UnSnapFrom(other.GetComponent<Zone>().connector);

                //connector.letterConnectors.SetSnappable(other.GetComponent<Zone>().connector, false);
                //connector.SnapTo(null);
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
            connector.side == Connector.Side.Right && otherZone.connector.side == Connector.Side.Left && lc.nextLetter == lcOther.letter ||
            connector.side == Connector.Side.Left && otherZone.connector.side == Connector.Side.Right && lc.letter == lcOther.nextLetter;
    }

}