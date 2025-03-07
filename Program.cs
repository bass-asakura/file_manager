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
                Console.WriteLine("__");
                var last = File.ReadAllText(@"C:\vs code\file_manager\lastsession");  // переменная в которой хранится последняя сесссия
                Console.WriteLine(last);

                var vvod = Console.ReadLine() ?? "";  // запрашиваем у пользователя команду

                switch (vvod.ToLower())
                { 
                    case "goto":

                        var pathDir = Console.ReadLine() ?? "";     
                        WriteInFile(pathDir);
                        break;

                    case "copy":

                        var pathSourse = Console.ReadLine() ?? "";
                        var pathDestination = Console.ReadLine() ?? "";

                        if (File.Exists(pathSourse))
                        {
                            File.Copy(pathSourse, Path.Combine(pathDestination, Path.GetFileName(pathSourse)));
                        } 

                        else if (Directory.Exists(pathSourse))
                        {
                            CopyDirectory(pathSourse, Path.Combine(pathDestination, Path.GetFileName(pathSourse)));
                        }
                        break;

                    case "delete":
                        var pathDelete = Console.ReadLine() ?? "";

                        if (File.Exists(pathDelete))
                        {
                            File.Delete(pathDelete);
                        }
                        
                        if (Directory.Exists(pathDelete))
                        {
                            foreach (string file in Directory.GetFiles(pathDelete)) File.Delete(file);
                            Directory.Delete(pathDelete);
                        }
                        
                        WriteInFile(Path.GetDirectoryName(pathDelete));
                        break;
                    
                    case "info":
                        var pathInfo = Console.ReadLine() ?? "";

                        if (File.Exists(pathInfo))
                        {
                            FileInfo fileInfo = new FileInfo(pathInfo);

                            Console.WriteLine($"Имя файла - {fileInfo.Name}");
                            Console.WriteLine($"Размер файла - {fileInfo.Length} байт");
                            Console.WriteLine($"Дата создания - {fileInfo.CreationTime}");
                            Console.WriteLine($"Дата последнего изменения - {fileInfo.LastWriteTime}");
                            Console.WriteLine($"Атрибуты файла - {fileInfo.Attributes}");
                        }

                        if (Directory.Exists(pathInfo))
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(pathInfo);

                            Console.WriteLine($"Имя папки - {dirInfo.Name}");
                            Console.WriteLine($"Дата создания - {dirInfo.CreationTime}");
                            Console.WriteLine($"Дата последнего изменения - {dirInfo.LastWriteTime}");
                            Console.WriteLine($"Атрибуты - {dirInfo.Attributes}");
                        }
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
        catch (ArgumentException)
        {
            StartProgramm();
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа");
            StartProgramm();
        }
    }

    static void WriteInFile(string? path)
    {
        if (path == null) return;

        var dirs = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);

        File.WriteAllText(@"C:\vs code\file_manager\lastsession", "");      // очистка файла путем перезаписывания в него пустой строки

        foreach (string dir in dirs) File.AppendAllText(@"C:\vs code\file_manager\lastsession", dir + "\n");
        foreach (string file in files) File.AppendAllText(@"C:\vs code\file_manager\lastsession", file + "\n");
    }

    static void CopyDirectory(string pathSourse, string pathDestination)
    {  
        if (!Directory.Exists(pathDestination))
        {
            Directory.CreateDirectory(pathDestination);
        }
       
        foreach (string file in Directory.GetFiles(pathSourse))
        {
            File.Copy(file, Path.Combine(pathDestination, Path.GetFileName(file)));
        }

        foreach (string subDir in Directory.GetDirectories(pathSourse))
        {
            CopyDirectory(subDir, Path.Combine(pathDestination, Path.GetFileName(subDir)));
        }
    }
}

