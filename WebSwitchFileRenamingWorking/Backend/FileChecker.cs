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
            //Entering the fighter folder
            var fighterPath = help.ScanForFileName("fighter", path);
            if (fighterPath.Equals("error"))
            {
                log += "Fighter path error \n";
                return;
            }

            //Entering the character specific folder
            fighterPath = help.EnterFolder(fighterPath);

            //Entering Model Folder
            fighterPath = help.EnterFolder(fighterPath);

            //Entering first folder in the hub
            fighterPath = help.EnterFolder(fighterPath);
            string[] characterHubDirectories = Directory.GetFileSystemEntries(fighterPath);
            string currentPath = characterHubDirectories.First();
            
            var index = currentPath.LastIndexOf("c0");

            log += $"The current path is: {currentPath.Substring(index, 3)} \n";
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
