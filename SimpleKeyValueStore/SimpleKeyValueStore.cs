using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SimpleKeyValueStore
{
    public class SimpleKeyValueStore
    {
        private IDictionary<string, string> dict;

        private readonly string defaultPath =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\SimpleKeyValueStore\";

        private Context context;


        public SimpleKeyValueStore(Context ctx = null)
        {
            this.Setup(ctx);
        }

        public SimpleKeyValueStore(string name)
        {
            this.Setup(new Context() {Name = name});
        }

        private void Setup(Context ctx)
        {
            this.dict = new ConcurrentDictionary<string, string>();

            if (ctx == null)
            {
                this.context = new Context()
                {
                    Name = "arbitrary",
                    Path = null
                };
            }

            if (this.context.Path == null)
                this.context.Path = this.defaultPath;

            if (!Directory.Exists(this.context.Path))
                Directory.CreateDirectory(this.context.Path);

            this.dict = this.LoadFromStore();
        }

        public void Set(string key, string value)
        {
            if (this.dict.ContainsKey(key))
                this.dict[key] = value;
            else
                this.dict.Add(key, value);
        }

        public (string, bool) TryGet(string key)
        {
            if (this.dict.ContainsKey(key))
                return (this.dict[key], true);

            return (null, false);
        }

        public void Flush()
        {
            string file = this.context.Path + this.context.Name + ".bin";
            File.WriteAllText(file, this.ToJson(this.dict));
        }

        private Dictionary<string, string> LoadFromStore()
        {
            var d = new Dictionary<string, string>();
            string file = this.context.Path + this.context.Name + ".bin";
            if (File.Exists(file))
            {
                var t = this.FromJson<Dictionary<string, string>>(File.ReadAllText(file));
                if (t != null)
                    d = t;
            }

            return d;
        }

        private string ToJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        private T FromJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }

    public class Context
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
