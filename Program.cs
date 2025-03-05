namespace file_manager;

class Program
{
    static void Main(string[] args)
    {
        StartProgramm();
        Directory.CreateDirectory(@"C:\test_for_filemanager" + @"\new_folder_1");
    }

    static void StartProgramm()
    {
        Console.WriteLine("1. Посмотреть файлы");
        Console.WriteLine("2. Копировать файл");
        Console.WriteLine("3. Удалить файл");
        var vvod = Console.ReadLine() ?? "";

        try
        {
            switch (vvod)
            {
                case "1":
                    Console.WriteLine("Введите путь");
                    var path = Console.ReadLine() ?? "";

                    var dirs = Directory.GetDirectories(path);
                    var files = Directory.GetFiles(path);

                    foreach (string dir in dirs) Console.WriteLine(dir);
                    foreach (string file in files) Console.WriteLine(file);
                    break;
                    
                case "2":
                    Console.WriteLine("Введите путь к файлу");
                    var pathFile = Console.ReadLine() ?? "";
                    
                    Console.WriteLine("Введите путь к папке, в котторую хотите скопировать");
                    var pathDir = Console.ReadLine() ?? "";
                    
                    File.Copy(pathFile, pathDir);
                    break;

                case "3":
                    break;    
                
                default:
                    Console.WriteLine("ti daun");
                    break;
            }
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Файл не найден");
        }
    }
}