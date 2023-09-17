namespace WebSwitchFileRenamingWorking.Backend
{
    public class FileRenamer
    {
        private bool isFighterReplacementSuccessful = false;
        private bool isUiReplacementSuccessful = false;
        public bool ReplaceFighterFiles(string path)
        {
            var help = new HelperFunctions();
            var c00Keep = UserPreferences.FileToKeep;
            var c00NewPosition = UserPreferences.LocationToMove;

            //Delete effect folder and rename sound file if present
            ReplaceSoundFiles(path);

            //Entering the fighter folder
            var fighterPath = help.ScanForFileName("fighter", path);
            if (fighterPath.Equals("error")) return false;

            //Entering the character specific folder
            fighterPath = help.EnterFolder(fighterPath);

            //Entering motion folder
            var motionPath = help.ScanForFileName("motion", path);

            if (!motionPath.Equals("error"))
            {
                string[] internalMotionDirectory = Directory.GetFileSystemEntries(motionPath);
                foreach (string modify in internalMotionDirectory)
                {
                    //Delete all undesired files
                    if (!modify.Contains(c00Keep) && !c00Keep.Equals("c0"))
                    {
                        Directory.Delete(modify, true);
                    }
                }

                //Rename the chosen file
                var remainingFilePath = c00Keep.Equals("c0")
                    ? internalMotionDirectory.First()
                    : help.CreateFilePath(internalMotionDirectory.First(), c00Keep);
                if (Directory.Exists(remainingFilePath))
                {
                    var newFolderPath = help.CreateFilePath(internalMotionDirectory.First(), c00NewPosition);
                    Directory.Move(remainingFilePath, newFolderPath);
                    isFighterReplacementSuccessful = true;
                }
            }

            //Entering Model Folder
            fighterPath = help.EnterFolder(fighterPath);

            //Entering each file in the hub
            string[] characterHubDirectories = Directory.GetFileSystemEntries(fighterPath);
            foreach (string dir in characterHubDirectories)
            {
                string[] internalCharacterDirectories = Directory.GetFileSystemEntries(dir);
                foreach (string modify in internalCharacterDirectories)
                {
                    //Delete all undesired files
                    if (!modify.Substring(modify.Length - 20).Contains(c00Keep) && !c00Keep.Equals("c0"))
                    {
                        Directory.Delete(modify, true);
                    }
                }

                //Rename the chosen file
                var remainingFilePath = c00Keep.Equals("c0") 
                    ? internalCharacterDirectories.First() 
                    : help.CreateFilePath(internalCharacterDirectories.First(), c00Keep);
                if (Directory.Exists(remainingFilePath))
                {
                    var newFolderPath = help.CreateFilePath(internalCharacterDirectories.First(), c00NewPosition);
                    try
                    {
                        Directory.Move(remainingFilePath, newFolderPath);
                        isFighterReplacementSuccessful = true;
                    } catch(Exception)
                    {
                        //Assumes that the file is already in this location (you know what assuming does)
                        isFighterReplacementSuccessful = true;
                    }
                }
            }
            return isFighterReplacementSuccessful;
        }

        public bool ReplaceUiFiles(string path)
        {
            var help = new HelperFunctions();
            var c00Keep = UserPreferences.FileToKeep.Replace("c", "_");
            var c00NewPosition = UserPreferences.LocationToMove.Replace("c", "_");

            //Entering the ui folder
            var fighterPath = help.ScanForFileName("ui", path);
            if (fighterPath.Equals("error")) return false;

           help.TryToDelete(fighterPath, "param");
           help.TryToDelete(fighterPath, "message");

            //Entering the replace folder
            fighterPath = help.EnterFolder(fighterPath);

            fighterPath = help.EnterFolder(fighterPath, "chara");

            //All chara_x folders
            string[] characterHubDirectories = Directory.GetFileSystemEntries(fighterPath);
            foreach (string dir in characterHubDirectories)
            {
                //Inside a specific chara_x folder
                string[] internalCharacterDirectories = Directory.GetFileSystemEntries(dir);
                foreach (string modify in internalCharacterDirectories)
                {
                    //Delete all undesired files
                    if (!modify.Substring(modify.Length - 20).Contains(c00Keep) && !c00Keep.Equals("c0"))
                    {
                        File.Delete(modify);
                    }
                }

                //Rename the chosen file
                var alteredPath = internalCharacterDirectories.First().Remove(internalCharacterDirectories.First().Length - 5);
                var remainingFilePath = c00Keep.Equals("_0")
                    ? alteredPath
                    : help.CreateFilePath(alteredPath, c00Keep);

                    var newFolderPath = help.CreateFilePath(alteredPath, c00NewPosition)+ ".bntx";
                    remainingFilePath += ".bntx";
                try
                {
                    Directory.Move(remainingFilePath, newFolderPath);
                } catch { isUiReplacementSuccessful = false; }
                    isUiReplacementSuccessful = true;
            }
            //Deleting chara_5 and chara_7 file
            help.TryToDeleteAppend(characterHubDirectories.First(), "chara_5");
            help.TryToDeleteAppend(characterHubDirectories.First(), "chara_7");
            return isUiReplacementSuccessful;
        }

        public bool ReplaceJsonFiles(string path)
        {
            var help = new HelperFunctions();
            var c00Keep = UserPreferences.FileToKeep;
            var c00NewPosition = UserPreferences.LocationToMove;

            //Enter man folder
            path = help.EnterFolder(path);

            if (!c00Keep.Equals("c0") && !c00NewPosition.Equals("c0"))
            {
                string jsonContent = string.Empty;
                try
                {
                    // Load the JSON content from the file
                    jsonContent = File.ReadAllText(path);
                } catch
                {
                    path += "\\config.json";
                    jsonContent = File.ReadAllText(path);
                }

                // Replace the desired string character using the Replace method
                string modifiedJsonContent = jsonContent.Replace(c00Keep, c00NewPosition);

                // Write the modified content back to the file
                File.WriteAllText(path, modifiedJsonContent);
                return true;
            }
            return false;
        }

        public bool ReplaceSoundFiles(string path)
        {
            var help = new HelperFunctions();
            var c00Keep = UserPreferences.FileToKeep;
            var c00NewPosition = UserPreferences.LocationToMove;

            //Entering the bank file
            var soundPath = help.ScanForFileName("sound", path);
            if (soundPath.Equals("error")) return false;

            //Entering the fighter folder
            var soundFighterPath = help.ScanForFileName("fighter", soundPath);

            if (!soundFighterPath.Equals("error"))
            {
                string[] internalFighterFolders = Directory.GetFileSystemEntries(soundFighterPath);
                foreach (string modify in internalFighterFolders)
                {
                    //Deletes all fighter voice files
                    if (!modify.Substring(modify.Length-20).Contains(c00Keep))
                        File.Delete(modify);
                }
                string[] internalFighterFoldersAfterDelete = Directory.GetFileSystemEntries(soundFighterPath);
                foreach (string modify in internalFighterFoldersAfterDelete)
                {
                    //Renames all fighter voice files
                    if (c00Keep.Equals("c0"))
                    {
                        var indexOfPlace = modify.IndexOf("c0");
                        c00Keep = modify.Substring(indexOfPlace, 3);
                    }
                    if (!c00NewPosition.Equals("c0") && modify.Substring(modify.Length - 20).Contains(c00Keep))
                    {
                        string prefix = modify.Substring(0, modify.Length - 20);
                        string suffix = modify.Substring(modify.Length - 20);
                        var newFolderPath = prefix + suffix.Replace(c00Keep, c00NewPosition);
                        Directory.Move(modify, newFolderPath);
                    }
                }
            }

            //Entering the fighter_voice folder
            var soundVoicePath = help.ScanForFileName("fighter_voice", soundPath);

            if (!soundVoicePath.Equals("error"))
            {
                string[] internalVoiceFolders = Directory.GetFileSystemEntries(soundVoicePath);
                foreach (string modify in internalVoiceFolders)
                {
                    //Deletes all fighter voice files
                    if (!modify.Contains(c00Keep))
                        File.Delete(modify);
                }

                foreach (string modify in internalVoiceFolders)
                {
                    if (c00Keep.Equals("c0"))
                    {
                        var indexOfPlace = modify.IndexOf("c0");
                        c00Keep = modify.Substring(indexOfPlace, 3);
                    }
                    if (!c00NewPosition.Equals("c0") && modify.Contains(c00Keep))
                    {
                        var newFolderPath = modify.Replace(c00Keep, c00NewPosition);
                        Directory.Move(modify, newFolderPath);
                    }
                }
            }
            return true;
        }
    }
}
