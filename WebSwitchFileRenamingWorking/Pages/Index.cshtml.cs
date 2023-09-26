using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpCompress.Common;
using WebSwitchFileRenamingWorking.Backend;

namespace WebSwitchFileRenamingWorking.Pages
{
    public partial class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string FilePath { get; set; }
        public Stack<string> Titles { get; set; }
        public string ResultStatus { get; set; }
        [BindProperty]
        public IFormFile UploadedFile { get; set; }
        [BindProperty]
        public List<int> AreChecked { get; set; }
        [BindProperty]
        public string UserInputMoveFileTo { get; set; }
        [BindProperty]
        public string UserInputKeepFile { get; set; }
        [BindProperty]
        public string LargeFileName { get; set; }

#pragma warning disable CS8618
        public IndexModel(ILogger<IndexModel> logger)
#pragma warning restore CS8618
        {
            _logger = logger;
            ResultStatus = string.Empty;
            AreChecked = new List<int>();
            Titles = new Stack<string>();
            Titles.Push("Is a .rar file");
            Titles.Push("Rename the Json");
            Titles.Push("Rename Fighter Files");
            Titles.Push("Rename Ui Files");
            UserInputMoveFileTo = string.Empty;
            LargeFileName = string.Empty;
            FilePath = string.Empty;
            UserInputKeepFile = "x";
        }

        public void OnPostSubmit()
        {
            SetPreferences(AreChecked);
            var unzipper = new PrepareZippedFile();
            string fileName = PrepareFile();
            ResultStatus = unzipper.BeginReplacementProcess(fileName);
        }

        public void OnPostSecondButton()
        {
            SetPreferences(AreChecked);
            var unzipper = new PrepareZippedFile();
            string fileName = PrepareFile();
            ResultStatus = unzipper.BeginCheckProcess(fileName);
        }

        public void OnPostCreateFolders() 
        {
            try
            {
                string baseDirectoroy = Path.Combine(Directory.GetCurrentDirectory(), "AdjModdingDirectory");
                Directory.CreateDirectory(baseDirectoroy);
                string folder1Path = Path.Combine($"{Directory.GetCurrentDirectory()}\\AdjModdingDirectory", "ModdingInputZippedFilesHere");
                string folder2Path = Path.Combine($"{Directory.GetCurrentDirectory()}\\AdjModdingDirectory", "ModdingOutputFolder");

                Directory.CreateDirectory(folder1Path);
                Directory.CreateDirectory(folder2Path);

                ResultStatus = "The folders are created correctly! Put your zipped folders in the file: ModdingInputZippedFilesHere";
            }
            catch (Exception)
            {
                ResultStatus = "There was an error creating the folders";
            }
        }

        public string PrepareFile()
        {
            if (UploadedFile is null && string.IsNullOrEmpty(LargeFileName))
            {
                ResultStatus = "File is null";
                return "";
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            FilePath = LargeFileName is null
                ? Path.GetFullPath(UploadedFile.FileName)
                : Path.GetFullPath(LargeFileName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8603 // Possible null reference return.
            return UploadedFile is null ? LargeFileName : UploadedFile.FileName;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void SetPreferences(List<int> Checked)
        {
            UserPreferences.ReplaceUi = Checked.Contains(0);
            UserPreferences.ReplaceFighter = Checked.Contains(1);
            UserPreferences.ReplaceJson = Checked.Contains(2);
            UserPreferences.IsRarFile = Checked.Contains(3);
            UserPreferences.FileToKeep = $"c0{UserInputKeepFile}";
            UserPreferences.LocationToMove= $"c0{UserInputMoveFileTo}";
        }
    }
}