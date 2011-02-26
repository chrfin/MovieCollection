using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Imdb
{
    /// <summary>
    /// FileCache class
    /// </summary>
    public class FileCache
    {
        public string CacheDirectory { get; set; }
        public int MaxDays{ get; set; }

        public FileCache() 
        {
            this.CacheDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"Cache");
            this.MaxDays = 7;
            createDirectory();
        }

        private void createDirectory()
        {
            if (!Directory.Exists(this.CacheDirectory))
                Directory.CreateDirectory(this.CacheDirectory);
        }

        public FileCache(string cacheDirectory, int maxDays)
        {
            this.CacheDirectory = cacheDirectory;
            this.MaxDays = maxDays;
            createDirectory();
        }

        public object Read(string name)
        {
            name = Path.Combine(this.CacheDirectory, name + ".cache");
            if (!File.Exists(name))
                return null;
            else
            {
                var Cache = Unserialize(name);
                return Cache.Created.AddDays(this.MaxDays) > DateTime.Now ? Cache.Object : null;
            }
        }

        public bool Save(string name, object value)
        {
            return Serialize(new CacheObject() { Created = DateTime.Now, Object = value }, Path.Combine(this.CacheDirectory, name + ".cache"));
        }

        private CacheObject Unserialize(string file)
        {
            try
            {
                BinaryFormatter serializer = new BinaryFormatter();
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    CacheObject Cache;
                    Cache = (CacheObject)serializer.Deserialize(fs);
                    return Cache;
                }
            }
            catch {return null;}            
        }

        private bool Serialize(object obj, string file)
        {
            try
            {
                var serializer = new BinaryFormatter(){AssemblyFormat=System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple};
                using (var fs = new FileStream(file, FileMode.OpenOrCreate))
                    serializer.Serialize(fs, obj);
                return true;
            }
            catch { return false; }
        } 

    }

    [Serializable()]
    public class CacheObject
    {
        public DateTime Created{ get; set; }
        public object Object{ get; set; }
    }

}
