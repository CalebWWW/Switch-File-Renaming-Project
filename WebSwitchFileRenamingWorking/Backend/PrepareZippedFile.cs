using System.IO.Compression;
using WebSwitchFileRenamingWorking.Backend;

namespace WebSwitchFileRenamingWorking.Backend
{
    public class PrepareZippedFile
    {

        public string destinationFolder = "C:\\Users\\dutch\\Desktop\\Switch Modding\\SwitchRenamingProj";
        public string Log = "";

        public string BeginReplacementProcess(string fileName)
        {
            var fileRenamer = new FileRenamer();
            var log = "";

            if (!fileName.Contains("zip")) fileName = fileName + ".zip";
            var filePath = $"C:\\Users\\dutch\\Desktop\\Switch Modding\\SwitchRenamingZip\\{fileName}";

            if (!UnzipFile(filePath))
                return Log;

            filePath = $"C:\\Users\\dutch\\Desktop\\Switch Modding\\SwitchRenamingProj";

            if (UserPreferences.ReplaceUi)
            {
                if (fileRenamer.ReplaceUiFiles(filePath))
                    log += " UI file renaming was a success";
                else
                    log += " Ui file renaming failed";
            }
            if (UserPreferences.ReplaceFighter)
            {
                if (fileRenamer.ReplaceFighterFiles(filePath))
                    log += " Fighter file renaming was a success";
                else
                    log += " Fighter file renaming failed";
            }
            if (UserPreferences.ReplaceJson)
            {
                if (fileRenamer.ReplaceJsonFiles(filePath))
                    log += " Json file renaming was a success";
                else
                    log += " Json file renaming failed";
            }

            return log;
        }

        public string BeginCheckProcess(string fileName)
        {
            var fileChecker = new FileChecker();
            var log = "";

            if (!fileName.Contains("zip")) fileName = fileName + ".zip";
            var filePath = $"C:\\Users\\dutch\\Desktop\\Switch Modding\\SwitchRenamingZip\\{fileName}";

            if (!UnzipFile(filePath))
                return Log;

            filePath = $"C:\\Users\\dutch\\Desktop\\Switch Modding\\SwitchRenamingProj";

            log = fileChecker.Check(filePath);

            return log;
        }

            public bool UnzipFile(string zipFilePath)
        {
            try
            {
                string[] destinationStaleFiles = Directory.GetFileSystemEntries(destinationFolder);
                foreach (string existingFolder in destinationStaleFiles)
                {
                    try
                    {
                        Directory.Delete(existingFolder, true);
                    } catch
                    { 
                        //If the directory fiails to delete, it must be the config or a read me
                        File.Delete(existingFolder);
                    }
                }

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
    }
}
