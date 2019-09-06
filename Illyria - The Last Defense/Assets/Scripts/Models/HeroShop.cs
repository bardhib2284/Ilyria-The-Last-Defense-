﻿using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class HeroShop : Shop 
{
    private ConsumableManager cm;
    public int GoldHeroValue = 5000;
    public int GemHeroValue = 250;
    public GameObject Hero_Summoning_Panel_UI;
    [HideInInspector]
    public GameObject Hero_Summoning_Panel_UI_Active_Template;
    public bool IsBusy
    {
        get
        {
            if (Hero_Summoning_Panel_UI_Active_Template != null)
                return true;
            else 
                return false;
        }

    }

    Consumable goldConsumable => ConsumableInventory.instance.GetConsumableByName("gold");
    Consumable gemConsumable => ConsumableInventory.instance.GetConsumableByName("gem");
    private void Awake()
    {
        cm = FindObjectOfType<ConsumableManager>();
    }

    private void Start()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate
        {
            Debug.Log("omg");
            
            UseConsumable(goldConsumable, GoldHeroValue);
        });
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate
        {
            Debug.Log("omg");
            UseConsumable(gemConsumable, GemHeroValue);
        });
    }

    public HeroShop()
    {
        base.ShopName = "HeroShop";
    }

    public override void UseConsumable<T>(T consumable, int value)
    {
        Debug.LogError("HERO SHOP STATE : " + IsBusy);
        if(!IsBusy)
        {
            Consumable c = consumable as Consumable;
            if (c == gemConsumable)
            {
                PayWithGem(c, value);
            }
            else if (c == goldConsumable)
            {
                PayWithGold(c, value);
            }
        }
    }

    // Explicit predicate delegate.
    private static bool FindHero(MonoBehaviour c, int stars)
    {
        if(c is Character)
        {
            if ((int)((Character)c).Stars == stars)
            {
                return true;
            }
            else
            {
                Dialog.instance.CreateAlertDialog("The Current Character " + ((Character)c).Name + " has " + ((Character)c).Stars + " and the required stars are " + stars, "Ok");
                return false;
            }
        }
        Dialog.instance.CreateAlertDialog("This monobehaviour is not character","Ok");
        return false;

    }

    public void ShowHeroUI()
    {
        this.gameObject.SetActive(true);
    }

    public void HideHeroUI()
    {
        this.gameObject.SetActive(false);
    }
    #region PAYING METHODS
    public void PayWithGold(Consumable consumable, int value)
    {
        Debug.Log("Player Requested To Buy In The Hero Shop with " + consumable.Name);
        bool payed = cm.UseConsumable(consumable, value);
        int starsByRandom = 0;
        if(payed)
        {
            try
            {
                Debug.Log("Played had enough " + consumable + " to buy the hero ");
                starsByRandom = UnityEngine.Random.Range(0, 100);
                if (starsByRandom >= 0 && starsByRandom <= 30)
                {
                    starsByRandom = 1;
                }
                else if (starsByRandom >= 31 && starsByRandom <= 60)
                {
                    starsByRandom = 2;
                }
                else if (starsByRandom >= 61 && starsByRandom <= 80)
                {
                    starsByRandom = 3;
                }
                else if (starsByRandom >= 81 && starsByRandom <= 95)
                {
                    starsByRandom = 4;
                }
                else if (starsByRandom >= 96 && starsByRandom <= 99)
                {
                    starsByRandom = 5;
                }
                Debug.Log("He got a " + starsByRandom + " star hero, now randoming the hero");
                List<MonoBehaviour> charactersByStars = shopContents.FindAll(element => FindHero(element,starsByRandom));
                int characterByRandom = UnityEngine.Random.Range(0, charactersByStars.Count);
                Character newCharacter = (Character)charactersByStars[characterByRandom];
                Hero_Summoning_Panel_UI_Active_Template = Instantiate(Hero_Summoning_Panel_UI, this.transform);
                Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(1).GetComponent<Image>().sprite = newCharacter.CharacterIcon;
                Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("FactionIcons/" + newCharacter.Faction.ToString());
                Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(2).GetComponent<Text>().text = newCharacter.Name;

                for (int i = 0; i < (int)newCharacter.Stars; i++)
                {
                    Instantiate(Resources.Load<GameObject>("UI/Star"), Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(1).GetChild(0));
                }
                Button[] buttons = Hero_Summoning_Panel_UI_Active_Template.transform.GetComponentsInChildren<Button>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].onClick.AddListener(() => Destroy(Hero_Summoning_Panel_UI_Active_Template, 0.2f));
                }
                Debug.Log("The Won Character : " + newCharacter);
                FindObjectOfType<TeamManager>().AddANewCharacterToTheTeam(newCharacter);
            }catch(Exception e)
            {
                Dialog.instance.CreateAlertDialog(e.Message,"Ok");
                return;
            }

        }

    }

    public void PayWithGem(Consumable consumable, int value)
    {
        Debug.Log("Player Requested To Buy In The Hero Shop with " + consumable);
        bool payed = cm.UseConsumable(consumable, value);
        int starsByRandom = 0;
        if (payed)
        {
            Debug.Log("Played had enough " + consumable + " to buy the hero ");
            starsByRandom = UnityEngine.Random.Range(0, 100);
            if (starsByRandom >= 0 && starsByRandom <= 50)
            {
                starsByRandom = 3;
            }
            else if (starsByRandom >= 51 && starsByRandom <= 90)
            {
                starsByRandom = 4;
            }
            else if (starsByRandom >= 91 && starsByRandom <= 99)
            {
                starsByRandom = 5;
            }
            Debug.Log("He got a " + starsByRandom + " star hero, now randoming the hero");
            List<MonoBehaviour> charactersByStars = shopContents.FindAll(x => FindHero(x, starsByRandom));
            int characterByRandom = UnityEngine.Random.Range(0, charactersByStars.Count);
            Character newCharacter = (Character)charactersByStars[characterByRandom];
            Hero_Summoning_Panel_UI_Active_Template = Instantiate(Hero_Summoning_Panel_UI, this.transform);
            Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(1).GetComponent<Image>().sprite = newCharacter.CharacterIcon;
            Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("FactionIcons/" + newCharacter.Faction.ToString());
            Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(2).GetComponent<Text>().text = newCharacter.Name;

            for (int i = 0; i < (int)newCharacter.Stars; i++)
            {
                Instantiate(Resources.Load<GameObject>("UI/Star"), Hero_Summoning_Panel_UI_Active_Template.transform.GetChild(1).GetChild(0));
            }
            Button[] buttons = Hero_Summoning_Panel_UI_Active_Template.transform.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.AddListener(() => Destroy(Hero_Summoning_Panel_UI_Active_Template, 0.2f));
            }
            Debug.Log("The Won Character : " + newCharacter);
            FindObjectOfType<TeamManager>().AddANewCharacterToTheTeam(newCharacter);
            int currentID = PlayerPrefs.GetInt("CharacterID");
            PlayerPrefs.SetInt("CharacterID", currentID + 1);
        }
        else
        {
            var result = FindObjectOfType<Dialog>().CreateActionDialog("You Dont Have Enough Gems To Summon A New Heroic Hero, Do You Wanna Buy Gems?", "No","Yes");
            Debug.Log(result + " RESULT ");
        }
    }
    #endregion
}
