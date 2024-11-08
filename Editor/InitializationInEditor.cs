using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.IO;

[InitializeOnLoad]
public class InitializationInEditor
{
    static InitializationInEditor()
    {
        Debug.Log(Client.Search("com.dgw0103.addressablesutility").Result);
    }
}