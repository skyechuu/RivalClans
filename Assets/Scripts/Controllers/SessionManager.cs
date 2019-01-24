using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public static SessionManager instance;
    DatabaseManager dbm;

    void Awake()
    {
        if (instance == null)
            instance = this;
        LoadDatabase();
    }

    public DatabaseManager GetDatabaseManager()
    {
        return dbm;
    }

    void LoadDatabase()
    {
        dbm = new DatabaseManager();
    }

}
