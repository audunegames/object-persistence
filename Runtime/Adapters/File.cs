using System;
using System.Collections.Generic;

namespace Audune.Persistence
{
  // Class that defines a persistence file
  public sealed class File : IEquatable<File>
  {
    // File variables
    public readonly Adapter adapter;
    public readonly string path;


    // Constructor
    internal File(Adapter adapter, string path)
    {
      this.adapter = adapter;
      this.path = path;
    }


    // Return if the file exists
    internal bool Exists()
    {
      return adapter.Exists(path);
    }

    // Read a stream of bytes from the file
    internal byte[] Read()
    {
      return adapter.Read(path);
    }

    // Write the specified stream of bytes to the file
    internal void Write(byte[] data)
    {
      adapter.Write(path, data);
    }

    // Move the file to a new destination file and return the moved file
    internal File Move(string destination)
    {
      adapter.Move(path, destination);
      return new File(adapter, destination);
    }

    // Copy the file to a new destination file and return the copied file
    internal File Copy(string destination)
    {
      adapter.Copy(path, destination);
      return new File(adapter, destination);
    }

    // Delete the file
    internal void Delete()
    {
      adapter.Delete(path);
    }


    // Return the string representation of the file
    public override string ToString()
    {
      return $"{adapter.adapterName}:/{path}";
    }


    #region Equatable implementation
    // Return if the file equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as File);
    }

    // Return if the file equals another file
    public bool Equals(File other)
    {
      return other is not null && EqualityComparer<Adapter>.Default.Equals(adapter, other.adapter) && path == other.path;
    }

    // Return the hash code of the file
    public override int GetHashCode()
    {
      return HashCode.Combine(adapter, path);
    }
    #endregion

    #region Equality operators
    // Return if the file equals another file
    public static bool operator ==(File left, File right)
    {
      return EqualityComparer<File>.Default.Equals(left, right);
    }

    // Return if the file does not equal another file
    public static bool operator !=(File left, File right)
    {
      return !(left == right);
    }
    #endregion
  }
}