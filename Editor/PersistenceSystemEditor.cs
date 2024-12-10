using Audune.Utils.Types;
using Audune.Utils.Types.Editor;
using Audune.Utils.UnityEditor.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Persistence.Editor
{
  // Class that defines an editor for the persistence system
  [CustomEditor(typeof(PersistenceSystem))]
  public class PersistenceSystemEditor : UnityEditor.Editor
  {
    // Properties of the editor
    private SerializedProperty _persistenceFileFormat;

    // Foldout state of the editor
    private bool _settingsFoldout = true;
    private bool _adaptersFoldout = true;
    private bool _componentsFoldout = true;

    // Generic menus for types
    private GenericMenu _adapterTypesMenu;


    // Return the target object of the editor
    public new PersistenceSystem target => serializedObject.targetObject as PersistenceSystem;


    // OnEnable is called when the component becomes enabled
    protected void OnEnable()
    {
      // Initialize the properties
      _persistenceFileFormat = serializedObject.FindProperty("_persistenceFileFormat");

      // Intialize the generic menus for types
      _adapterTypesMenu = typeof(Adapter).CreateGenericMenuForChildTypes(TypeDisplayOptions.DontShowNamespace, null, type => target.gameObject.AddComponent(type));
    }

    // Draw the inspector GUI
    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      EditorGUI.BeginChangeCheck();

      _settingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_settingsFoldout, "Settings");
      if (_settingsFoldout)
      {
        EditorGUILayout.PropertyField(_persistenceFileFormat);

        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFoldoutHeaderGroup();

      _adaptersFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_adaptersFoldout, "Registered Adapters");
      if (_adaptersFoldout)
      {
        var adapters = target.GetAdapters().ToList();
        if (adapters.Count > 0)
          EditorGUILayout.HelpBox(string.Join("\n", adapters.Select(a => $"• {a.GetType().ToDisplayString(TypeDisplayOptions.DontShowNamespace)} \"{a.adapterName}\" [Priority {a.adapterPriority}]")), MessageType.None);
        else
          EditorGUILayout.HelpBox("None", MessageType.None);

        var emptyAdapterNames = adapters.Where(a => string.IsNullOrEmpty(a.adapterName)).ToList();
        if (emptyAdapterNames.Count > 0)
          EditorGUILayout.HelpBox($"Warning - The following adapters have no name:\n{string.Join("\n", emptyAdapterNames.Select(a => $"� {a.GetType().ToDisplayString(TypeDisplayOptions.DontShowNamespace)}"))}", MessageType.None);

        var duplicatedAdapterNames = adapters.GroupBy(a => a.adapterName).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicatedAdapterNames.Count > 0)
          EditorGUILayout.HelpBox($"Warning - The following adapter names are duplicated:\n{string.Join("\n", duplicatedAdapterNames.Select(n => $"� \"{n}\""))}", MessageType.None);

        EditorGUILayout.Space();
      }
      EditorGUILayout.EndFoldoutHeaderGroup();

      _componentsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_componentsFoldout, "Components");
      if (_componentsFoldout)
      {
        var addAdapterPosition = EditorGUILayout.GetControlRect();
        addAdapterPosition = EditorGUI.PrefixLabel(addAdapterPosition, new GUIContent("Add Adapter"));
        EditorGUIExtensions.GenericMenuDropdown(addAdapterPosition, new GUIContent("(select)"), _adapterTypesMenu);
      }
      EditorGUILayout.EndFoldoutHeaderGroup();

      if (EditorGUI.EndChangeCheck())
        serializedObject.ApplyModifiedProperties();
    }
  }
}