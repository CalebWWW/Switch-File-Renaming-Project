namespace WebSwitchFileRenamingWorking.Backend
{
    public class HelperFunctions
    {
        public HelperFunctions() { }

        /// <summary>
        /// Used to get into the desired file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ScanForFileName(string fileName, string path)
        {
            try
            {
                //Keep Entering folders until the correct folder is entered
                for (int searchIndex = 0; searchIndex < 3; searchIndex++)
                {
                    var finalPath = EnterFolder(path, fileName);
                    if (!finalPath.Equals("error")) return finalPath;
                    path = EnterFolder(path);
                }
                return "error";
            }
            catch
            {
                return "error";
            }
        }

        /// <summary>
        /// Tries to enter a chosen folder and returns "error" if it isn't found
        /// </summary>
        /// <param name="originalPath"></param>
        /// <param name="folderToEnter"></param>
        /// <returns></returns>
        public string EnterFolder(string originalPath, string folderToEnter)
        {
            string[] folderContents = Directory.GetFileSystemEntries(originalPath);

            foreach (string folder in folderContents)
            {
                if (folder.Equals($"{originalPath}\\{folderToEnter}"))
                    return folder;
            }
            return "error";
        }

        /// <summary>
        /// Enters the next folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public string EnterFolder(string path)
        {
            string[] internalFolders = Directory.GetFileSystemEntries(path);
            foreach (string folder in internalFolders)
            {
                if (!folder.Equals($"{path}\\README.txt"))
                    return folder;
            }
            throw new InvalidOperationException("Logfile cannot be read-only");
        }

        /// <summary>
        /// Creates a file path by appending the new string to the end of the path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="appendString"></param>
        /// <returns></returns>
        public string CreateFilePath(string path, string appendString)
        {
            var appendLength = appendString.Length;
            return $"{path.Substring(0, path.Length - appendLength)}{appendString}";
        }

        /// <summary>
        /// Checks to see if the file is present in this directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="expectedFile"></param>
        /// <returns></returns>
        public bool CheckToSeeIfThisIsCorrectDirectory(string path, string expectedFile)
        {
            string[] folderContents = Directory.GetFileSystemEntries(path);

            foreach (string folder in folderContents)
            {
                if (folder.Equals($"{path}\\{expectedFile}"))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Creates a path by combining path and fileToDelete and deletes the directory there
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileToDelete"></param>

        public void TryToDeleteAppend(string path, string fileToDelete)
        {
            try
            {
                Directory.Delete(CreateFilePath(path, fileToDelete), true);
            }
            catch { }
        }

        /// <summary>
        /// Attempts to delete a folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileToDelete"></param>
        public void TryToDelete(string path, string fileToDelete)
        {
            try
            {
                Directory.Delete($"{path}\\{fileToDelete}", true);
            }
            catch { }
        }
    }
}
