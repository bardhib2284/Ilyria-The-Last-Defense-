using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SqliteTest : MonoBehaviour {

    // Use this for initialization
    void Start () {

        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";
        
        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS Test (id INTEGER PRIMARY KEY AUTOINCREMENT, val INTEGER )";
        
        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        // Insert values in table
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO Test (val) VALUES (5)";
        cmnd.CommandText = "INSERT INTO Test (val) VALUES (2)";
        cmnd.CommandText = "INSERT INTO Test (val) VALUES (4)";
        cmnd.ExecuteNonQuery();

        // Read and print all values in table
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query ="SELECT * FROM Test";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        int i = 0;
        while (reader.Read())
        {
            Debug.Log("{id}: " + reader[i].ToString());
            Debug.Log("{val}: " + reader[i].ToString());
            i++;
        }

        // Close connection
        dbcon.Close();

    }
    
    // Update is called once per frame
    void Update () {
        
    }
}