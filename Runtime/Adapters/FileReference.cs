using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines a reference to a file
  [Serializable]
  public sealed class FileReference : IEquatable<FileReference>
  {
    // File reference variables
    [SerializeField, Tooltip("The adapter name of the file")]
    private string _adapterName;
    [SerializeField, Tooltip("The path of the file")]
    private string _path;


    // Constructor
    public FileReference(string adapterName, string path)
    {
      _adapterName = adapterName;
      _path = path;
    }

    // Constructor for a file
    public FileReference(File file)
    {
      _adapterName = file.adapter.adapterName;
      _path = file.path;
    }


    // Resolve the file reference
    public File Resolve()
    {
      if (PersistenceSystem.current == null)
        throw new PersistenceException("Could not find a persistence system in the scene");

      if (PersistenceSystem.current.TryGetAdapter(_adapterName, out var adapter))
        return adapter[_path];
      else
        throw new PersistenceException($"Could not find a registered adapter with name \"{_adapterName}\"");
    }


    #region Equatable implementation
    // Return if the file reference equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as FileReference);
    }

    // Return if the file reference equals another file reference
    public bool Equals(FileReference other)
    {
      return other is not null && _adapterName == other._adapterName && _path == other._path;
    }

    // Return the hash code of the file reference
    public override int GetHashCode()
    {
      return HashCode.Combine(_adapterName, _path);
    }
    #endregion

    #region Equality operators
    // Return if the file reference equals another file reference
    public static bool operator ==(FileReference left, FileReference right)
    {
      return EqualityComparer<FileReference>.Default.Equals(left, right);
    }

    // Return if the file reference does not equal another file reference
    public static bool operator !=(FileReference left, FileReference right)
    {
      return !(left == right);
    }
    #endregion

    #region Implicit operators
    // Convert a file reference to a file
    public static implicit operator File(FileReference fileReference)
    {
      return fileReference.Resolve();
    }
    #endregion
  }
}