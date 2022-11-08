using System;
using System.Collections.Generic;
using System.IO;

namespace GodotLocalizationEditor.Utils
{
    public class CSVFile
    {
        public string Name;
        public string Path;
        public List<string> Langs;
        public List<Translation> Translations;

        public CSVFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);

            var lines = File.ReadAllLines(path);
            if (lines.Length == 0)
                throw new Exception("File empty");
            
            var langs = lines[0].Split(",");
            Langs = new List<string>(langs);
            Langs.RemoveAt(0);
            Translations = new List<Translation>();
            for (var i = 1; i < lines.Length; i++)
                Translations.Add(Translation.FromLine(lines[i]));
        }

        public CSVFile(string path, string[] langs)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Langs = new List<string>(langs);
            Translations = new List<Translation>();
        }

        public void Save()
        {
            var lines = new List<string>();
            lines.Add("keys," + string.Join(",", Langs));
            foreach (var translation in Translations)
                lines.Add(translation.ToLine());
            File.WriteAllLines(Path, lines);
        }
    }
}
