namespace WebSwitchFileRenamingWorking.Backend
{
    public class FileChecker
    {
        private string log = string.Empty;
        public string Check(string path)
        {
            var help = new HelperFunctions();

            log = string.Empty;
            FindAllFilesInBaseDirectory(path, help);
            FindTheNameOfCurrentFiles(path, help);
            return log;
        }

        public void FindTheNameOfCurrentFiles(string path, HelperFunctions help)
        {
            log += $"The current path is: {help.FindExistingFileDirectory(path)} \n";
        }

        public void FindAllFilesInBaseDirectory(string path, HelperFunctions help)
        {
            for (int i = 0; i < 3; i++)
            {
                var isFilePresent = help.CheckToSeeIfThisIsCorrectDirectory(path, "fighter");
                path = isFilePresent ? path : help.EnterFolder(path);
                if (isFilePresent) break;
            }

            log += "Files in base directory include: ";
            string[] folderContents = Directory.GetFileSystemEntries(path);
            foreach (string folder in folderContents)
            {
                var folderName = folder.Substring(folder.LastIndexOf('\\'));
                log += folderName + ", ";
            }

            log += "\n";
        }
    }
}
