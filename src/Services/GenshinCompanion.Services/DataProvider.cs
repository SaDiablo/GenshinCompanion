using GenshinCompanion.CoreStandard;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GenshinCompanion.Services
{
    public class DataProvider
    {
        public static async Task<T> Open<T>(string fileName, DataFolder folder = DataFolder.Banners, DataFormat format = DataFormat.Json)
        {
            //TODO:
            //add checking for each DataFormat by default
            string folderPath = GetFolderPath(folder);
            string filePath = GetFilePath(fileName, folderPath, format);

            string[] files = null;
            if (Directory.Exists(folderPath))
            {
                files = Directory.GetFiles(folderPath);
            }

            T deserializedData;
            if (files != null && files.Contains(filePath, StringComparer.OrdinalIgnoreCase))
            {
                using (FileStream openStream = File.OpenRead(filePath))
                {
                    deserializedData = await JsonSerializer.DeserializeAsync<T>(openStream);
                }

                return deserializedData;
            }

            return default;
        }

        public static async Task<bool> Save(object objectToSave, string fileName, DataFolder folder = DataFolder.Banners, DataFormat format = DataFormat.Json)
        {
            //TODO:
            //DONE - Don't hardcode path, check earlier for availability to save in folder
            //To make it nondestructive to older saves
            //Keep backups and check differences and then save
            //DONE ~ Do it asynchronously
            //DONE ~ make it format agnostic
            //DONE - make it platform agnostic
            //DONE - make the format a enum to choose from

            if (objectToSave != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string folderPath = GetFolderPath(folder);
                string filePath = GetFilePath(fileName, folderPath, format);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (FileStream fileStream = File.Create(filePath))
                {
                    await JsonSerializer.SerializeAsync(fileStream, objectToSave, options);
                    await fileStream.DisposeAsync();
                }

                return true;
            }

            return false;
        }

        private static string GetFolderPath(DataFolder folder)
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                PropertyNames.ApplicationName,
                folder.ToString());
        }

        private static string GetFilePath(string fileName, string folderPath, DataFormat format)
        {
            return Path.Combine(
                folderPath,
                $"{fileName}.{format.ToString().ToLowerInvariant()}");
        }
    }
}
