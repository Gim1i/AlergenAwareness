using System;
using System.Collections.Generic;
using UnityEngine;

public class Modal_Managment : MonoBehaviour
{
    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject[] modalSlots;
    [SerializeField] private modalInformation[] statesLookupArray;
    private List<(GameObject GO, Modal_Data script)> activeModals = new List<(GameObject GO, Modal_Data script)>();
    private int filledModalSlots = 0;

    private void Start()
    {
        Debug.Assert(modalPrefab != null, "Missing modal prefab");
        Debug.Assert(modalSlots.Length > 0, "Missing any modal slot");
    }

    public void CreateNewModal(modalInformation state)
    {
        Debug.Log("Modal: Creation");
        for (int i = 0; i < statesLookupArray.Length; i++) { //Look though the registered states
            if (statesLookupArray[i].compaireWithoutSprite(state)) { //Find the state that includes the sprite (state might not have one)
                GameObject newModal = Instantiate(modalPrefab, modalSlots[filledModalSlots].transform); //Make new modal on the highest empty spot
                newModal.GetComponent<Modal_Data>().Setup(statesLookupArray[i]); //Save details to modal
                activeModals.Add(new(newModal, newModal.GetComponent<Modal_Data>())); //Save to universal modal list
                filledModalSlots++; //Move highest empty slot down one
                return;
            }
        }
    }

    public bool DoesModalExist(modalInformation state)
    {
        Debug.Log("Modal: Exist check");
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].script.ModalStatus().compaireWithoutSprite(state)) {
                return true; //return true if exists
            }
        }
        return false; //return false if it can't be found
    }

    public void RemoveModal(modalInformation state)
    {
        Debug.Log("Modal: Removal");
        for (int i = 0; i < filledModalSlots; i++) { //Locate the modal requested to be removed
            if (activeModals[i].script.ModalStatus().compaireWithoutSprite(state)) {
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