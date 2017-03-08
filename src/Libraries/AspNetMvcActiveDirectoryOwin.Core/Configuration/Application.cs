namespace AspNetMvcActiveDirectoryOwin.Core.Configuration
{
    public class Application
    {
        public Application(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; private set; }
        public string Version { get; private set; }
    }
}