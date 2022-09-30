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
    protected Connector snappableLeft;
    protected Connector snappableRight;
    protected bool selected;
    protected bool snappedLeft;
    protected bool snappedRight;
    protected bool connectedLeft;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void SetSnappable(Connector connector, bool snappable)
    //{
    //    if (connector.side == Connector.Side.Left)
    //    {
    //        this.snappableRight = snappable ? connector : null;
    //    } else
    //    {
    //        this.snappableLeft = snappable ? connector : null;
    //    }


    //    if (!selected)
    //    {
    //        Snap();
    //    }

    //}

    void OnSelectLetter()
    {
        Debug.Log("OnSelectLetter", this);
        selected = true;
    }

    void OnUnselectLetter()
    {
        Debug.Log("OnUnselectLetter", this);
        selected = false;
        Snap();
    }

    void Snap()
    {
        // Try to snap left connector to the right connector of snappableLeft
        if (!snappedLeft && snappableLeft != null)
        {
            
            



            
            //connectorLeft.SnapTo(snappableLeft);

            //Transform trLetter = transform.parent;
            //Transform trOtherLetter = snappableLeft.letterConnectors.transform.parent;


            //// Move visual root to target

            ////Transform root = trLetter.transform.Find("Visuals/Root");
            ////Transform targetVisuals = snappableLeft.letterConnectors.transform.parent.Find("Visuals");
            ////Vector3 snapDistance = snappableLeft.transform.position - connectorLeft.transform.position;
            ////root.SetParent(targetVisuals);
            ////root.position += snapDistance;
            ////root.position = targetVisuals;
            ////root.localRotation = Quaternion.identity;

            //Rigidbody rbOther = trOtherLetter.GetComponent<Rigidbody>();
            //Rigidbody rbLetter = trLetter.GetComponent<Rigidbody>();

            ////rbLetter.rotation = rbOther.rotation;
            ////Vector3 snapDistance = snappableLeft.transform.position - connectorLeft.transform.position;
            //////rbLetter.position += snapDistance;
            ////rbLetter.position += snapDistance;


            ////// Add a joint
            ////FixedJoint joint = trLetter.gameObject.AddComponent<FixedJoint>();
            ////joint.connectedBody = trOtherLetter.GetComponent<Rigidbody>();

            //ConfigurableJoint joint = trLetter.gameObject.AddComponent<ConfigurableJoint>();
            //joint.autoConfigureConnectedAnchor = false;
            //joint.connectedBody = trOtherLetter.GetComponent<Rigidbody>();
            //joint.anchor = connectorLeft.transform.localPosition;
            //joint.connectedAnchor = snappableLeft.transform.localPosition;
            
            //joint.xMotion = ConfigurableJointMotion.Locked;
            //joint.yMotion = ConfigurableJointMotion.Locked;
            //joint.zMotion = ConfigurableJointMotion.Locked;
            //joint.angularXMotion = ConfigurableJointMotion.Locked;
            //joint.angularYMotion = ConfigurableJointMotion.Locked;
            //joint.angularZMotion = ConfigurableJointMotion.Locked;

            //joint.breakForce = 1000;

            //snappedLeft = true;
        }

        //if (!connected)
        //{
        //    Connect();

        //}
    }

    void Connect()
    {

        //connected = true;

        nRemainingConnections += snappableLeft != null ? -1 : 1;
        if (nRemainingConnections == 0)
        {
            // TEMP Reload scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("AttachAndSnap");
        }

    }
}
