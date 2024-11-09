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
public class InitializationInEditor
{
    static InitializationInEditor()
    {
        Debug.Log(nameof(InitializationInEditor));
        Events.registeredPackages -= AddAndRemoveDependencies;
        Events.registeredPackages += AddAndRemoveDependencies;
    }



    private static void AddAndRemoveDependencies(PackageRegistrationEventArgs packageRegistrationEventArgs)
    {
        if (packageRegistrationEventArgs.added.Any((x) => x.name == "com.dgw0103.addressablesutility"))
        {
            Client.Add("https://github.com/dgw0103/UnityUtility.git");
        }
        else if (packageRegistrationEventArgs.removed.Any((x) => x.name == "com.dgw0103.addressablesutility"))
        {
            Client.Remove("https://github.com/dgw0103/UnityUtility.git");
            Events.registeredPackages -= AddAndRemoveDependencies;
        }
    }
    private static string Key { get => typeof(InitializationInEditor).Assembly + typeof(InitializationInEditor).FullName; }
    private static async void Initialize()
    {
        Debug.Log(nameof(Initialize));
        bool isPackageInstalled = await IsPackageInstalled();

        if (isPackageInstalled == false)
        {
            Debug.Log("add unity utility");
            Client.Add("https://github.com/dgw0103/UnityUtility.git");
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