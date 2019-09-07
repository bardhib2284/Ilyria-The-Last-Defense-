using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public int index;
	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log("OnPointerEnter");
		if(eventData.pointerDrag == null)
			return;

		Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
		if(d != null) {
			//d.placeholderParent = this.transform;
		}
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log("OnPointerExit");
		if(eventData.pointerDrag == null)
			return;

        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
		//if(d != null && d.placeholderParent==this.transform) {
			//d.placeholderParent = d.parentToReturnTo;
		//}
	}
	
	public void OnDrop(PointerEventData eventData) {
		Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);
        if(this.transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Dragable>().Reset();
        }
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        d.DestroyMock = false;
        d.mock.transform.SetParent(this.transform);
	}
}