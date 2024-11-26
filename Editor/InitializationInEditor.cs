using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
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
        Events.registeringPackages -= RemoveDependencies;
        Events.registeringPackages += RemoveDependencies;
    }



    private static void AddDependencies(PackageRegistrationEventArgs packageRegistrationEventArgs)
    {
        Debug.Log("add-----------------------");
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
            RemoveRequest removeRequest = Client.Remove("https://github.com/dgw0103/UnityUtility.git");
            Wait(removeRequest);
            Events.registeringPackages -= RemoveDependencies;
            Events.registeredPackages -= AddDependencies;
        }
    }
    private static async void Wait(Request request)
    {
        await WaitForRequestCompletion(request);
    }
    private static async Task<PackageCollection> GetPackageCollection()
    {
        try
        {
            ListRequest listRequest = Client.List();
            StatusCode resultStatus = await WaitForRequestCompletion(listRequest);

            if (resultStatus != StatusCode.Success)
            {
                Debug.LogError("Failed to retrieve package list: " + listRequest.Error.message);
                return null;
            }

            return listRequest.Result;
        }
        catch (Exception ex)
        {
            Debug.LogError("An error occurred: " + ex.Message);
            return null;
        }
    }
    private static async Task<StatusCode> WaitForRequestCompletion(Request request)
    {
        while (request.IsCompleted == false)
        {
            await Task.Delay(100);
        }

        return request.Status;
    }
}