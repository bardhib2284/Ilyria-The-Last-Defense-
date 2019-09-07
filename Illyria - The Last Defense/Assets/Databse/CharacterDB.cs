using UnityEngine;
using System.Collections;
using System.Data;
using System;

public class CharacterDB : SqliteHelper
{
    private const string Tag = "EvilEye: CharacterDB:\t";

    //TODO: FOR NOW THESE ONLY
    private const string TABLE_NAME = "Characters";
    private const string KEY_ID = "id";
    private const string KEY_Name = "name";
    private const string KEY_LEVEL = "level";
    private const string KEY_EXPERIENCE = "experience";
    private const string KEY_STARS = "stars";
    private const string KEY_FACTION = "faction";
    private const string KEY_ITEM_1_Name = "item_1_name";
    private const string KEY_ITEM_2_Name = "item_2_name";
    private const string KEY_ITEM_3_Name = "item_3_name";
    private const string KEY_ITEM_4_Name = "item_4_name";

    public CharacterDB() : base()
    {
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME
                        + " ( " +
                        KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        KEY_Name + " TEXT, " +
                        KEY_LEVEL + " INTEGER, " +
                        KEY_STARS + " INTEGER, " +
                        KEY_EXPERIENCE + " INTEGER, " +
                        KEY_FACTION + " TEXT, " +
                        KEY_ITEM_1_Name + " TEXT, " +
                        KEY_ITEM_2_Name + " TEXT, " +
                        KEY_ITEM_3_Name + " TEXT, " +
                        KEY_ITEM_4_Name + " TEXT "
                        + " ) ";
        dbcmd.ExecuteNonQuery();
    }

    public void AddCharacter(Character c)
    {
        Debug.Log("Adding a new character in the database :");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_Name + ", "
                + KEY_LEVEL + ", "
                + KEY_STARS + ", "
                + KEY_EXPERIENCE + ", "
                + KEY_FACTION + ", "
                + KEY_ITEM_1_Name + ", "
                + KEY_ITEM_2_Name + ", "
                + KEY_ITEM_3_Name + ", "
                + KEY_ITEM_4_Name + " ) "

                + "VALUES ( '"
                + c.Name + "', '"
                + c.Level_Current + "', '"
                + (int)c.Stars + "', '"
                + 0 + "', '"
                + c.Faction.ToString() + "', '"
                + string.Empty + "', '"
                + string.Empty + "', '"
                + string.Empty + "', '"
                + string.Empty + "' )";
        Debug.Log(dbcmd.CommandText);
        int result = dbcmd.ExecuteNonQuery();
    }

    public override IDataReader GetDataById(int id)
    {
        Debug.Log(Tag + "Getting Character: " + id);
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
        return dbcmd.ExecuteReader();
    }

    public override IDataReader GetDataByString(string str)
    {
        Debug.Log(Tag + "Getting Character: " + str);
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + str + "'";
        return dbcmd.ExecuteReader();
    }

    public override void DeleteDataById(int id)
    {
        base.DeleteDataById(id);
    }

    public override void DeleteDataByString(string id)
    {
        Debug.Log(Tag + "Deleting Character: " + id);

        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
        dbcmd.ExecuteNonQuery();
    }

    public override void DeleteAllData()
    {
        DeleteAllData(TABLE_NAME);
    }

    public IDataReader GetCharacters()
    {
        Debug.Log(Tag + "Getting All Characters : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME;
        return dbcmd.ExecuteReader();
    }

    public IDataReader GetCharactersSortedByLevel()
    {
        Debug.Log(Tag + "Getting All Characters : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_LEVEL + " DESC ";
        return dbcmd.ExecuteReader();
    }

    public IDataReader GetCharactersSortedBySpecificLevel(int stars)
    {
        Debug.Log(Tag + "Getting All Characters : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_STARS + " = " + stars + " ORDER BY " + KEY_LEVEL + " DESC ";
        return dbcmd.ExecuteReader();
    }

    public IDataReader GetCharactersSortedByFaction(Character.CharacterFaction c)
    {
        Debug.Log(Tag + "Getting All Characters : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_FACTION + c;
        return dbcmd.ExecuteReader();
    }

    public override IDataReader GetNumOfRows()
    {
        Debug.Log(Tag + "Getting All Characters : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT COUNT(*) FROM " + TABLE_NAME;
        return dbcmd.ExecuteReader();
    }

    public IDataReader UpdateCharacter(Character c)
    {
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            dbcmd.CommandText =
            "UPDATE " + TABLE_NAME +
            " SET " + KEY_LEVEL + " = " + c.Level_Current + ", " +
            KEY_STARS + " = " + (int)c.Stars + ", " +
            KEY_EXPERIENCE + " = " + c.Experience_Current + ", " +
            KEY_ITEM_1_Name + " = " + null + ", " +
            KEY_ITEM_2_Name + " = " + null + ", " +
            KEY_ITEM_3_Name + " = " + null + ", " +
            KEY_ITEM_3_Name + " = " + null +
            " WHERE " + KEY_ID + " = " + 1;
        Debug.Log(dbcmd.CommandText);
        return dbcmd.ExecuteReader();
    }

    public IDataReader UpdateCharacterExperience(Character c)
    {
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            dbcmd.CommandText =
            "UPDATE " + TABLE_NAME +
            " SET " +
            KEY_EXPERIENCE + " = " + c.Experience_Current + " " +
            " WHERE " + KEY_ID + " = " + c.ID;
        Debug.Log(dbcmd.CommandText);
        return dbcmd.ExecuteReader();
    }

    #region Gets
    public string GetColumnName()
    {
        return KEY_Name;
    }
    public string GetColumnStars()
    {
        return KEY_STARS;
    }
    public string GetColumnLevel()
    {
        return KEY_LEVEL;
    }
    public string GetColumnFaction()
    {
        return KEY_FACTION;
    }
    public string GetColumnItem_1()
    {
        return KEY_ITEM_1_Name;
    }
    public string GetColumnItem_2()
    {
        return KEY_ITEM_2_Name;
    }
    public string GetColumnItem_3()
    {
        return KEY_ITEM_3_Name;
    }
    public string GetColumnItem_4()
    {
        return KEY_ITEM_4_Name;
    }
    #endregion
}