namespace file_manager;

class Program
{
    static void Main(string[] args)
    {   
        StartProgramm();
    }

    static void StartProgramm()
    {
        while(true)
        {
            try
            {
                var vvod = Console.ReadLine() ?? ""; // запрашиваем у пользователя команду

                switch (vvod.ToLower())
                { 
                    case "goto":

                        var pathDir = Console.ReadLine() ?? "";
                        var dirs = Directory.GetDirectories(pathDir);
                        var files = Directory.GetFiles(pathDir);

                        foreach (string dir in dirs) Console.WriteLine(dir);
                        foreach (string file in files) Console.WriteLine(file);
                        break;

                    case "copy":

                        var pathSourse = Console.ReadLine() ?? "";
                        var pathDestination = Console.ReadLine() ?? "";
                        
                        File.Copy(pathSourse, pathDestination);
                        break;

                    case "delete":

                        var pathDelete = Console.ReadLine() ?? "";
                        File.Delete(pathDelete);
                        break; 

                    case "quit":
                        return;   
                    
                    default:
                        Console.WriteLine("Ошибка");
                        break;
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
            }
        }
    }
}
