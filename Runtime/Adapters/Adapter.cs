using System.Collections.Generic;
using System;
using UnityEngine;

namespace Audune.Persistence
{
  // Base class that defines an adapter for the persistence system
  [RequireComponent(typeof(PersistenceSystem))]
  public abstract class Adapter : MonoBehaviour
  {
    // Adapter variables
    [SerializeField, Tooltip("The name of the adapter")]
    private string _adapterName = "Adapter";
    [SerializeField, Tooltip("The priority of the adapter")]
    private int _adapterPriority;


    // Return the name of the adapter
    public string adapterName => _adapterName;

    // Return the priority of the adapter
    public int adapterPriority => _adapterPriority;

    // Return if the adapter is enabled
    public abstract bool adapterEnabled { get; }


    // Return a persistent file in the adapter
    public File this[string path] => new File(this, path);


    #region Managing files
    // List the available files in the adapter
    public abstract IEnumerable<string> List(Predicate<string> predicate = null);

    // Return if the specified file exists
    public abstract bool Exists(string path);

    // Read a stream of bytes from the specified file
    public abstract byte[] Read(string path);

    // Write the specified stream of bytes to the specified file
    public abstract void Write(string path, byte[] data);

    // Move the specified file to a new destination file
    public abstract void Move(string path, string destination);

    // Copy the specified file to a new destination file
    public abstract void Copy(string path, string destination);

    // Delete the specified file
    public abstract void Delete(string path);
    #endregion
  }
}