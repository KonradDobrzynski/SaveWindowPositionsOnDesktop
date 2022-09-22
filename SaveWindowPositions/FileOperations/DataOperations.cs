using Newtonsoft.Json;
using SaveWindowPositions.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using static SaveWindowPositions.ExternalItems.ExternalDeclarations;

namespace SaveWindowPositions.FileOperations
{
    public static class DataOperations
    {
        private static string _pathToDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SaveChromeWindowsApp");
        private static string _pathToFile = Path.Combine(_pathToDirectory, "data.json");

        public static void Save(List<WindowInformation> windowInformations)
        {
            var jsonData = JsonConvert.SerializeObject(windowInformations);

            if (!Directory.Exists(_pathToDirectory))
            {
                Directory.CreateDirectory(_pathToDirectory);
            }

            if (!File.Exists(_pathToFile))
            {
                File.Create(_pathToFile);
            }

            File.WriteAllText(_pathToFile, jsonData);
        }

        public static List<WindowInformation> Restore()
        {
            var jsonData = File.ReadAllText(_pathToFile);

            var data = JsonConvert.DeserializeObject<List<WindowInformation>>(jsonData);

            return data;
        }
    }
}
