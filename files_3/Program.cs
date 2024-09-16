using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к папке, которую нужно очистить:");
        string directoryPath = Console.ReadLine();

        if (Directory.Exists(directoryPath))
        {
            TimeSpan maxAge = TimeSpan.FromMinutes(30);

            long initialSize = GetDirectorySize(directoryPath);
            Console.WriteLine($"Размер папки до очистки: {initialSize} байт");

            CleanDirectory(directoryPath, maxAge);

            long finalSize = GetDirectorySize(directoryPath);
            Console.WriteLine($"Размер папки после очистки: {finalSize} байт");

            long freedSpace = initialSize - finalSize;
            Console.WriteLine($"Освобождено места: {freedSpace} байт");
        }
        else
        {
            Console.WriteLine("Указанная папка не существует.");
        }
    }

    static void CleanDirectory(string path, TimeSpan maxAge)
    {
        try
        {
            var now = DateTime.Now;

            foreach (string filePath in Directory.GetFiles(path))
            {
                DateTime lastAccessTime = File.GetLastAccessTime(filePath);
                if (now - lastAccessTime > maxAge)
                {
                    Console.WriteLine($"Удаление файла: {filePath}");
                    File.Delete(filePath);
                }
            }

            foreach (string dirPath in Directory.GetDirectories(path))
            {
                DateTime lastAccessTime = Directory.GetLastAccessTime(dirPath);
                if (now - lastAccessTime > maxAge)
                {
                    Console.WriteLine($"Удаление папки: {dirPath}");
                    Directory.Delete(dirPath, true);
                }
                else
                {
                    CleanDirectory(dirPath, maxAge);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    static long GetDirectorySize(string directoryPath)
    {

        long totalSize = 0;

        DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
        FileInfo[] files = dirInfo.GetFiles();
        foreach (FileInfo file in files)
        {
            totalSize += file.Length;
        }

        DirectoryInfo[] directories = dirInfo.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            totalSize += GetDirectorySize(directory.FullName);
        }

        return totalSize;
    }
}
