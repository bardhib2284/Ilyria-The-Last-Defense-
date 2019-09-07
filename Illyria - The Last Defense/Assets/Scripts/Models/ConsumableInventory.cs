using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Globalization;

public class ConsumableInventory : Inventory
{
    public static ConsumableInventory instance;
    public const string CONSUMABLE_UI_PREFAB_NAME = "Consumable_UI_Prefab";

    //UI
    public Text GoldText;
    public Text GemText;

    //PROPERTIES
    public int GoldValue
    {
        set 
        {
            if(GoldText != null)
            {
                if (value > 999999999 || value < -999999999)
                {
                    GoldText.text = value.ToString("0,,,.###B", CultureInfo.InvariantCulture);
                }
                else if (value > 999999 || value < -999999)
                {
                    GoldText.text = value.ToString("0,,.##M", CultureInfo.InvariantCulture);
                }
                else if (value > 999 || value < -999)
                {
                    GoldText.text = value.ToString("0,.##K", CultureInfo.InvariantCulture);
                }
                else
                {
                    GoldText.text = value.ToString();
                }
            }
        }
    }
    public int GemValue
    {
        set
        {
            if(GemText != null)
            {
                if (value > 999999999 || value < -999999999)
                {
                    GemText.text = value.ToString("0,,,.###B", CultureInfo.InvariantCulture);
                }
                else if (value > 999999 || value < -999999)
                {
                    GemText.text = value.ToString("0,,.##M", CultureInfo.InvariantCulture);
                }
                else if (value > 999 || value < -999)
                {
                    GemText.text = value.ToString("0,.##K", CultureInfo.InvariantCulture);
                }
                else
                {
                    GemText.text = value.ToString();
                }
            }
        }
    }

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
    public List<Consumable> AllConsumables;

    private void Start()
    {
        StartCoroutine("GetResourcesFromDatabase");
        //this.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(GameManager.instance.Active_Mode.Rewards.ReceiveReward);

    }
    public Consumable GetConsumableByName(string name)
    {
        string uppercaseName = name.ToUpper();
        Consumable c = AllConsumables.Find(x => x.Name == uppercaseName);
        if(c == null)
        {
            Dialog.instance.CreateAlertDialog("Required Consumable Not Found", "Ok");
            return null;
        }
        return c;
    }
    public void UpdateResource(Consumable consumable)
    {
        Debug.Log(consumable.Name + " updated ");
        if(consumable.Name == "GOLD" || consumable.Name == "GEM")
        {
            if (consumable.Name == "GOLD")
            {
                GoldValue = consumable.Value;
            }
            else
                GemValue = consumable.Value;
        }
        Consumable c = AllConsumables.Find(x => x.Name == consumable.Name);
        StartCoroutine("UpdateResourceToDatabase", c);
    }
    public void AddConsumable(Consumable consumable)
    {
        Consumable c = AllConsumables.Find(x => x.Name == consumable.Name);
        if(c != null)
        {
            c.Value += consumable.Value;
            UpdateResource(c);
        }
        else 
        { 
            AllConsumables.Add(consumable);
            StartCoroutine("AddResourceToDatabase",c); 
        }
    }

    private void OnEnable()
    {
        if(AllConsumables != null)
        {
            foreach (var consumable in AllConsumables)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("UI/" + CONSUMABLE_UI_PREFAB_NAME), this.transform.GetChild(0).GetChild(0).GetChild(0));
                temp.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/" + consumable.Icon);
                temp.transform.GetChild(1).GetComponent<Text>().text = consumable.Name;
                temp.transform.GetChild(2).GetComponent<Text>().text = consumable.Value.ToString();
            }
        }
    }
    private IEnumerator UpdateResourceToDatabase(Consumable c)
    {
        DBTestBehaviourScript.instance.UpdateConsumable(c);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator AddResourceToDatabase(Consumable c)
    {
        DBTestBehaviourScript.instance.AddConsumable(c);
        yield return new WaitForSeconds(0.5f);
    }
    private IEnumerator GetResourcesFromDatabase()
    {
        yield return new WaitForSeconds(1f);
        List<Consumable> consumables = DBTestBehaviourScript.instance.ReadConsumables();
        Dialog.instance.CreateAlertDialog(consumables.Count.ToString(), " oK ");
        GoldValue = consumables.Find(x => x.Name == "GOLD").Value;
        GemValue = consumables.Find(x => x.Name == "GEM").Value;
        yield return AllConsumables = consumables;
    }

    /*private void OnDisable()
    {
        Transform contentTransform = this.transform.GetChild(0).GetChild(0).GetChild(0);
        for (int i = 0; i < contentTransform.childCount; i++)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }*/
}