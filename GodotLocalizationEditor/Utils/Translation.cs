using System;
using System.Collections.Generic;
using System.Linq;

namespace GodotLocalizationEditor.Utils
{
    public class Translation
    {
        public string Key;
        public List<string> Translations;

        public Translation(string key, List<string> translations)
        {
            Key = key;
            Translations = translations;
        }

        public string ToLine() => Key + "," + string.Join(",", Translations.Select(x => x.Contains(',') ? '"' + x.Replace("\n", "\\n").Replace("\r", "") + '"' : x.Replace("\n", "\\n").Replace("\r", "")));

        public static Translation FromLine(string line)
        {
            var infos = line.Split(",");
            var translations = new List<string>(infos);
            translations.RemoveAt(0);
            var removeIds = new List<int>();
            for (var i = 0; i < translations.Count; i++)
            {
                if (translations[i].StartsWith('"') && !translations[i].EndsWith('"'))
                {
                    var baseId = i;
                    i++;
                    while (!translations[i].EndsWith('"'))
                    {
                        removeIds.Add(i);
                        translations[baseId] += "," + translations[i];
                        i++;
                    }

                    if (i != baseId)
                    {
                        removeIds.Add(i);
                        translations[baseId] += "," + translations[i];
                    }
                }
            }

            for (var i = 0; i < translations.Count; i++)
            {
                translations[i] = translations[i].Replace("\\n", "\n");
                if (translations[i].StartsWith('"') && translations[i].EndsWith('"'))
                    translations[i] = translations[i].Substring(1, translations[i].Length - 2);
            }

            for(var i = removeIds.Count - 1; i > -1; i--)
                translations.RemoveAt(removeIds[i]);
            
            return new Translation(infos[0], translations);
        }
    }
}
