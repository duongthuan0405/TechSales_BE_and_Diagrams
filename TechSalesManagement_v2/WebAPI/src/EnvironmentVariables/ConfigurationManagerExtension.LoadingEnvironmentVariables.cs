namespace WebAPI.Extension;

public static partial class ConfigurationManagerExtension {
    internal static IConfiguration LoadEnvironmentVariables(this ConfigurationManager configuration, string folderName)
    {
        DirectoryInfo dir = new DirectoryInfo(folderName);
        foreach(FileInfo f in dir.GetFiles())
        {
            DotNetEnv.Env.Load(f.FullName);
        }
        configuration.AddEnvironmentVariables();
        return configuration;
    }
}