using System;

namespace Framework.Utils
{
    /// <summary>
    /// Sets the <see cref="MonoSingleton{T}"/> class as Verbose, which will print various
    /// messages when creating and destroying instances.
    /// </summary>
    public class VerboseAttribute : Attribute
    {
    }

    /// <summary>
    /// Marks the created instance of the <see cref="MonoSingleton{T}"/> as don't destroy on
    /// load. This makes the object persist between scenes.
    /// </summary>
    public class DontDestroyOnLoadAttribute : Attribute
    {
    }

    /// <summary>
    /// Sets the <see cref="MonoSingleton{T}"/> class to create the instance by loading it
    /// from the Resources folder instead of creating it and attaching a script.
    /// </summary>
    /// <remarks>
    /// If the singleton is a prefab, loading it from Resources will include all its
    /// children.
    /// </remarks>
    public class LoadFromResourcesAttribute : Attribute
    {
        public string Path { get; private set; }

        public LoadFromResourcesAttribute(string path)
        {
            this.Path = path;
        }
    }
}