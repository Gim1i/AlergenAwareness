using System;
using System.Collections.Generic;
using UnityEngine;

public class Modal_Managment : MonoBehaviour
{
    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject[] modalSlots;
    [SerializeField] private Sprite[] modalStateSprites;
    private List<(modalState state, GameObject GO)> activeModals = new List<(modalState state, GameObject GO)>();
    private Sprite[] sortedModalStateSprites;
    private int filledModalSlots = 0;

    private void Start()
    {
        Debug.Assert(modalPrefab != null, "Missing modal prefab");
        Debug.Assert(modalSlots.Length > 0, "Missing any modal slot");

        sortedModalStateSprites = new Sprite[modalStateSprites.Length];
        for (int i = 0; i < modalStateSprites.Length; i++) { //sort all modal state sprites into ENUM order for later use
            switch (modalStateSprites[i].name.ToLower()) {
                case "happy":
                    Debug.Assert(sortedModalStateSprites[0] == null, "Duplicate sprite names"); //Error if sprite is already assigned
                    sortedModalStateSprites[0] = modalStateSprites[i]; //Assign sprite into the same location as its enum value
                    break;
                case "sad":
                    Debug.Assert(sortedModalStateSprites[1] == null, "Duplicate sprite names");
                    sortedModalStateSprites[1] = modalStateSprites[i];
                    break;
                default:
                    Debug.Assert(false, "State sprite ENUM equivalent not found");
                    break;
            }
        }
    }

    public void CreateNewModal(modalState state)
    {
        Debug.Log("Modal: Creation");
        GameObject newModal = Instantiate(modalPrefab, modalSlots[filledModalSlots].transform); //Make new modal on that slot
        newModal.GetComponent<Modal_Data>().Setup(sortedModalStateSprites[(int)state], state);
        activeModals.Add(new (state, newModal)); //Save to universal modal list
        filledModalSlots++; //Move highest empty slot down one
    }

    public bool DoesModalExist(modalState state)
    {
        Debug.Log("Modal: Exist check");
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].state == state) {
                return true; //return true if exists
            }
        }
        return false; //return false if it can't be found
    }

    public void RemoveModal(modalState state)
    {
        Debug.Log("Modal: Removal");
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].state == state) {
                activeModals[i].GO.GetComponent<Modal_Data>().DeleteSelf();
                activeModals.RemoveAt(i);
                filledModalSlots--;

                if (i != filledModalSlots) { //Moving Modals up check
                    for (int h = i; h < filledModalSlots; h++) { //Move modals up
                        activeModals[h].GO.transform.parent = modalSlots[h].transform; //Change parent of modal
                        activeModals[h].GO.GetComponent<Modal_Data>().ResetMovement();
                    }
                }
                break;
            }
        }
    }
}