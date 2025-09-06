using Audune.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Persistence
{
  // Interface that defines a persistence system
  public interface IPersistenceSystem
  {
    // Return the serializer of the persistence system
    public Serializer serializer { get; }
    

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


    #region Managing adapters
    // Return all registered adapters
    public IEnumerable<Adapter> GetAdapters();

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

    #region Managing files
    // List the available files
    public IEnumerable<File> List(Predicate<string> predicate = null);

    // Return if the specified file exists
    public bool Exists(File file);

    // Read a state from the specified file
    public State Read(File file);

    // Read a deserializable object from the specified file into an existing object
    public void Read(File file, IDeserializable deserializable);

    // Write the specified state to the specified file
    public void Write(File file, State state);

    // Write the specified serializable object to the specified file
    public void Write(File file, ISerializable serializable);

    // Move the specified file to a new destination file
    public void Move(File file, File destination);

    // Copy the specified file to a new destination file
    public void Copy(File file, File destination);

    // Delete the specified source file
    public void Delete(File source);
    #endregion
  }
}