using Audune.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Audune.Persistence
{
  // Class that defines the system for persistent data
  [AddComponentMenu("Audune/Persistence/Persistence System")]
  public sealed class PersistenceSystem : MonoBehaviour
  {
    // Persistence system properties
    [SerializeField, Tooltip("The format of the persistence files")]
    private EncoderType _persistenceFileFormat = EncoderType.MessagePack;

    // Internal state of the persistence system
    private Serializer _serializer;

    // Persistence system events
    public event Action<File> OnFileRead;
    public event Action<File> OnFileWritten;
    public event Action<File, File> OnFileMoved;
    public event Action<File, File> OnFileCopied;
    public event Action<File> OnFileDeleted;


    // Awake is called when the script instance is being loaded
    private void Awake()
    {
      // Create the serializer
      _serializer = new Serializer(_persistenceFileFormat);
    }


    #region Adapter management
    // Return all registered adapters
    public IEnumerable<Adapter> GetAdapters()
    {
      return GetComponents<Adapter>().OrderBy(a => a.adapterPriority);
    }

    // Return all enabled registered adapters
    public IEnumerable<Adapter> GetEnabledAdapters()
    {
      return GetAdapters().Where(adapter => adapter.adapterEnabled);
    }

    // Return if an adapter with the specified name exists
    public bool TryGetAdapter(string name, out Adapter adapter)
    {
      adapter = GetAdapters().Where(adapter => adapter.adapterName == name).FirstOrDefault();
      return adapter != null;
    }

    // Return the adapter with the specified name
    public Adapter GetAdapter(string name)
    {
      if (TryGetAdapter(name, out Adapter adapter))
        return adapter;
      else
        throw new PersistenceException($"Could not find a registered adapter with name {name}");
    }

    // Return if there is a first adapter that is enabled
    public bool TryGetFirstEnabledAdapter(out Adapter adapter)
    {
      adapter = GetEnabledAdapters().FirstOrDefault();
      return adapter != null;
    }

    // Return the first adapter that is enabled
    public Adapter GetFirstEnabledAdapter()
    {
      if (TryGetFirstEnabledAdapter(out Adapter adapter))
        return adapter;
      else
        throw new PersistenceException("Could not find an enabled registered adapter");
    }
    #endregion

    #region File management
    // List the available files
    public IEnumerable<File> List(Predicate<string> predicate = null)
    {
      return GetAdapters().SelectMany(adapter => adapter.List(predicate).Select(path => adapter.GetFile(path)));
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
      OnFileRead?.Invoke(file);
      return data;
    }

    // Read a state from the specified file
    public State Read<TState>(File file)
    {
      var data = ReadData(file);
      return _serializer.DecodeState(data);
    }

    /*// Read a deserializable object from the specified file into an existing object
    public void Read<TState>(File file, IPicklable<TState> deserializable) where TState : State
    {
      var data = ReadData(file);
      _pickler.Decode(data, deserializable);
    }

    // Read a deserializable object from the specified file into an existing object with the provided context
    public void Read<TState, TContext>(File file, ISerializable<TState, TContext> deserializable, TContext context) where TState : State
    {
      var data = ReadData(file);
      _pickler.Decode(data, deserializable, context);
    }*/

    // Write the specified data to the specified file
    public void WriteData(File file, byte[] data)
    {
      file.Write(data);
      OnFileWritten?.Invoke(file);
    }

    // Write the specified state to the specified file
    public void Write(File file, State state)
    {
      var data = _serializer.EncodeState(state);
      WriteData(file, data);
    }

    /*// Write the specified serializable object to the specified file
    public void Write<TState>(File file, IPicklable<TState> serializable) where TState : State
    {
      var data = _pickler.Encode(serializable);
      WriteData(file, data);
    }

    // Write the specified serializable object to the specified file with the provided context
    public void Write<TState, TContext>(File file, ISerializable<TState, TContext> serializable, TContext context) where TState : State
    {
      var data = _pickler.Encode(serializable, context);
      WriteData(file, data);
    }*/

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

      OnFileMoved?.Invoke(file, destination);
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

      OnFileCopied?.Invoke(file, destination);
    }

    // Delete the specified source file
    public void Delete(File source)
    {
      source.Delete();
      OnFileDeleted?.Invoke(source);
    }
    #endregion
  }
}