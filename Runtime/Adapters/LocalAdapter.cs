using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines an adapter that stores data on the local filesystem
  [AddComponentMenu("Audune/Persistence/Adapters/Local Adapter")]
  public class LocalAdapter : Adapter
  {
    // Enum that defines the directory type
    public enum DirectoryType
    {
      [InspectorName("Unity Data Path")]
      DataPath,
      [InspectorName("Unity Persistent Data Path")]
      PersistentDataPath,
      [InspectorName("Unity StreamingAssets Path")]
      StreamingAssetsPath,
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
      [InspectorName("Windows Application Data")]
      ApplicationData,
      [InspectorName("Windows Local Application Data")]
      LocalApplicationData,
      [InspectorName("Windows User Profile")]
      UserProfile,
      [InspectorName("Windows User Profile Documents")]
      MyDocuments,
#endif
    }


    // Local persistence adapter variables
    [SerializeField, Tooltip("The root location to store files")]
    private DirectoryType _root = DirectoryType.PersistentDataPath;
    [SerializeField, Tooltip("The directory to store files, relative to the directory type")]
    private string _directory = "Saves";
    [SerializeField, Tooltip("The extension to use to store files")]
    private string _extension = ".save";


    // Return if the adapter is accessible
    public override bool adapterEnabled => enabled;

    // Return the absolute directory to store files
    public string directory {
      get {
        var directory = _directory
          .Replace("$productName$", Application.productName)
          .Replace("$companyName$", Application.companyName)
          .Replace("$version$", Application.version);

        return _root switch {
          DirectoryType.DataPath => Path.Combine(Application.dataPath, directory),
          DirectoryType.PersistentDataPath => Path.Combine(Application.persistentDataPath, directory),
          DirectoryType.StreamingAssetsPath => Path.Combine(Application.streamingAssetsPath, directory),
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
          DirectoryType.ApplicationData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), directory),
          DirectoryType.LocalApplicationData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), directory),
          DirectoryType.UserProfile => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), directory),
          DirectoryType.MyDocuments => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), directory),
#endif
          _ => directory,
        };
      }
    }


    // Return the path for the specified source file
    private string GetPath(string source)
    {
      return Path.Combine(directory, $"{source}{_extension}");
    }


    #region Managing files  
    // List the available files in the adapter
    public override IEnumerable<string> List(Predicate<string> predicate = null)
    {
      try
      {
        if (!Directory.Exists(directory))
          return Enumerable.Empty<string>();

        return Directory.EnumerateFiles(directory, $"*{_extension}")
          .Select(path => {
            var relativePath = Path.GetRelativePath(directory, path);
            return relativePath[..(relativePath.Length - _extension.Length)];
          })
          .Where(path => predicate == null || predicate(path));
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not list files: {ex.Message}", ex);
      }
    }

    // Return the specified file
    public override bool Exists(string path)
    {
      var sourcePath = GetPath(path);

      return System.IO.File.Exists(sourcePath);
    }

    // Read a stream of bytes from the specified file
    public override byte[] Read(string path)
    {
      try
      {
        var sourcePath = GetPath(path);

        return System.IO.File.ReadAllBytes(sourcePath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not read file \"{path}\": {ex.Message}", ex);
      }
    }

    // Write the specified stream of bytes to the specified file
    public override void Write(string path, byte[] data)
    {
      try
      {
        var sourcePath = GetPath(path);

        if (!Directory.Exists(Path.GetDirectoryName(sourcePath)))
          Directory.CreateDirectory(Path.GetDirectoryName(sourcePath));

        System.IO.File.WriteAllBytes(sourcePath, data);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not write file \"{path}\": {ex.Message}", ex);
      }
    }

    // Move the specified file to a new destination file
    public override void Move(string path, string destination)
    {
      try
      {
        var sourcePath = GetPath(path);
        var destinationPath = GetPath(destination);

        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
          Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

        System.IO.File.Move(sourcePath, destinationPath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not move file \"{path}\": {ex.Message}", ex);
      }
    }

    // Copy the specified file to a new destination file
    public override void Copy(string path, string destination)
    {
      try
      {
        var sourcePath = GetPath(path);
        var destinationPath = GetPath(destination);

        if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
          Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

        System.IO.File.Copy(sourcePath, destinationPath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not copy file \"{path}\": {ex.Message}", ex);
      }
    }

    // Delete the specified file
    public override void Delete(string path)
    {
      try
      {
        var sourcePath = GetPath(path);

        System.IO.File.Delete(sourcePath);
      }
      catch (IOException ex)
      {
        throw new AdapterException($"Could not delete file \"{path}\": {ex.Message}", ex);
      }
    }
    #endregion
  }
}