using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.IO;
using System.Threading;
using System;
using System.Threading.Tasks;

[InitializeOnLoad]
public class InitializationInEditor
{
    static InitializationInEditor()
    {
        RunPackageListTask();
    }
    private static async void RunPackageListTask()
    {
        try
        {
            ListRequest listRequest = Client.List();
            StatusCode resultStatus = await WaitForRequestCompletion(listRequest);

            if (resultStatus != StatusCode.Success)
            {
                Debug.LogError("Failed to retrieve package list: " + listRequest.Error.message);
                return;
            }

            foreach (var package in listRequest.Result)
            {
                Debug.Log(package.name);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred: " + ex.Message);
        }
    }
    private static async Task<StatusCode> WaitForRequestCompletion(ListRequest request)
    {
        while (request.IsCompleted == false)
        {
            await Task.Delay(100);
        }

        return request.Status;
    }
}