using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public AudioClip snapClip;
    public AudioClip unsnapClip;

    public AudioSource audioSource;

    internal Connector snappableTo;

    public enum Side
    {
        Left, Right
    }

    public Side side;

    internal LetterConnectors letterConnectors;

    // Start is called before the first frame update
    void Start()
    {
        letterConnectors = GetComponentInParent<LetterConnectors>();

        if (side == Side.Left)
        {
            letterConnectors.connectorLeft = this;
        }
        else
        {
            letterConnectors.connectorRight = this;
        }

        audioSource = GetComponentInChildren<AudioSource>();
    }

    public bool SnappableTo(Connector conTarget)
    {
        if (side != Side.Right) return false;

        Transform trLetter = letterConnectors.transform.parent;

        // do not add more than one 
        if (trLetter.gameObject.GetComponent<ConfigurableJoint>() != null) return false;

        Transform trOtherLetter = conTarget.letterConnectors.transform.parent;

        // Add a joint to the body which has the right connector
        ConfigurableJoint joint = trLetter.gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = transform.localPosition;
        joint.connectedAnchor = conTarget.transform.localPosition;
        
        // temporaryly orient both object to the same rotation, so the target orientation is automatically calculated, otherwise its complicated
        Quaternion bkp = trOtherLetter.rotation;
        trOtherLetter.rotation = trLetter.rotation;
        joint.connectedBody = trOtherLetter.GetComponent<Rigidbody>();
        trOtherLetter.rotation = bkp;
        //joint.targetRotation = Quaternion.identity * (conTarget.transform.rotation * Quaternion.Inverse(transform.rotation));


        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        //joint.breakForce = 1000;

        // Establish link between snappable connectors
        snappableTo = conTarget;
        snappableTo.snappableTo = this;

        Debug.Log("Snappable frame " + Time.frameCount, this);

        // Notify LC
        letterConnectors.OnSnappable(true);

        return true;
    }

    public bool UnSnappableTo(Connector conOther)
    {
        if (side != Side.Right) return false;

        Transform trLetter = letterConnectors.transform.parent;
        Rigidbody rbLetter = trLetter.GetComponent<Rigidbody>();
        Rigidbody rbOther = conOther.letterConnectors.transform.parent.GetComponent<Rigidbody>();

        // Must already have a joint
        ConfigurableJoint joint = trLetter.gameObject.GetComponent<ConfigurableJoint>();
        if (joint == null) return false;

        // Verify other connector 's rigidbody is the same as currently connected
        if (joint.connectedBody != rbOther) return false;

        Debug.Log("Unsnappable frame " + Time.frameCount, this);

        // Unsnap
        Destroy(joint);

        // Notify LC
        letterConnectors.OnSnappable(false);

        // Remove link between snappable connectors
        snappableTo.snappableTo = null;
        snappableTo = null;

        return true;
    }

    public void OnConnected()
    {
        audioSource.PlayOneShot(snapClip);
    }

    public void OnDisconnected()
    {
        audioSource.PlayOneShot(unsnapClip);
    }


}