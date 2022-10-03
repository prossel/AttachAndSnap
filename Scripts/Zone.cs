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

    bool allowSound = true;
    bool inZone = false;

    Connector connector;
    AudioSource audioSource;

    private void OnEnable()
    {
        connector = GetComponentInParent<Connector>();
        audioSource = GetComponentInChildren<AudioSource>();

        connector.onConnected.AddListener(OnConnected);
        connector.onDisconnected.AddListener(OnDisconnected);
    }

    private void OnDisable()
    {
        connector.onConnected.RemoveListener(OnConnected);
        connector.onDisconnected.RemoveListener(OnDisconnected);
    }

    void AllowSound(bool allow)
    {
        allowSound = allow;
        CheckSound();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter " + name + " <> " + other.name + " frame " + Time.frameCount, this);

        if (IsMatchingZone(other))
        {
            inZone = true;
            CheckSound();

            if (zoneType == ZoneType.Snap && connector.side == Connector.Side.Right)
            {
                connector.SnappableTo(other.GetComponent<Zone>().connector);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("OnTriggerExit " + name + " <> " + other.name + " frame " + Time.frameCount, this);

        if (IsMatchingZone(other))
        {
            inZone = false;
            CheckSound();

            if (zoneType == ZoneType.Snap && connector.side == Connector.Side.Right)
            {
                connector.UnSnappableTo(other.GetComponent<Zone>().connector);
            }
        }
    }

    private void OnConnected(Connector connector)
    {
        AllowSound(false);
    }

    private void OnDisconnected(Connector connector)
    {
        AllowSound(true);
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

    void CheckSound()
    {
        if (audioSource != null)
        {
            if (inZone && allowSound)
            {
                audioSource.Play();
            } else
            {
                audioSource.Pause();
            }
        }
    }
}
