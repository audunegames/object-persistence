using UnityEditor;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines an editor for a local adapter
  [CustomEditor(typeof(LocalAdapter))]
  public class LocalAdapterEditor : UnityEditor.Editor
  {
    // Properties of the editor
    private SerializedProperty _adapterName;
    private SerializedProperty _adapterPriority;
    private SerializedProperty _root;
    private SerializedProperty _directory;
    private SerializedProperty _extension;

    // Foldout state of the editor
    private bool _adapterSettingsFoldout = true;
    private bool _executionSettingsFoldout = false;


    // Return the target object of the editor
    public new LocalAdapter target => serializedObject.targetObject as LocalAdapter;


    // OnEnable is called when the component becomes enabled
    protected void OnEnable()
    {
      // Initialize the properties
      _adapterName = serializedObject.FindProperty("_adapterName");
      _adapterPriority = serializedObject.FindProperty("_adapterPriority");
      _root = serializedObject.FindProperty("_root");
      _directory = serializedObject.FindProperty("_directory");
      _extension = serializedObject.FindProperty("_extension");
    }

    // Draw the inspector GUI
    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      EditorGUI.BeginChangeCheck();

      EditorGUILayout.PropertyField(_adapterName, new GUIContent("Name", "The name of the adapter"));

      EditorGUILayout.Space();

      _adapterSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_adapterSettingsFoldout, "Adapter Settings");
      if (_adapterSettingsFoldout)
      {
        EditorGUILayout.PropertyField(_root);
        EditorGUILayout.PropertyField(_directory);
        EditorGUILayout.PropertyField(_extension);

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox($"Files are saved to {target.directory}", MessageType.None);
        if (GUILayout.Button("Show in explorer"))
          EditorUtility.RevealInFinder(target.directory);

        EditorGUILayout.Space();
      }
      EditorGUI.EndFoldoutHeaderGroup();

      _executionSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_executionSettingsFoldout, "Execution Settings");
      if (_executionSettingsFoldout)
      {
        EditorGUILayout.PropertyField(_adapterPriority, new GUIContent("Priority", "The priority of the adapter"));
      }
      EditorGUI.EndFoldoutHeaderGroup();

      if (EditorGUI.EndChangeCheck())
        serializedObject.ApplyModifiedProperties();
    }
  }
}