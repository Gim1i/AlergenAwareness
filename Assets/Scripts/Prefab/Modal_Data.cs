using UnityEngine;

public class Modal_Data : MonoBehaviour
{
    [SerializeField] private float movementStopLeway = 0.1f;
    [SerializeField] private float movementTime = 3f;
    [SerializeField] private GameObject statusSprite;
    private modalInformation state;
    private Vector3? v3MovingNeeded = null; //Using null as movement completed
    private bool movingNeeded = true; //Re-enables movement
    private Vector3 localEndPos = new Vector3(0, 0, 0);

    public void Setup(modalInformation status) //Inital setup of modal (on instantiation)
    {
        this.statusSprite.GetComponent<SpriteRenderer>().sprite = status.getSprite();
        this.state = status;
    }

    private void Update() //Animated movment
    {
        if (movingNeeded) {
            if (v3MovingNeeded == null) { //Calculate the moves needed to get to endPos in movementTime seconds
                v3MovingNeeded = localEndPos - transform.localPosition;
            }
            transform.Translate((Vector3)v3MovingNeeded * Time.deltaTime/movementTime);

            if ((transform.localPosition.x < movementStopLeway && transform.localPosition.y < movementStopLeway && transform.localPosition.y > -movementStopLeway)) {
                transform.localPosition = localEndPos; //Set location to the exact end spot (when close enough) to avoid minor positioning errors
                v3MovingNeeded = null;
                movingNeeded = false;
            }
        }
    }

    public void DeleteSelf() //Self deletion alongside any associated animations
    {
        Destroy(gameObject);
    }

    public modalInformation ModalStatus() { return state; }
    public void ResetMovement() { //Reset movement settings
        movingNeeded = true;
        v3MovingNeeded = null; //Force movement code to re-calculate destination
    }
}
