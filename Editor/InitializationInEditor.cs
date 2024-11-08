using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

[InitializeOnLoad]
public class InitializationInEditor
{
    static InitializationInEditor()
    {
        ListRequest listRequest = Client.List();
        Debug.Log(true);
    }
}