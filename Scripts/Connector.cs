using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Connector : MonoBehaviour
{
    public AudioClip snapClip;
    public AudioClip unsnapClip;


    public UnityEvent<Connector> onConnected;
    public UnityEvent<Connector> onDisconnected;

    protected Connector snappableTo;
    
    protected bool connected = false;
    protected bool letterSelected = false;

    protected AudioSource audioSource;
    protected PointableUnityEventWrapper pointableEvents;

    public enum Side
    {
        Left, Right
    }

    public Side side;

    internal LetterConnectors letterConnectors;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        pointableEvents = GetComponentInParent<PointableUnityEventWrapper>();
        pointableEvents.WhenSelect.AddListener(OnSelectLetter);
        pointableEvents.WhenUnselect.AddListener(OnUnselectLetter);
    }

    private void OnDisable()
    {
        pointableEvents.WhenSelect.RemoveListener(OnSelectLetter);
        pointableEvents.WhenUnselect.RemoveListener(OnUnselectLetter);
    }

    // Start is called before the first frame update
    void Start()
    {
        letterConnectors = GetComponentInParent<LetterConnectors>();
        onConnected.AddListener(letterConnectors.OnConnected);
        onDisconnected.AddListener(letterConnectors.OnDisconnected);

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

        //Debug.Log("Snappable frame " + Time.frameCount, this);

        // Try to connect
        Connect();

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

        //Debug.Log("Unsnappable frame " + Time.frameCount, this);

        // Unsnap
        Destroy(joint);

        // Remove link between snappable connectors
        snappableTo.snappableTo = null;
        snappableTo = null;

        Disconnect();
        conOther.Disconnect();

        return true;
    }

    void OnSelectLetter()
    {
        //Debug.Log("OnSelectLetter frame " + Time.frameCount, this);
        letterSelected = true;

        // If connected and both letters selected, need to disconnect
        if (connected && snappableTo.letterSelected)
        {
            Disconnect();
        }
    }

    void OnUnselectLetter()
    {
        //Debug.Log("OnUnselectLetter frame " + Time.frameCount, this);
        letterSelected = false;
        Connect();
    }

    bool Connect()
    {
        if (!connected && CanConnect())
        {
            Debug.Log(letterConnectors.letter + " " + side + " connected");
            connected = true;
            audioSource?.PlayOneShot(snapClip);
            
            onConnected.Invoke(this);

            snappableTo?.Connect();

            return true;
        }
        return false;
    }

    bool Disconnect()
    {
        if (connected && !CanConnect())
        {
            Debug.Log(letterConnectors.letter + " " + side + " disconnected");
            connected = false;
            audioSource?.PlayOneShot(unsnapClip);

            onDisconnected.Invoke(this);

            snappableTo?.Disconnect();

            return true;
        }
        return false;
    }

    bool CanConnect()
    {
        return snappableTo != null && (!letterSelected || !snappableTo.letterSelected);
    }

}