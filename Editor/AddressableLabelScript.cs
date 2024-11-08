#if INCLUDE_ADDRESSABLE
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEngine;

internal class AddressableLabelScript
{
    [InitializeOnLoadMethod]
    private static void SetLabelRenamingEvent()
    {
        AddressableAssetSettings.OnModificationGlobal -= ChangeLabelEnumScript;
        AddressableAssetSettings.OnModificationGlobal += ChangeLabelEnumScript;
    }
    private static void ChangeLabelEnumScript(AddressableAssetSettings arg1, AddressableAssetSettings.ModificationEvent arg2, object arg3)
    {
        if (arg2 == AddressableAssetSettings.ModificationEvent.LabelAdded || arg2 == AddressableAssetSettings.ModificationEvent.LabelRemoved)
        {
            CodeWriter codeWriter = new CodeWriter();

            codeWriter.WriteLine("using System;");
            codeWriter.WriteLine();
            codeWriter.WriteLine("public enum AddressableLabel");
            codeWriter.BeginBlock();
            foreach (var item in arg1.GetLabels())
            {
                codeWriter.WriteLine(item.Replace(item[0], char.ToUpper(item[0])) + ",");
            }
            codeWriter.EndBlock();
            
            File.WriteAllText(AddressableLabelScriptWindow.FilePath, codeWriter.ToString());
            AssetDatabase.Refresh();
        }

    }
}
#endif