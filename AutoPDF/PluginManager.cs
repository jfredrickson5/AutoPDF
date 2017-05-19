using AutoPDF.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoPDF
{
    class PluginManager
    {
        private static PluginManager instance;
        public static PluginManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PluginManager();
                }
                return instance;
            }
        }
        public List<IFileNameGenerator> FileNameGeneratorPlugins { get; private set; }
        public List<IInputParser> InputParserPlugins { get; private set; }
        public List<Type> SupportedPlugins
        {
            get
            {
                return new List<Type> {
                    typeof(IInputParser),
                    typeof(IFileNameGenerator)
                };
            }
        }

        private PluginManager()
        {
            FileNameGeneratorPlugins = new List<IFileNameGenerator>();
            InputParserPlugins = new List<IInputParser>();
            LoadPlugins();
        }

        public string PluginsString()
        {
            var output = "";
            foreach (var fileNamePlugin in FileNameGeneratorPlugins)
            {
                output += " " + fileNamePlugin.GetType().Name;
            }
            foreach (var inputParserPlugin in InputParserPlugins)
            {
                output += " " + inputParserPlugin.GetType().Name;
            }
            return output.Trim();
        }
        
        private string[] FindLibraries()
        {
            var pluginsPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] libraryFiles = Directory.GetFiles(pluginsPath).Where(f => Regex.IsMatch(f, @"AutoPDF\.Plugin\.(.+)\.dll")).ToArray();
            return libraryFiles;
        }

        private ICollection<Assembly> LoadPluginAssemblies()
        {
            var libraryFiles = FindLibraries();
            ICollection<Assembly> assemblies = new List<Assembly>(libraryFiles.Length);
            foreach (var libraryFile in libraryFiles)
            {
                var name = AssemblyName.GetAssemblyName(libraryFile);
                var assembly = Assembly.Load(name);
                assemblies.Add(assembly);
            }
            return assemblies;
        }
        
        private List<Type> FindPlugins()
        {
            List<Type> foundPlugins = new List<Type>();
            var assemblies = LoadPluginAssemblies();
            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                foreach (var assemblyType in assemblyTypes)
                {
                    if (assemblyType.IsInterface || assemblyType.IsAbstract)
                    {
                        continue;
                    }
                    else
                    {
                        foreach (var supportedPlugin in SupportedPlugins)
                        {
                            if (assemblyType.GetInterface(supportedPlugin.FullName) != null)
                            {
                                foundPlugins.Add(assemblyType);
                            }
                        }
                    }
                }
            }
            return foundPlugins;
        }

        private void LoadPlugins()
        {
            var plugins = FindPlugins();
            foreach (var plugin in plugins)
            {
                if (plugin.GetInterface("IInputParser") != null)
                {
                    var instance = (IInputParser)Activator.CreateInstance(plugin);
                    InputParserPlugins.Add(instance);
                }
                if (plugin.GetInterface("IFileNameGenerator") != null)
                {
                    var instance = (IFileNameGenerator)Activator.CreateInstance(plugin);
                    FileNameGeneratorPlugins.Add(instance);
                }
            }
        }
    }
}
