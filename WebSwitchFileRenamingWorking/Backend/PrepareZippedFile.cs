using System.IO.Compression;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace WebSwitchFileRenamingWorking.Backend
{
    public class PrepareZippedFile
    {

        public string destinationFolder = $"{Directory.GetCurrentDirectory()}\\AdjModdingDirectory\\ModdingOutputFolder";
        public string baseFolder = $"{Directory.GetCurrentDirectory()}\\AdjModdingDirectory\\ModdingInputZippedFilesHere";
        public string Log = "";
        public bool IsRarFile = UserPreferences.IsRarFile;

        public string BeginReplacementProcess(string fileName)
        {
            var fileRenamer = new FileRenamer();

            if (!PrepareFiles(fileName)) return Log;
            var filePath = destinationFolder;

            if (UserPreferences.ReplaceUi)
            {
                if (fileRenamer.ReplaceUiFiles(filePath))
                    Log += " UI file renaming was a success";
                else
                    Log += " Ui file renaming failed";
            }
            if (UserPreferences.ReplaceFighter)
            {
                if (fileRenamer.ReplaceFighterFiles(filePath))
                    Log += " Fighter file renaming was a success";
                else
                    Log += " Fighter file renaming failed";
            }
            if (UserPreferences.ReplaceJson)
            {
                if (fileRenamer.ReplaceJsonFiles(filePath))
                    Log += " Json file renaming was a success";
                else
                    Log += " Json file renaming failed";
            }

            return Log;
        }

        public string BeginCheckProcess(string fileName)
        {
            var fileChecker = new FileChecker();

            if (!PrepareFiles(fileName)) return Log;
            var filePath = destinationFolder;

            Log = fileChecker.Check(filePath);
            return Log;
        }

        public bool PrepareFiles(string fileName)
        {
            fileName = IsRarFile ? fileName.Contains("rar") 
                ? fileName : fileName + ".rar"
                : fileName.Contains(".zip") ? fileName : fileName + ".zip";
            var filePath = $"{baseFolder}\\{fileName}";
            if (IsRarFile)
            {
                if (!UnzipRarFile(filePath))
                    return false;
            }
            else
            {
                if (!UnzipFile(filePath))
                    return false;
            }
            return true;
        }

        public bool UnzipFile(string zipFilePath)
        {
            DeleteAllFilesInDestination();
            try
            {
                ZipFile.ExtractToDirectory(zipFilePath, destinationFolder);
                return true;
            }
            catch (InvalidDataException invalidDataEx)
            {
                Console.WriteLine("Invalid data error: " + invalidDataEx.Message);
                Log += " The file path is incorrect, INVALID DATA ERROR";
            }
            catch (IOException ioEx)
            {
                Console.WriteLine("IO error: " + ioEx.Message);
                Log += " This file already exists, IO ERROR";
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while unzipping: " + ex.Message);
                Log += " Unknown unzipping error";
            }
            return false;
        }

        public bool UnzipRarFile(string zipFilePath)
        {
            DeleteAllFilesInDestination();
            try
            {
                // Open the RAR file for extraction
                var archive = ArchiveFactory.Open(zipFilePath);
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    // Extract the entry
                    entry.WriteToDirectory(destinationFolder, new ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
                return true;
            } catch (Exception) 
            {
                return false;
            }
        }

        public void DeleteAllFilesInDestination()
        {
            string[] destinationStaleFiles = Directory.GetFileSystemEntries(destinationFolder);
            foreach (string existingFolder in destinationStaleFiles)
            {
                try
                {
                    Directory.Delete(existingFolder, true);
                }
                catch
                {
                    //If the directory fiails to delete, it must be the config or a read me
                    File.Delete(existingFolder);
                }
            }
        }
    }
}
