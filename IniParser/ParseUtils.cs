using System.Runtime.InteropServices;
using System.Text;

namespace IniParser
{
    public class ParseUtils
    {
        string filePath;
        string main = "Main";

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public ParseUtils(string iniFile = "")
        {
            filePath = new FileInfo(iniFile == "" ? main + ".ini" : iniFile).FullName;
        }

        public Dictionary<string, string> ParseIniDataWithSections()
        {
            var iniData = File.ReadAllLines(filePath);
            var dictionary = new Dictionary<string, string>();
            var rows = iniData.Where(t => !String.IsNullOrEmpty(t.Trim()) && !t.StartsWith(";") && (t.Contains('[') || t.Contains('=')));
            if (rows == null || rows.Count() == 0) 
                return dictionary;
            string section = "";
            foreach (string row in rows)
            {
                string rw = row.TrimStart();
                if (rw.StartsWith("["))
                    section = rw.TrimStart('[').TrimEnd(']');
                else
                {
                    int index = rw.IndexOf('=');
                    dictionary[section + "-" + rw.Substring(0, index).Trim()] = rw.Substring(index + 1).Trim().Trim('"');
                }
            }
            return dictionary;
        }

        public string Read(string key, string section = "")
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section == "" ? main : section, key, "", retVal, 255, filePath);
            return retVal.ToString();
        }

        public List<string> Read(List<string> keys, string section = "")
        {
            var retVal = new StringBuilder(255);
            foreach (var key in keys)
            {
                GetPrivateProfileString(section == "" ? main: section, key, "", retVal, 255, filePath);
            }
            return new List<string> { retVal.ToString() };
        }

        public void Write(string key, string value, string section = "")
        {
            WritePrivateProfileString(section == "" ? main : section, key, value, filePath);
        }

        public void Write(List<string> keys, string value, string section = "")
        {
            foreach (var key in keys)
            {
                WritePrivateProfileString(section == "" ? main : section, key, value, filePath);
            }
        }

        public void DeleteKey(string key)
        {
            var fileContents = File.ReadAllText(filePath);
            fileContents = fileContents.Split("\n" + key)[0];
            File.WriteAllText(filePath, fileContents);
        }

        public void DeleteKeys(List<string> keys)
        {
            foreach (var key in keys)
            {
                var fileContents = File.ReadAllText(filePath);
                fileContents = fileContents.Split("\n" + key)[0];
                File.WriteAllText(filePath, fileContents);
            }
        }

        public void DeleteSection(string section = "")
        {
            Write("", "", section == "" ? main : section);
        }

        public void DeleteSections(List<string> sections = null)
        {
            foreach (var section in sections)
            {
                Write("", "", section == "" ? main : section);
            }
        }

        public bool KeyExists(string key, string section = "")
        {
            return Read(key, section).Length > 0;
        }
    }
}