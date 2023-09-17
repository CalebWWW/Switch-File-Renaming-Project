using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSwitchFileRenamingWorking.Backend;

namespace WebSwitchFileRenamingWorking.Pages
{
    public partial class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
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
            ResultStatus = "";
            AreChecked = new List<int>();
            Titles = new Stack<string>();
            Titles.Push("Rename the Json");
            Titles.Push("Rename Fighter Files");
            Titles.Push("Rename Ui Files");
            UserInputMoveFileTo = "";
            UserInputKeepFile = "x";
        }

        public void OnPostSubmit()
        {
            SetPreferences(AreChecked);
            var unzipper = new PrepareZippedFile();

            if (UploadedFile is null)
            {
                if (LargeFileName.Equals(""))
                {
                    ResultStatus = "File is null";
                    return;
                }
                ResultStatus = unzipper.BeginReplacementProcess(LargeFileName);
            }
            else
                ResultStatus = unzipper.BeginReplacementProcess(UploadedFile.FileName);
        }

        public void OnPostSecondButton()
        {
            SetPreferences(AreChecked);
            var unzipper = new PrepareZippedFile();

            if (UploadedFile is null)
            {
                if (LargeFileName.Equals(""))
                {
                    ResultStatus = "File is null";
                    return;
                }
                ResultStatus = unzipper.BeginCheckProcess(LargeFileName);
            }
            else
                ResultStatus = unzipper.BeginCheckProcess(UploadedFile.FileName);
        }

        public void SetPreferences(List<int> Checked)
        {
            UserPreferences.ReplaceUi = Checked.Contains(0);
            UserPreferences.ReplaceFighter = Checked.Contains(1);
            UserPreferences.ReplaceJson = Checked.Contains(2);
            UserPreferences.FileToKeep = $"c0{UserInputKeepFile}";
            UserPreferences.LocationToMove= $"c0{UserInputMoveFileTo}";
        }
    }
}