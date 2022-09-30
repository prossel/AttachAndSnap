using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public AudioClip snapClip;
    public AudioClip unsnapClip;

    public AudioSource audioSource;

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

    public bool SnapTo(Connector conTarget)
    {
        if (side != Side.Right) return false;

        Transform trLetter = letterConnectors.transform.parent;

        // do not add more than one 
        if (trLetter.gameObject.GetComponent<ConfigurableJoint>() != null) return false;

        Transform trOtherLetter = conTarget.letterConnectors.transform.parent;

        // Add a joint to the body which has the right connector
        ConfigurableJoint joint = trLetter.gameObject.AddComponent<ConfigurableJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = trOtherLetter.GetComponent<Rigidbody>();
        joint.anchor = transform.localPosition;
        joint.connectedAnchor = conTarget.transform.localPosition;


        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        //joint.breakForce = 1000;

        audioSource.PlayOneShot(snapClip);

        Debug.Log("Snap");

        return true;
    }

    public bool UnSnapFrom(Connector conOther)
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

        // Unsnap
        Destroy(joint);
        
        audioSource.PlayOneShot(unsnapClip);

        Debug.Log("Unsnap");

        return true;
    }

}