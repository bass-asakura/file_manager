namespace file_manager;

class Program
{
    static void Main(string[] args)
    {   
        StartProgramm();
    }

    static void StartProgramm()
    {
        int row = Console.CursorTop;
        int col = Console.CursorLeft;
        int index = 0;

        try
        {
            while(true)
            {
                Console.WriteLine("__");
                var last = File.ReadAllText(@"C:\vs code\file_manager\lastsession");  // переменная в которой хранится последняя сесссия
                DrawMenu(last.Split("\n"), row, col, index);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        if (index < paths.Length - 1) index++;
                        break;

                    case ConsoleKey.UpArrow:
                        if (index > 0) index--;
                        break;
                    
                    case ConsoleKey.Q: return;

                    case ConsoleKey.Enter:
                        break;
                }
                
                // var vvod = Console.ReadLine() ?? "";  // запрашиваем у пользователя команду
                
                // switch (vvod.ToLower())
                // { 
                //     case "goto": GoTo(); break;

                //     case "copy": copy(); break;

                //     case "delete": delete(); break;

                //     case "info": info(); break;

                //     case "quit": return;        // команда для выхода  

                //     default: Console.WriteLine("Ошибка"); break;          // обработка неверного ввода команды
                // }
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
            Console.WriteLine("Неверный аргумент");
            StartProgramm();
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа");
            StartProgramm();
        }
    }

    static void info()
    {
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

        else if (Directory.Exists(pathInfo))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathInfo);

            Console.WriteLine($"Имя папки - {dirInfo.Name}");
            Console.WriteLine($"Дата создания - {dirInfo.CreationTime}");
            Console.WriteLine($"Дата последнего изменения - {dirInfo.LastWriteTime}");
            Console.WriteLine($"Атрибуты - {dirInfo.Attributes}");
        }

        else Console.WriteLine("Ничего не найдено");
    }

    static void delete()
    {
        var pathDelete = Console.ReadLine() ?? "";

        if (File.Exists(pathDelete))
        {
            File.Delete(pathDelete);
        }
        
        if (Directory.Exists(pathDelete))
        {
            CleanDirectory(pathDelete);     // удаляем все из папки
            Directory.Delete(pathDelete);   // удаляем папку
        }
        
        WriteInFile(Path.GetDirectoryName(pathDelete));     // обновляем информацию в файле
    }

    static void copy()
    {
        var pathSourse = Console.ReadLine() ?? "";
        var pathDestination = Console.ReadLine() ?? "";

        if (File.Exists(pathSourse))
        {
            File.Copy(pathSourse, Path.Combine(pathDestination, Path.GetFileName(pathSourse)));
        } 

        if (Directory.Exists(pathSourse))
        {
            CopyDirectory(pathSourse, Path.Combine(pathDestination, Path.GetFileName(pathSourse)));
        }

        WriteInFile(Path.GetDirectoryName(pathSourse));    // обновляем информацию в файле
    }

    static void GoTo()
    {
        var pathDir = Console.ReadLine() ?? "";     
        WriteInFile(pathDir);            // записываем в файл последнюю сессию
    }

    static void WriteInFile(string? path)   // функция для записи в файл
    {
        if (path == null) return;       // чтобы не было warning CS8604

        var dirs = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);

        File.WriteAllText(@"C:\vs code\file_manager\lastsession", " ");      // очистка файла путем перезаписывания в него пустой строки

        foreach (string dir in dirs) File.AppendAllText(@"C:\vs code\file_manager\lastsession", dir + "\n");
        foreach (string file in files) File.AppendAllText(@"C:\vs code\file_manager\lastsession", file + "\n");
    }

    static void CopyDirectory(string pathSourse, string pathDestination)
    {  
        if (!Directory.Exists(pathDestination))     // создание папки, если ее нет
        {
            Directory.CreateDirectory(pathDestination);
        }
       
        foreach (string file in Directory.GetFiles(pathSourse))     // копирование файлов
        {
            File.Copy(file, Path.Combine(pathDestination, Path.GetFileName(file)));
        }

        foreach (string subDir in Directory.GetDirectories(pathSourse))     // рекурсивное копирование подпапок
        {
            CopyDirectory(subDir, Path.Combine(pathDestination, Path.GetFileName(subDir)));
        }
    }

    static void CleanDirectory(string path)
    {
        foreach (string file in Directory.GetFiles(path))   // удаляем все файлы из папки
        {
            File.Delete(file);
        }

        foreach (string dir in Directory.GetDirectories(path))      // удаляем файлы из подпапок и подпапки
        {
            CleanDirectory(dir);
            Directory.Delete(dir);
        }
    }

    private static void DrawMenu(string[] items, int row, int col, int index)
    {
        Console.SetCursorPosition(col, row);

        for (int i = 0; i < items.Length; i++)
        {
            if (i == index)
            {
                Console.BackgroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(items[i]);
            Console.ResetColor();
        }
        Console.WriteLine();
    }
}

