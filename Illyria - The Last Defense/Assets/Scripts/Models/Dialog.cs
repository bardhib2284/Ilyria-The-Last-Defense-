using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Dialog : MonoBehaviour
{
    public static Dialog instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    public string action { get; private set; }
    public GameObject AlertDialogPrefab;
    public GameObject ActionDialogPrefab;

    public void CreateAlertDialog(string message, string buttonText)
    {
        GameObject actionDialog = Instantiate(AlertDialogPrefab, GameObject.FindWithTag("MainCanvas").transform);
        actionDialog.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
        actionDialog.transform.GetChild(0).transform.GetChild(1).GetChild(0).GetComponent<Text>().text = buttonText;
        actionDialog.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { Destroy(actionDialog); });
    }

    public void CreateAlertDialog(string message)
    {
        GameObject actionDialog = Instantiate(AlertDialogPrefab, GameObject.FindWithTag("MainCanvas").transform);
        actionDialog.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = message;
        actionDialog.transform.GetChild(0).transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Ok";
        actionDialog.transform.GetChild(0).transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { Destroy(actionDialog); });
    }

    public bool CreateActionDialog(string message, string firstButtonText, string secondButtonText)
    {
        GameObject actionDialog = Instantiate(ActionDialogPrefab, GameObject.FindWithTag("MainCanvas").transform);
        actionDialog.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = message;
        actionDialog.transform.GetChild(0).transform.GetChild(1).GetChild(0).GetComponent<Text>().text = firstButtonText;
        actionDialog.transform.GetChild(0).transform.GetChild(2).GetChild(0).GetComponent<Text>().text = secondButtonText;
        Button[] buttons = actionDialog.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(delegate { action = i.ToString(); Destroy(actionDialog);});
        }
        StartCoroutine(WaitForAction());
        return false;
    }

    public IEnumerator WaitForAction()
    {
        Debug.Log("Waiting For Action...");
        yield return action;
        yield return new WaitUntil(() => action == "1" || action == "2");
    }
}