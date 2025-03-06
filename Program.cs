namespace file_manager;

class Program
{
    static void Main(string[] args)
    {   
        StartProgramm();
    }

    static void StartProgramm()
    {
        try
        {
            while(true)
            {
                var last = File.ReadAllText(@"C:\vs code\file_manager\lastsession");  // переменная в которой хранится последняя сесссия
                Console.WriteLine(last);

                var vvod = Console.ReadLine() ?? "";  // запрашиваем у пользователя команду

                switch (vvod.ToLower())
                { 
                    case "goto":

                        var pathDir = Console.ReadLine() ?? "";     
                        var dirs = Directory.GetDirectories(pathDir);
                        var files = Directory.GetFiles(pathDir);

                        File.WriteAllText(@"C:\vs code\file_manager\lastsession", "");      // очистка файла путем перезаписывания в него пустой строки

                        foreach (string dir in dirs) 
                        {   
                            File.AppendAllText(@"C:\vs code\file_manager\lastsession", dir + "\n");
                        }

                        foreach (string file in files) 
                        {
                            File.AppendAllText(@"C:\vs code\file_manager\lastsession", file + "\n");
                        }

                        break;

                    case "copy":

                        var pathSourse = Console.ReadLine() ?? "";
                        var pathDestination = Console.ReadLine() ?? "";

                        if (File.Exists(pathSourse))
                        {
                            File.Copy(pathSourse, pathDestination);
                        }
                        if (Directory.Exists(pathSourse))
                        {
                            CopyDirectory(pathSourse, pathDestination);
                        }
                        break;

                    case "delete":

                        var pathDelete = Console.ReadLine() ?? "";
                        File.Delete(pathDelete);
                        break;
                    
                    case "info":
                        var pathInfo = Console.ReadLine() ?? "";
                        FileInfo fileInfo = new FileInfo(pathInfo);

                        Console.WriteLine($"Имя файла: {fileInfo.Name}");
                        Console.WriteLine($"Размер файла: {fileInfo.Length} байт");
                        Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
                        Console.WriteLine($"Дата последней модификации: {fileInfo.LastWriteTime}");
                        Console.WriteLine($"Дата последнего доступа: {fileInfo.LastAccessTime}");
                        Console.WriteLine($"Атрибуты файла: {fileInfo.Attributes}");
                        Console.WriteLine($"Расширение файла: {fileInfo.Extension}");
                        Console.WriteLine($"Только для чтения: {fileInfo.IsReadOnly}");
                        break;

                    case "quit":
                        return;   
                    
                    default:
                        Console.WriteLine("Ошибка");
                        break;
                }
            }
        }

        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Папка не найдена");
            StartProgramm();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл не найден");
            StartProgramm();
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
            StartProgramm();
        }
    }

    /// <summary>
    /// Рекурсивно копирует папку и её содержимое.
    /// </summary>
    /// <param name="sourceDir">Исходная папка.</param>
    /// <param name="destDir">Целевая папка.</param>

    static void CopyDirectory(string sourceDir, string destDir)
    {
        // Создаем целевую папку, если она не существует
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        // Копируем файлы
        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string fileName = Path.GetFileName(file);
            string destFile = Path.Combine(destDir, fileName);
            File.Copy(file, destFile, true); // true — разрешает перезапись
        }

        // Копируем подпапки
        foreach (string subDir in Directory.GetDirectories(sourceDir))
        {
            string subDirName = Path.GetFileName(subDir);
            string destSubDir = Path.Combine(destDir, subDirName);
            CopyDirectory(subDir, destSubDir); // Рекурсивно копируем подпапку
        }
    }
}
// скопировал функцию для копирования папок (надо разобраться)
// улучшить копирование файла, с импользованием Combine и GetFileName