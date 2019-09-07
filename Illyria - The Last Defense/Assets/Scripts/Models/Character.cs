﻿using UnityEngine;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Character : MonoBehaviour
{
    //Character Information
    #region Character Information
    //Used For Character Disctinction
    public int ID;
    public string Name;
    public string Description;
    public Sprite CharacterIcon;
    //TODO: THIS IS NOT FINAL ( UPDATE REQUIRED? )
    public enum CharacterFaction { Chaos, Dark, Earth, Fire, Light, Water, Leaf }
    public enum CharacterRole { Warrior, Assassin, Healer, Mage, Hunter, Demon }
    public enum CharacterStars { One = 1, Two = 2, Three = 3, Four = 4, Five = 5 }
    public CharacterFaction Faction;
    public CharacterRole Role;
    public CharacterStars Stars;
    public List<Character> currentTeam;
    public List<Character> enemyTeam;
    public Sprite Skill_1_Icon;
    public Sprite Skill_2_Icon;
    public Sprite Skill_3_Icon;
    public Sprite Skill_4_Icon;
    //IS THE ATTACK TYPE GOING TO BE THE FACTION TYPE OR NOT ??
    //public enum CharacterAttackType { ..... }
    #endregion

    [Header("CHARACTER Properties")]
    //Character Properties
    #region Character Properties
    public Animator Animator;
    public int Health_Start;
    public int Health_Max;
    public int Health_Current;
    public int Mana_Start;
    public int Mana_Max;
    public int Mana_Current;

    public int Attack_Start;
    public int Attack_Max;
    public int Attack_Current;
    public int Armor_Start;
    public int Armor_Max;
    public int Armor_Current;
    public int Speed_Start;
    public int Speed_Max;
    public int Speed_Current;
    public int _level_Current;
    public int Critical_Percentage;
    public int Critical_Percentage_Max;
    public int Critical_Damage_Percentage;
    public int Critical_Damage_Percentage_Max;
    public int Damage_Reduction;
    public int Damage_Reduction_Max;
    public int Block_Percentage;
    public int Block_Percentage_Max;
    //TODO: CHANGE THE NAME OF THIS VARIABLE TO SOMETHING MORE SUITABLE 
    public bool Has_True_Hit;
    public int Stun_Percentage_Max;
    public int Stun_Percentage;
    public int Petrify_Percentage_Max;
    public int Petrify_Percentage;
    public bool Has_Immunity_To_Anything_Depending_On_The_Level;
    public GameObject stunnedObj;
    public int Level_Current
    {
        get { return _level_Current; }
        set
        {
            _level_Current = value;
            UpdateLevelStats();
        }
    }
    public List<Effect> effects;
    public int Level_Max => ((int)Stars) * 20;
    public int Experience_LevelUp
    {
        get { return (_level_Current >= 1 ? Level_Current : 1 + 1) * (200 + _level_Current >= 1 ? Level_Current : 1); }
        set { Debug.Log("Level Current Value " + Level_Current + " multiplying by 60 " + Level_Current * 60); Experience_LevelUp_Inspector = value; }
    }
    public int Experience_Required
    {
        get { return Experience_LevelUp - Experience_Current; }
        set { Experience_Required_Inspector = value; }
    }
    public int Experience_LevelUp_Inspector;
    public int Experience_Required_Inspector;
    public int Experience_Current;
    public bool Skills_1_Unlocked => Level_Current >= 1;
    public bool Skills_2_Unlocked => Level_Current >= 40;
    public bool Skills_3_Unclocked => Level_Current >= 60;
    public bool Skills_4_Unlocked => Level_Current >= 80;
    public bool _stunned;
    public bool Stunned
    {
        get { return _stunned; }
        set
        {
            if (_stunned != value)
            {
                _stunned = value;

            }
            if (_stunned == true)
            {
                Debug.Log("Stunned applied for : " + this.name);
                GameObject StunnedObj = Instantiate(Resources.Load<GameObject>("HeroPrefabs/Stunned_Sprite"), transform);
                stunnedObj = StunnedObj;
                stunnedObj.transform.position = this.transform.position + Vector3.up * 8f;
            }
            else if (_stunned == false)
            {
                Debug.Log("Stunned removed for 1 : " + this.name);
                if (stunnedObj != null)
                {
                    Debug.Log("Stunned removed for 2 : " + this.name);
                    Destroy(stunnedObj);
                    stunnedObj = null;
                }
            }
        }
    }
    GameObject selectObj;
    private bool _select;
    public bool Select
    {
        get { return _select; }
        set
        {
            if (_select != value)
            {
                _select = value;
            }
            if (_select == true)
            {
                GameObject SelectObject = Instantiate(Resources.Load<GameObject>("HeroPrefabs/Select_Sprite"), transform);
                selectObj = SelectObject;
                selectObj.transform.position = this.transform.position + Vector3.up * 12f;
            }
            else if (Select == false)
            {
                Debug.LogError("Select UI False For : " + this.name);
                if (selectObj != null)
                {
                    Debug.Log("Select UI Removed 2 :  " + this.name);
                    Destroy(selectObj);
                    selectObj = null;
                }
            }
        }
    }

    private bool _dead;
    public bool Dead
    {
        get { return _dead; }
        set
        {
            if (_dead != value)
            {
                _dead = value;
                if (_dead == true)
                    Animator.SetTrigger("Dead");
                return;
            }
            if (_dead == true)
                Animator.SetTrigger("Dead");
        }
    }
    public List<Item> items;
    public ObservableCollection<Item> Items { get; set; }
    #endregion

    public bool IsBusy;
    public Character SelectedEnemy;
    protected void Start()
    {
        effects = new List<Effect>();
        Debug.Log("CHARACTER START METHOD");
        Items = new ObservableCollection<Item>();

        //Level_Current = 1;

        Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(
                                delegate (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                                {
                                    if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                                    {
                                        Debug.LogWarning("THE NEW ITEM INDEX : " + e.NewStartingIndex);
                                        //UpdateItemStats(e.NewStartingIndex);
                                    }
                                });
        foreach (var item in items)
        {
            Items.Add(item);
        }

        //Extras
        Critical_Percentage_Max = LimitToRange(Critical_Percentage_Max, 0, 100);
        Critical_Percentage = Critical_Percentage_Max;
        Block_Percentage_Max = LimitToRange(Block_Percentage_Max, 0, 100);
        Block_Percentage = Block_Percentage_Max;
        Critical_Damage_Percentage_Max = LimitToRange(Critical_Percentage_Max, 1, 500);
        Critical_Damage_Percentage = Critical_Damage_Percentage_Max;
        Mana_Current = 0;
        UpdateHealthUI();
        Health_Current = Health_Max;
        Attack_Current = Attack_Max;
        Armor_Current = Armor_Max;
        Speed_Current = Speed_Max;
        Dead = false;
        Animator = GetComponent<Animator>();
        Experience_LevelUp_Inspector = Experience_LevelUp;
        Experience_Required_Inspector = Experience_Required;
    }

    public void UpdateLevelStats()
    {
        Health_Current = Health_Start * Level_Current;
        Attack_Current = Attack_Start * Level_Current;
        Armor_Current = Armor_Start * Level_Current;
        Speed_Current = Speed_Start * Level_Current;
        if (Health_Current > Health_Max)
            Health_Max = Health_Current;
        if (Attack_Current > Attack_Max)
            Attack_Max = Attack_Current;
        if (Armor_Current > Armor_Max)
            Armor_Max = Armor_Current;
        if (Speed_Current > Speed_Max)
            Speed_Current = Speed_Max;
    }

    public void UpdateItemsStats()
    {
        foreach (Item i in Items)
        {
            UpdateItemStats(i);
        }
    }

    public void UpdateItemStats(Item item)
    {
        this.Health_Max += item.Health_Bonus;
        this.Attack_Max += item.Attack_Bonus;
        this.Armor_Max += item.Armor_Bonus;
    }

    public void UpdateItemStats(int index)
    {
        this.Health_Max += Items[index].Health_Bonus;
        this.Attack_Max += Items[index].Attack_Bonus;
        this.Armor_Max += Items[index].Armor_Bonus;
    }

    public void SetStunned(bool stunned = true)
    {
        Stunned = stunned;
    }

    public void SetDead(bool dead = true)
    {
        Dead = dead;
    }

    public bool GetDead()
    {
        return false;
    }

    public Character(CharacterJson characterJson)
    {
        ID = characterJson.Id;
        Level_Current = characterJson.Current_Level;
        Items = characterJson.Items;
    }

    public override string ToString()
    {
        return name + ID + Stars + Faction;
    }

    public static int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
    {
        if (value < inclusiveMinimum) { return inclusiveMinimum; }
        if (value > inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }

    //METHODS
    public virtual void Attack()
    {
        throw new System.NotImplementedException();
    }
    public virtual void ActiveSkill()
    {
        throw new System.NotImplementedException();
    }
    public virtual void StartTurn()
    {
        IsBusy = true;
        Debug.Log(this.Name + " turn to attack");
        if (Mana_Current >= 100)
        {
            ActiveSkill();
        }
        else
        {
            Attack();
        }
    }
    public virtual void TakeDamage(int damage)
    {
        Animator.SetTrigger("Hurt");
        Debug.Log("Receiver Current Health : " + Health_Current);
        Health_Current -= damage;
        UpdateManaUI(20);
        if (Health_Current <= 0)
        {
            Dead = true;
        }
        Debug.Log("Receiver Current Health After Damaged : " + Health_Current);
        FindObjectOfType<GameManager>().StartCoroutine(UpdateHealthUI());
    }

    public virtual IEnumerator UpdateHealthUI()
    {
        Image uiHealthAnimation = this.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        float healthChange = (float)Health_Current / (float)Health_Max;
        this.transform.GetChild(0).GetChild(2).GetComponent<Image>().fillAmount = healthChange;
        Debug.Log("Health Animation Fill Value : " + uiHealthAnimation.fillAmount + " difference to health change : " + healthChange);
        while ((float)uiHealthAnimation.fillAmount >= healthChange)
        {
            uiHealthAnimation.fillAmount -= 0.02f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public virtual void UpdateManaUI(int manaToRefill)
    {
        Mana_Current += manaToRefill;
        Mana_Current = LimitToRange(Mana_Current, 0, Mana_Max);
        float manaChange = (float)Mana_Current / (float)Mana_Max;
        this.transform.GetChild(0).GetChild(3).GetComponent<Image>().fillAmount = manaChange;
    }
}