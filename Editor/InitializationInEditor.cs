using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.IO;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Linq;

[InitializeOnLoad]
public class InitializationInEditor : AssetPostprocessor
{
    static InitializationInEditor()
    {
        Debug.Log(nameof(InitializationInEditor));
        Events.registeredPackages -= AddDependencies;
        Events.registeredPackages += AddDependencies;
    }
    private void OnPreprocessAsset()
    {
        Debug.Log(nameof(OnPreprocessAsset));
        Events.registeringPackages -= RemoveDependencies;
        Events.registeringPackages += RemoveDependencies;
    }



    private static void AddDependencies(PackageRegistrationEventArgs packageRegistrationEventArgs)
    {
        Debug.Log("add-----------------------");
        foreach (var item in packageRegistrationEventArgs.added)
        {
            Debug.Log(item.name);
        }
        if (packageRegistrationEventArgs.added.Any((x) => x.name == "com.dgw0103.addressablesutility"))
        {
            Debug.Log("add https://github.com/dgw0103/UnityUtility.git");
            Client.Add("https://github.com/dgw0103/UnityUtility.git");
        }
    }
    private static void RemoveDependencies(PackageRegistrationEventArgs packageRegistrationEventArgs)
    {
        Debug.Log("remove----------------------");
        foreach (var item in packageRegistrationEventArgs.removed)
        {
            Debug.Log(item.name);
        }
        if (packageRegistrationEventArgs.removed.Any((x) => x.name == "com.dgw0103.addressablesutility"))
        {
            Debug.Log("remove https://github.com/dgw0103/UnityUtility.git");
            Client.Remove("https://github.com/dgw0103/UnityUtility.git");
            Events.registeringPackages -= RemoveDependencies;
            Events.registeredPackages -= AddDependencies;
        }
    }
    private static async Task<bool> IsPackageInstalled()
    {
        try
        {
            ListRequest listRequest = Client.List();
            StatusCode resultStatus = await WaitForRequestCompletion(listRequest);

            if (resultStatus != StatusCode.Success)
            {
                Debug.LogError("Failed to retrieve package list: " + listRequest.Error.message);
                return false;
            }

            return listRequest.Result.Any((x) => x.name == "com.dgw0103.addressablesutility");
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred: " + ex.Message);
            return false;
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