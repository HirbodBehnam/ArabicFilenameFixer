using System;
using System.Collections.Generic;
using System.IO;

namespace ArabicFilenameFixer
{
    internal class Program
    {
        struct FileInfo
        {
            public string Path;
            public bool IsDirectory;
        }
        static void Main()
        {
            foreach (FileInfo file in GetFiles(Environment.CurrentDirectory))
            {
                string newFilename = FixName(file.Path);
                if (newFilename != file.Path)
                {
                    try
                    {
                        Console.WriteLine(file.Path);
                        if (file.IsDirectory)
                            Directory.Move(file.Path, newFilename);
                        else
                            File.Move(file.Path, newFilename);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
            }
        }

        private static IEnumerable<FileInfo> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                yield return new FileInfo { Path = path, IsDirectory = true };
                path = FixName(path);
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                        queue.Enqueue(subDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                }
                if (files != null)
                    foreach (string file in files)
                        yield return new FileInfo { Path = file, IsDirectory = false };
            }
        }

        private static string FixName(string name)
        {
            return name.Replace('ي', 'ی').Replace('ك', 'ک');
        }
    }
}
