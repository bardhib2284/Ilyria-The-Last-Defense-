using UnityEngine;
using System.Collections;
using System.Data;

public class InventoryDB : SqliteHelper
{
    private const string Tag = "EvilEye: InventoryDB:\t";

    private const string TABLE_NAME = "Inventory";

    private const string KEY_ID = "Resource_ID";
    private const string KEY_NAME = "Resource_Name";
    private const string KEY_VALUE = "Resource_Value";
    private const string KEY_ICONPATH = "Resource_IconPath";

    public InventoryDB() : base()
    {
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME
                        + " ( " +
                        KEY_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        KEY_NAME + " TEXT, " +
                        KEY_VALUE + " TEXT, " +
                        KEY_ICONPATH + " TEXT"
                        + " ) ";
        dbcmd.ExecuteNonQuery();
        using (IDataReader data = GetDataByString("GOLD"))
        {
            if (data.Read())
                return;
        }
        AddConsumable(new Consumable { Name = "GOLD", Value = 5000, Icon = "Textures/Consumable" });
        AddConsumable(new Consumable { Name = "GEM", Value = 500, Icon = "Textures/Consumable" });
    }

    public void AddConsumable(Consumable c)
    {
        IDataReader data = GetDataByString(c.Name);
        if (data.Read())
        {
            Debug.Log("Updating an old resource in the database :");
            UpdateResource(c);
            return;
        }
        Debug.Log("Adding a new resource in the database :");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_NAME + ", "
                + KEY_VALUE + ", "
                + KEY_ICONPATH + " ) "

                + "VALUES ( '"
                + c.Name + "', '"
                + c.Value + "', '"
                + string.Empty + "' )";
        Debug.Log(dbcmd.CommandText);
        int result = dbcmd.ExecuteNonQuery();
    }

    public override IDataReader GetDataById(int id)
    {
        Debug.Log(Tag + "Getting Resource By Id: " + id);
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
        return dbcmd.ExecuteReader();
    }

    public override IDataReader GetDataByString(string str)
    {
        Debug.Log(Tag + "Getting Resource By String: " + str);
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_NAME + " = '" + str + "'";
        return dbcmd.ExecuteReader();
    }

    public override void DeleteDataById(int id)
    {
        base.DeleteDataById(id);
    }

    public override void DeleteDataByString(string id)
    {
        Debug.Log(Tag + "Deleting Resource: " + id);

        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
        dbcmd.ExecuteNonQuery();
    }

    public override void DeleteAllData()
    {
        DeleteAllData(TABLE_NAME);
    }

    public IDataReader GetResources()
    {
        Debug.Log(Tag + "Getting All Resources : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT * FROM " + TABLE_NAME;
        return dbcmd.ExecuteReader();
    }

    public override IDataReader GetNumOfRows()
    {
        Debug.Log(Tag + "Getting All Resources : ");
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "SELECT COUNT(*) FROM " + TABLE_NAME;
        return dbcmd.ExecuteReader();
    }

    public IDataReader UpdateResource(Consumable c)
    {
        IDbCommand dbcmd = GetDbCommand();
        dbcmd.CommandText =
            "UPDATE " + TABLE_NAME +
            " SET " + KEY_VALUE + " = " + c.Value +
            " WHERE " + KEY_ID + " = " + c.Id;
        Debug.Log(dbcmd.CommandText);
        return dbcmd.ExecuteReader();
    }
}