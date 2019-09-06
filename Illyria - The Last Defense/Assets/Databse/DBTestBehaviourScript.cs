using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Collections.ObjectModel;
using System;

public class DBTestBehaviourScript : MonoBehaviour
{
    public static DBTestBehaviourScript instance;

    CharacterDB mCharacterDB;
    InventoryDB mInventoryDB;
    private void Start()
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
        mCharacterDB = new CharacterDB();
        mInventoryDB = new InventoryDB();
    }

    #region ADD FUNCTIONS
    public void AddCharacter(Character c)
    {
        mCharacterDB.AddCharacter(c);
    }
    public void UpdateCharacter(Character character)
    {
        Debug.Log("Updating The :" + character.Name);
        if(character != null)
            mCharacterDB.UpdateCharacter(character);
    }

    public void UpdateCharacterExperience(Character character)
    {
        Debug.Log("Updating The :" + character.Name + " experience");
        if (character != null)
            mCharacterDB.UpdateCharacterExperience(character);
    }

    public void AddConsumable(Consumable c)
    {
        mInventoryDB.AddConsumable(c);
    }
    public void UpdateConsumable(Consumable c)
    {
        mInventoryDB.UpdateResource(c);
    }
    #endregion


    public List<Character> ReadCharacters()
    {
        try
        {
            IDataReader data = mCharacterDB.GetCharacters();
            List<Character> characters = new List<Character>();
            while (data.Read())
            {
                Debug.Log(data[0].ToString() + data[1] + data[2]);
                GameObject characterObject = Instantiate(Resources.Load<GameObject>("HeroPrefabs/" + (string)data[1]), transform.position, Quaternion.identity);
                characterObject.SetActive(false);
                Character character = characterObject.GetComponent<Character>();
                character.ID = Convert.ToInt32(data[0]);
                character.Name = (string)data[1];
                character.Level_Current = Convert.ToInt32(data[2]);
                character.Stars = (Character.CharacterStars)Convert.ToInt32(data[3]);
                character.Experience_Current = Convert.ToInt32(data[4]);
                ObservableCollection<Item> items = new ObservableCollection<Item>();
                if (!string.IsNullOrEmpty((string)data[5]))
                {
                    items.Add(Resources.Load<Item>((string)data[5]));
                    if (!string.IsNullOrEmpty((string)data[6]))
                    {
                        items.Add(Resources.Load<Item>((string)data[6]));
                        if (!string.IsNullOrEmpty((string)data[7]))
                        {
                            items.Add(Resources.Load<Item>((string)data[7]));
                            if (!string.IsNullOrEmpty((string)data[8]))
                            {
                                items.Add(Resources.Load<Item>((string)data[8]));
                            }
                        }
                    }
                }
                character.Items = items;
                characters.Add(character);
            }
            return characters;
        }
        catch (Exception e)
        {
            FindObjectOfType<Dialog>().CreateAlertDialog(e.Message.ToString(), "Ok");
            Debug.Log(e.Message);
            return null;
        }
    }

    public List<Consumable> ReadConsumables()
    {
        try
        {
            IDataReader data = mInventoryDB.GetResources();
            List<Consumable> consumables = new List<Consumable>();
            while(data.Read())
            {
                int id;
                bool idResult = int.TryParse(data[0].ToString(), out id);
                int value;
                bool valueResult = int.TryParse(data[2].ToString(), out value);
                Consumable consumable = new Consumable(id, (string)data[1], value, (string)data[3]);
                consumables.Add(consumable);
            }
            return consumables;
        }
        catch(Exception e)
        {
            FindObjectOfType<Dialog>().CreateAlertDialog(e.StackTrace,"Ok");
            Debug.Log(e.Message);
            return null;
        }
    }
}