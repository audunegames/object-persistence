using Audune.Utils.UnityEditor.Editor;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Persistence.Editor
{
  // Class that defines a property drawer for a file reference
  [CustomPropertyDrawer(typeof(FileReference))]
  public class FileReferenceDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var adapterName = property.FindPropertyRelative("_adapterName");
      var path = property.FindPropertyRelative("_path");

      EditorGUI.BeginProperty(position, label, property);

      var allAdapters = Object.FindObjectsByType<Adapter>(FindObjectsInactive.Include, FindObjectsSortMode.None);

      var linePosition = position.AlignTop(EditorGUIUtility.singleLineHeight, EditorGUIUtility.standardVerticalSpacing, out position);
      var fieldPosition = EditorGUI.PrefixLabel(linePosition, label);

      var adapterNamePosition = fieldPosition.AlignLeft(Mathf.Max(fieldPosition.width * 0.3f, 80.0f), EditorGUIUtility.standardVerticalSpacing, out fieldPosition);
      if (!allAdapters.Any(a => a.adapterName == adapterName.stringValue))
        adapterName.stringValue = string.Empty;
      adapterName.stringValue = EditorGUIExtensions.ItemPopup(adapterNamePosition, 
        Enumerable.Repeat(string.Empty, 1).Concat(allAdapters.Select(a => a.adapterName)).ToList(),
        adapterName.stringValue,
        name => new GUIContent(!string.IsNullOrEmpty(name) ? name : "(None)"));

      var separatorPosition = fieldPosition.AlignLeft(10.0f, EditorGUIUtility.standardVerticalSpacing, out fieldPosition);
      EditorGUI.LabelField(separatorPosition, ":/");

      path.stringValue = EditorGUI.TextField(fieldPosition, path.stringValue);

      EditorGUI.EndProperty();
    }

    // Return the height of the property
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
  }
}
