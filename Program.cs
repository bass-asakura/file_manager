namespace file_manager;

class Program
{
    static void Main(string[] args)
    {   
        StartProgramm();
    }

    static void StartProgramm()
    {
        int row = Console.CursorTop;    // текущая строка курсора в консоли
        int col = Console.CursorLeft;   // текущая колонка курсора в консоли
        int index = 0;                  // индекс выбранного элемента
        string pathSourse = "";         // нужна для копирования
        bool flagCopy = false;          // нужна чтобы клавиша V не работала без C

        try
        {
            while(true)
            {
                Console.WriteLine(" ");
                var last = File.ReadAllText(@"C:\vs code\file_manager\lastsession");  // переменная в которой хранится последняя сесссия

                string[] paths = last.Split("\n");      // список путей для отрисовки
                DrawMenu(paths, row, col, index);

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Q: return;      
                    case ConsoleKey.DownArrow:                        // перемещение путем изменения индекса
                        if (index < paths.Length - 2) index++;      
                        break;
                    case ConsoleKey.UpArrow:                         // перемещение путем изменения индекса
                        if (index > 0) index--;
                        break;
                    case ConsoleKey.G:
                        Console.Clear();

                        if (Directory.GetFiles(paths[index]).Length == 0 && Directory.GetDirectories(paths[index]).Length == 0)     // проверка на то пуста ли папка
                        {                                                                                                           // если пуста то выводим только путь к папке
                            if ($"{last[last.Length - 1]}" != @"\")         // проверка чтобы в консоли не выводилось больше одного "\" 
                            {
                                File.WriteAllText(@"C:\vs code\file_manager\lastsession", paths[index] + @"\");     // добавление "\" нужно для корректной работы клавиши B
                            }
                            else
                            {
                                File.WriteAllText(@"C:\vs code\file_manager\lastsession", paths[index]);
                            }
                        }
                        else
                        {
                            WriteInFile(paths[index]);                                                          // если не пуста то выводим ее содержимое
                        }
                        index = 0;          // здесь и в последующих случаях нужно, чтобы при переходе в папку курсор был на первом элементе
                        break;
                     
                    case ConsoleKey.B:

                        Console.Clear();
                        WriteInFile(Path.GetDirectoryName(Path.GetDirectoryName(paths[index])));        // возвращение назад путем записи в файл предыдущей папки
                        index = 0;
                        break;

                    case ConsoleKey.I:
                        info(paths[index]);
                        break;
                    
                    case ConsoleKey.D:
                        Console.Clear();
                        delete(paths[index]);

                        string? pathDelete = Path.GetDirectoryName(paths[index]);       
                        if (pathDelete == null) return;                               // чтобы не было warning CS8604

                        if (Directory.GetFiles(pathDelete).Length == 0 & Directory.GetDirectories(pathDelete).Length == 0)    // если папка пуста после удаления 
                        {                                                                                                     // то записываем в файл путь к ней
                            File.WriteAllText(@"C:\vs code\file_manager\lastsession", Path.GetDirectoryName(paths[index]) + @"\");
                        }
                        else
                        {
                            WriteInFile(Path.GetDirectoryName(paths[index]));       // иначе записываем ее содержимое
                        }
                        break;

                    case ConsoleKey.P:
                        var path = Console.ReadLine() ?? "";        
                        WriteInFile(path);
                        Console.Clear();
                        break;
                    case ConsoleKey.C:
                        pathSourse = paths[index];      
                        flagCopy = true;            
                        break;

                    case ConsoleKey.V:
                        if (flagCopy)
                        {
                            string pathDestination = paths[index];
                            copy(pathSourse, pathDestination);
                        }
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
            Console.WriteLine("Неверный аргумент");
            StartProgramm();
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Нет доступа");
            StartProgramm();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Неизвестная ошибка" + ex.Message);
        }
    }

    static void info(string pathInfo)
    {
        Console.WriteLine(" ");
        if (File.Exists(pathInfo))      // проверяем является ли путь файлом
        {
            FileInfo fileInfo = new FileInfo(pathInfo);

            Console.WriteLine($"Имя файла - {fileInfo.Name}");
            Console.WriteLine($"Размер файла - {fileInfo.Length} байт");
            Console.WriteLine($"Дата создания - {fileInfo.CreationTime}");
            Console.WriteLine($"Дата последнего изменения - {fileInfo.LastWriteTime}");
            Console.WriteLine($"Атрибуты файла - {fileInfo.Attributes}");
        }

        else if (Directory.Exists(pathInfo))  // проверяем является ли путь директорией
        {
            DirectoryInfo dirInfo = new DirectoryInfo(pathInfo);

            Console.WriteLine($"Имя папки - {dirInfo.Name}");
            Console.WriteLine($"Дата создания - {dirInfo.CreationTime}");
            Console.WriteLine($"Дата последнего изменения - {dirInfo.LastWriteTime}");
            Console.WriteLine($"Атрибуты - {dirInfo.Attributes}");
        }

        else Console.WriteLine("Ничего не найдено");  // неверный ввод
    }

    static void delete(string pathDelete)
    {
        if (File.Exists(pathDelete))        // проверяем является ли путь файлом
        {
            File.Delete(pathDelete);
        }
        
        if (Directory.Exists(pathDelete))       // проверяем является ли путь директорией
        {
            CleanDirectory(pathDelete);     // удаляем все из папки
            Directory.Delete(pathDelete);   // удаляем папку
        }
    }

    static void copy(string pathSourse, string pathDestination)
    {
        if (File.Exists(pathSourse))        // проверяем является ли путь файлом
        {
            File.Copy(pathSourse, Path.Combine(pathDestination, Path.GetFileName(pathSourse)));        // копируем содержимое файла 
        }                                                                                              // в только что созданый файл с таким же названием

        if (Directory.Exists(pathSourse))   // проверяем является ли путь директорией
        {
            CopyDirectory(pathSourse, Path.Combine(pathDestination, Path.GetFileName(pathSourse)));     // аналогично с файлом
        }
    }

    static void WriteInFile(string? path)   // функция для записи в файл
    {
        if (path == null) return;       // чтобы не было warning CS8604

        var dirs = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);

        File.WriteAllText(@"C:\vs code\file_manager\lastsession", "");      // очистка файла путем перезаписывания в него пустой строки

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

    static void CleanDirectory(string path)     // нужна для удаления папки
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
                Console.BackgroundColor = Console.ForegroundColor;      // отображения места где находится курсор
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(items[i]);     
            Console.ResetColor();
        }
        Console.WriteLine();
    }
}

