using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahuaRenamer
{
    static class Program
    {
        private static void Main(string[] args)
        {
            var buildNumber = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER", EnvironmentVariableTarget.Process);  
            var directories = Directory.GetDirectories(".", "MahuaEvents", SearchOption.AllDirectories);
            if (directories.Length != 1) throw new FileNotFoundException("We could not find one 'MahuaEvents' folder.");
            if (buildNumber is null) throw new NotSupportedException("Please run this program in AppVeyor environment.");

            var classNames = Directory.GetFiles(directories.First()).Select(Path.GetFileNameWithoutExtension).ToArray();
            var files = Directory.GetFiles(".","*.cs", SearchOption.AllDirectories);
            for (var i = 0; i < files.Length; i++)
            {
                var file = files[i];
                Console.WriteLine($"[{i}/{files.Length}] Processing: {Path.GetFileName(file)}");
                var content = File.ReadAllText(file);
                content = classNames.Aggregate(content, (current, name) => current.Replace(name, $"{name}_{buildNumber}"));
                File.WriteAllText(file, content, Encoding.UTF8);
            }

            Console.WriteLine("Operation finished.");
        }
    }
}
