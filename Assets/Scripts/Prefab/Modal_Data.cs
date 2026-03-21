using UnityEngine;

public class Modal_Data : MonoBehaviour
{
    public modalState currentStatus { get; private set; } //Variable can be seen by any script but only altered by this one
    [SerializeField] private float movementStopLeway = 0.1f;
    [SerializeField] private float movementTime = 3f;
    [SerializeField] private GameObject statusSprite;
    private modalState status;
    private Vector3? v3MovingNeeded = null; //Using null as movement completed
    private bool movingNeeded = true; //Re-enables movement
    private Vector3 localEndPos = new Vector3(0, 0, 0);

    public void Setup(Sprite statusSprite, modalState status) //Inital setup of modal (on instantiation)
    {
        this.statusSprite.GetComponent<SpriteRenderer>().sprite = statusSprite;
        this.status = status;
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

    public modalState ModalStatus() { return status; }
    public void ResetMovement() { //Reset movement settings
        movingNeeded = true;
        v3MovingNeeded = null; //Force movement code to re-calculate destination
    }
}
