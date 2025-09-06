using Audune.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines the system for persistent data
  [AddComponentMenu("Audune/Object Persistence/Persistence System")]
  public sealed class PersistenceSystem : MonoBehaviour, IPersistenceSystem
  {
    // Static instance of the persistence system
    private static IPersistenceSystem _current;

    // Return the static instance of the persistence system
    public static IPersistenceSystem current => _current;


    // Persistence system variables
    [SerializeField, Tooltip("The format of the persistence files")]
    private EncoderType _persistenceFileFormat = EncoderType.MessagePack;

    // Internal state of the persistence system
    private Serializer _serializer;


    // Return the serializer of the persistence system
    public Serializer serializer => _serializer;
    

    // Event that is triggered when a file is read
    public event Action<File> onFileRead;

    // Event that is triggered when a file is written
    public event Action<File> onFileWritten;

    // Event that is triggered when a file is moved
    public event Action<File, File> onFileMoved;

    // Event that is triggered when a file is copied
    public event Action<File, File> onFileCopied;

    // Event that is triggered when a file is deleted
    public event Action<File> onFileDeleted;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
      // Set the static instance
      if (_current == null)
        _current = this;
      else
        Destroy(gameObject);
      
      // Create the serializer
      _serializer = new Serializer(_persistenceFileFormat);
    }

    // OnDestroy is called when the component will be destroyed
    private void OnDestroy()
    {
      // Reset the static instancce
      if ((object)_current == this)
        _current = null;
    }


    #region Managing adapters
    // Return all registered adapters
    public IEnumerable<Adapter> GetAdapters()
    {
      return GetComponents<Adapter>().OrderBy(a => a.adapterPriority);
    }
    #endregion

    #region Managing files
    // List the available files
    public IEnumerable<File> List(Predicate<string> predicate = null)
    {
      return GetAdapters().SelectMany(adapter => adapter.List(predicate).Select(path => adapter[path]));
    }

    // Return if the specified file exists
    public bool Exists(File file)
    {
      return file.Exists();
    }

    // Read a byte array from the specified file
    private byte[] ReadData(File file)
    {
      var data = file.Read();
      onFileRead?.Invoke(file);
      return data;
    }

    // Read a state from the specified file
    public State Read(File file)
    {
      var data = ReadData(file);
      return _serializer.DecodeState(data);
    }

    // Read a deserializable object from the specified file into an existing object
    public void Read(File file, IDeserializable deserializable)
    {
      var data = ReadData(file);
      _serializer.Decode(data, deserializable);
    }

    // Write the specified data to the specified file
    public void WriteData(File file, byte[] data)
    {
      file.Write(data);
      onFileWritten?.Invoke(file);
    }

    // Write the specified state to the specified file
    public void Write(File file, State state)
    {
      var data = _serializer.EncodeState(state);
      WriteData(file, data);
    }

    // Write the specified serializable object to the specified file
    public void Write(File file, ISerializable serializable)
    {
      var data = _serializer.Encode(serializable);
      WriteData(file, data);
    }

    // Move the specified file to a new destination file
    public void Move(File file, File destination)
    {
      if (file.adapter == destination.adapter)
      {
        file.Move(destination.path);
      }
      else
      {
        var data = file.Read();
        destination.Write(data);
        file.Delete();
      }

      onFileMoved?.Invoke(file, destination);
    }

    // Copy the specified file to a new destination file
    public void Copy(File file, File destination)
    {
      if (file.adapter == destination.adapter)
      {
        file.Move(destination.path);
      }
      else
      {
        var data = file.Read();
        destination.Write(data);
      }

      onFileCopied?.Invoke(file, destination);
    }

    // Delete the specified source file
    public void Delete(File source)
    {
      source.Delete();
      onFileDeleted?.Invoke(source);
    }
    #endregion
  }
}