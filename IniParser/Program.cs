using IniParser;
using System.Reflection;

Console.Title = "Stern's Ini Parser";

Console.WriteLine("------------------------------------------------------------");
Console.WriteLine("Initializing...");
Console.WriteLine("------------------------------");

Console.WriteLine("Please put your ini file path here:");

var iniFile = Console.ReadLine();
var parser = new ParseUtils(iniFile);

Console.WriteLine("Select parse action.  Read Key = 0, Write Key = 1, Delete Key = 2, Delete Section = 3, Key Exists = 4");
var action = Console.ReadLine();

var key = "";
if (action != "3")
{
    Console.WriteLine("Please put value key here:");
    key = Console.ReadLine();
}

var section = "";
Console.WriteLine("Want select a section or leave it default ('Main').  select = 0, leave it = 1");
var answer = Console.ReadLine();

if (answer == "0")
{
    Console.WriteLine("Please put a section here:");
    section = Console.ReadLine();
}

switch (action)
{
    case "0":
        string readedValue = parser.Read(key, section);
        Console.WriteLine("Readed Value = '{0}'", readedValue);
        break;
    case "1":
        Console.WriteLine("Please put a value here:");
        var value = Console.ReadLine();
        parser.Write(key, value, section);
        Console.WriteLine("Successfully Wrote. {0}: '{1}'", key, value);
        break;
    case "2":
        parser.DeleteKey(key);
        Console.WriteLine("Successfully Deleted. '{0}'", key);
        break;
    case "3":
        parser.DeleteSection(section);
        Console.WriteLine("Successfully Deleted. '{0}'", section);
        break;
    case "4":
        var exists = parser.KeyExists(key, section);
        if (exists)
            Console.WriteLine("{0} exists", key);
        else
            Console.WriteLine("{0} doesn't exists", key);
        break;
}

Console.WriteLine("Want try another action?  yes = 0, no = 1");
var refresh = Console.ReadLine() == "0";
if (refresh)
{
    System.Diagnostics.Process.Start(Assembly.GetExecutingAssembly().GetName().Name);
    Environment.Exit(0);
}