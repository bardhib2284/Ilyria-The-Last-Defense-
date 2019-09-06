using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject mock;
    public bool DestroyMock;
    public GameObject Parent;
    public bool AmIMock;
    public DropZone[] DropZone;
    public Character character;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(mock != null)
        {
            mock.transform.SetParent(this.transform.root);
            return;
        }
        Initialize();
    }
    public void Initialize()
    {
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        mock = Instantiate(this.gameObject, this.transform.root);
        var dragable = mock.GetComponent<Dragable>();
        dragable.enabled = false;
        dragable.AmIMock = true;
        dragable.Parent = this.gameObject;
        this.GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    private void OnEnable()
    {
        this.GetComponentInChildren<Button>().onClick.AddListener(delegate {
            Clicked();
        });
    }

    public void OnDrag(PointerEventData eventData)
    {
        mock.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (mock.transform.parent.GetComponent<DropZone>() == null)
        {
            Reset();
        }
        if (DestroyMock == true)
        {
            if(AmIMock)
            {
                Reset();
            }
            else
            {
                Destroy(mock);
            }
        }
    }

    public void Reset()
    {
        if (AmIMock)
        {
            Parent.GetComponent<CanvasGroup>().alpha = 1f;
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(mock);
            GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    private void Clicked()
    {
        Debug.Log("Mouse Up Method");
        if(AmIMock)
        {
            Reset();
        }
        else
        {
            if(mock != null)
            {
                Reset();
                return;
            }
            DropZone = FindObjectsOfType<DropZone>().OrderBy(m => m.transform.GetSiblingIndex()).ToArray(); ;
            for (int i = 0; i < DropZone.Length; i++)
            {
                Debug.Log(" Child " + i + " has " + DropZone[i].transform.childCount);
                if(DropZone[i].transform.childCount > 0)
                {
                    continue;
                }
                else
                {
                    Initialize();
                    Dragable d = mock.GetComponent<Dragable>();
                    d.transform.SetParent(DropZone[i].transform);
                    //d.DestroyMock = false;
                    //d.mock.transform.SetParent(this.transform);
                    break;
                }
            }
        }
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
