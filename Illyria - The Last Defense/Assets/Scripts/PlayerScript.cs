using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    public bool IsBusy;
    public const string HERO_SHOP_NAME = "HERO_SHOP_OBJECT";
    Camera cam;
    public bool SelectTarget;
    //TODO:FIND A BETTER NAME
    public Character askedFrom;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        #region BASIC INTERACTION WITH UI
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    Debug.LogError("Hitting INTERACTABLE : " + hit.transform.name);
                    interactable.Interact();
                }
            }
        }
        #endregion
        if (SelectTarget)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    if(objectHit.name == HERO_SHOP_NAME)
                    {
                        FindObjectOfType<ShopManager>().TurnOnTheHeroSummoning();
                    }
                    if (objectHit.GetComponent<Character>())
                    {
                        askedFrom.SelectedEnemy = objectHit.GetComponent<Character>();
                    }
                }
            }
        }
    }

    public void SelectTargetForCharacter(Character c,bool selectTarget = true)
    {
        askedFrom = c;
        SelectTarget = selectTarget;
    }
}
