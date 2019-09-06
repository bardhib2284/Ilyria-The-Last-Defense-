using UnityEngine;
using System.Collections;

public class CampaignObject : MonoBehaviour,IInteractable
{
    public void Interact()
    {
        FindObjectOfType<CampaignManager>().CampaignClick();
    }
}
