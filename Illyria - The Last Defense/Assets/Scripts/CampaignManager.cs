using UnityEngine;
using System.Collections;

public class CampaignManager : MonoBehaviour
{
    public GameObject CampaignObject;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CampaignClick()
    {
        CampaignObject.SetActive(!CampaignObject.activeInHierarchy);
    }
}
