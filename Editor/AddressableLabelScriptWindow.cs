using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.PackageManager;
using System.Threading;
using System.Text;

internal class AddressableLabelScriptWindow : EditorWindow
{
    private static AddressableLabelScriptPathData addressableLabelScriptPathData;
    private const string titleName = "Addressable label script";
    private static readonly string addressableLabelScriptFileName = "AddressableLabel.cs";



    private void OnEnable()
    {
        string scriptFilePath = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
        string directoryPath = Path.GetDirectoryName(scriptFilePath);
        string dataScriptFilePath = directoryPath + "/AddressableLabelSettings.asset";

        addressableLabelScriptPathData = AssetDatabase.LoadAssetAtPath<AddressableLabelScriptPathData>(dataScriptFilePath);
        if (addressableLabelScriptPathData == null)
        {
            addressableLabelScriptPathData = CreateInstance<AddressableLabelScriptPathData>();
            AssetDatabase.CreateAsset(addressableLabelScriptPathData, dataScriptFilePath);
            AssetDatabase.SaveAssets();
        }
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Addressable label script file path", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label(Application.dataPath + "/Assets/", GUILayout.Width(230f));
        addressableLabelScriptPathData.filePath = EditorGUILayout.TextField(addressableLabelScriptPathData.filePath);
        GUILayout.Label(addressableLabelScriptFileName, GUILayout.Width(130f));
        GUILayout.EndHorizontal();
        EditorUtility.SetDirty(addressableLabelScriptPathData);
    }



    internal static string FilePath { get => Application.dataPath + "/" + addressableLabelScriptPathData.filePath + addressableLabelScriptFileName; }
    [MenuItem("Window/Asset Management/Addressables/" + titleName)]
    private static void OpenAddressableLabelWindow()
    {
        GetWindow<AddressableLabelScriptWindow>(false, titleName);
    }





    [CreateAssetMenu(fileName = nameof(AddressableLabelScriptPathData), menuName = "Addressables/" + nameof(AddressableLabelScriptPathData))]
    private class AddressableLabelScriptPathData : ScriptableObject
    {
        [SerializeField] internal string filePath;
    }
}