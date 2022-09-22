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
        private static string _pathToFileWindowData = Path.Combine(_pathToDirectory, "data.json");
        private static string _pathToFileApplicationData = Path.Combine(_pathToDirectory, "application.json");

        public static void SaveWindowsData(List<WindowInformation> windowInformations)
        {
            var jsonData = JsonConvert.SerializeObject(windowInformations);

            if (!Directory.Exists(_pathToDirectory))
            {
                Directory.CreateDirectory(_pathToDirectory);
            }

            File.WriteAllText(_pathToFileWindowData, jsonData);
        }

        public static List<WindowInformation> RestoreWindowsData()
        {
            if (!File.Exists(_pathToFileWindowData))
            {
                return new List<WindowInformation>();
            }

            var jsonData = File.ReadAllText(_pathToFileWindowData);

            var data = JsonConvert.DeserializeObject<List<WindowInformation>>(jsonData);

            return data;
        }

        public static void SaveApplicationData(bool autoRestoreState)
        {
            ApplicationInformation applicationInformation = new ApplicationInformation()
            {
                AutoRestoreState = autoRestoreState
            };

            var jsonData = JsonConvert.SerializeObject(applicationInformation);

            if (!Directory.Exists(_pathToDirectory))
            {
                Directory.CreateDirectory(_pathToDirectory);
            }

            File.WriteAllText(_pathToFileApplicationData, jsonData);
        }

        public static ApplicationInformation RestoreApplicationData()
        {
            if (!File.Exists(_pathToFileApplicationData))
            {
                return new ApplicationInformation();
            }

            var jsonData = File.ReadAllText(_pathToFileApplicationData);

            var data = JsonConvert.DeserializeObject<ApplicationInformation>(jsonData);

            return data;
        }
    }
}
