using Hello_World.AIModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI;
using Microsoft.UI.Dispatching;



namespace Hello_World
{
    public sealed partial class MainPage : Page
    {
        private GenAIModel? _model;
        private MainViewModel _viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
            InitializeModel();
        }

        private void InitializeModel()
        {
            Debug.WriteLine("********************************");
            Debug.WriteLine("Initializing Model");
            Debug.WriteLine("********************************");

            Task.Run(async () =>
            {
                GenAIModel.InitializeGenAI();
                _model = await GenAIModel.CreateAsync(@"C:\Users\jearleycha\source\repos\MASH\Hello World\Models\cpu-int4-rtn-block-32-acc-level-4\");
                Debug.WriteLine("********************************");
                Debug.WriteLine("Model is ready");
                Debug.WriteLine("********************************");

                DispatcherQueue.TryEnqueue(() =>
                {
                    _viewModel.ModelIsReady = true;
                    _viewModel.ButtonText = "Ready to receive your wishes";
                    StartGatheringMagicAnimation();
                });
            });
        }

        private async void RollMagicNumberButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtonText("Gathering magical forces to see your future");

            try
            {
                int magicNumber = GenerateMagicNumber(2, 5);
                var userInput = GetUserInput();

                if (userInput != null)
                {
                    LogUserInput(userInput);

                    var aiSuggestions = await GetAISuggestionsAsync(userInput);
                    _viewModel.AISuggestions = aiSuggestions;

                    LogAISuggestions(aiSuggestions);

                    var result = ProcessGameArray(CreateGameArray(userInput, aiSuggestions), magicNumber) as List<string>;

                    await ShowResultDialog(result);
                    UpdateButtonText("Ready to retry if you didn't like your fortune");
                }
                else
                {
                    Debug.WriteLine("One or more TextBox controls are null.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearUserInput();
        }

        private void StartGatheringMagicAnimation()
        {
            _viewModel.ButtonText = "Ready to receive your wishes";
            var storyboard = (Storyboard)Resources["GatheringMagicStoryboard"];
            storyboard.Begin();
        }

        private int GenerateMagicNumber(int low, int high)
        {
            Random random = new Random();
            return random.Next(low, high);
        }

        private UserInput GetUserInput()
        {
            var container = this.Content as FrameworkElement;
            return new UserInput
            {
                Spouse1 = (container?.FindName("Spouse1") as TextBox)?.Text ?? string.Empty,
                Spouse2 = (container?.FindName("Spouse2") as TextBox)?.Text ?? string.Empty,
                Kids1 = (container?.FindName("Kids1") as TextBox)?.Text ?? string.Empty,
                Kids2 = (container?.FindName("Kids2") as TextBox)?.Text ?? string.Empty,
                Car1 = (container?.FindName("Car1") as TextBox)?.Text ?? string.Empty,
                Car2 = (container?.FindName("Car2") as TextBox)?.Text ?? string.Empty,
                Career1 = (container?.FindName("Career1") as TextBox)?.Text ?? string.Empty,
                Career2 = (container?.FindName("Career2") as TextBox)?.Text ?? string.Empty
            };
        }

        private async Task<AISuggestions> GetAISuggestionsAsync(UserInput userInput)
        {
            Debug.WriteLine("Entering GetAISuggestions method");

            string kids3 = GenerateMagicNumber(0, 5).ToString();

            var spouseTask = GetSuggestionAsync(CreateSpousePrompt(userInput));
            var carTask = GetSuggestionAsync(CreateCarPrompt(userInput));
            var careerTask = GetSuggestionAsync(CreateCareerPrompt(userInput));

            await Task.WhenAll(spouseTask, carTask, careerTask);

            return new AISuggestions
            {
                Spouse3 = await spouseTask,
                Kids3 = kids3,
                Car3 = await carTask,
                Career3 = await careerTask
            };
        }

        private async Task<string> GetSuggestionAsync(string inputText)
        {
            if (_model != null && _model.IsReady)
            {
                Debug.WriteLine(inputText);
                string response = await _model.ProcessPromptAsync(inputText);
                return CleanAIResponseGetLastLineWithoutQuotes(response);
            }

            return string.Empty;
        }

        private object ProcessGameArray(List<List<string>> gameArray, int magicNumber)
        {
            foreach (var subArray in gameArray)
            {
                int index = 0;
                while (subArray.Count > 1)
                {
                    index = (index + magicNumber - 1) % subArray.Count;
                    subArray.RemoveAt(index);
                }
            }

            var flattenedList = gameArray.SelectMany(subArray => subArray).ToList();
            Debug.WriteLine("In ProcessGameArray");
            Debug.WriteLine("Flattened List:");
            foreach (var item in flattenedList)
            {
                Debug.WriteLine(item);
            }

            return flattenedList;
        }

        public string CleanAIResponseGetLastLineWithoutQuotes(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var lines = input.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var lastLine = lines.LastOrDefault()?.Trim() ?? string.Empty;

            if (lastLine.StartsWith("\"") && lastLine.EndsWith("\""))
            {
                lastLine = lastLine.Substring(1, lastLine.Length - 2);
            }

            return lastLine;
        }

        private void ValidateNumberInput(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void UpdateButtonText(string text)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                _viewModel.ButtonText = text;
            });
        }

        private void LogUserInput(UserInput userInput)
        {
            Debug.WriteLine($"Spouse1: {userInput.Spouse1}, Spouse2: {userInput.Spouse2}, Kids1: {userInput.Kids1}, Kids2: {userInput.Kids2}, Car1: {userInput.Car1}, Car2: {userInput.Car2}, Career1: {userInput.Career1}, Career2: {userInput.Career2}");
        }

        private void LogAISuggestions(AISuggestions aiSuggestions)
        {
            Debug.WriteLine(aiSuggestions.Spouse3);
            Debug.WriteLine(aiSuggestions.Car3);
            Debug.WriteLine(aiSuggestions.Kids3);
            Debug.WriteLine(aiSuggestions.Career3);
        }

        private List<List<string>> CreateGameArray(UserInput userInput, AISuggestions aiSuggestions)
        {
            return new List<List<string>>
                {
                    new List<string> { "Mansion", "Apartment", "Shack", "House" },
                    new List<string> { userInput.Spouse1, userInput.Spouse2, aiSuggestions.Spouse3 },
                    new List<string> { userInput.Kids1, userInput.Kids2, aiSuggestions.Kids3 },
                    new List<string> { userInput.Car1, userInput.Car2, aiSuggestions.Car3 },
                    new List<string> { userInput.Career1, userInput.Career2, aiSuggestions.Career3 }
                };
        }

        private async Task ShowResultDialog(List<string> result)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Your True Fortune",
                Content = $"Behold! Your fortune has become clear. With the power of your magic number, I see your future. You'll have an amazing career in {result[4]}, you'll live in a/an {result[0]}, marry {result[1]}, have {result[2]} child(ren) with them and drive a {result[3]}.",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void ClearUserInput()
        {
            _viewModel.Spouse1 = string.Empty;
            _viewModel.Spouse2 = string.Empty;
            _viewModel.Kids1 = string.Empty;
            _viewModel.Kids2 = string.Empty;
            _viewModel.Car1 = string.Empty;
            _viewModel.Car2 = string.Empty;
            _viewModel.Career1 = string.Empty;
            _viewModel.Career2 = string.Empty;
            _viewModel.AISuggestions = new AISuggestions();
        }

        private string CreateSpousePrompt(UserInput userInput)
        {
            return $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less. I'll give you two people names and I want you to tell me a, only one, name and nothing else. You want you to only use very popular celebrity names. Let's begin now: {userInput.Spouse1} and {userInput.Spouse2}";
        }

        private string CreateCarPrompt(UserInput userInput)
        {
            return $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less. I'll give you two vehicle names or types of vehicle and I want you to tell me a, only one, vehicle name or type and nothing else. Ideally something that would be funny or odd for the first two. Let's begin now: {userInput.Car1} and {userInput.Car2}";
        }

        private string CreateCareerPrompt(UserInput userInput)
        {
            return $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less. I'll give you two career or jobs and I want you to tell me a, only one, career or job and nothing else. Ideally something that would be funny or odd for the first two. Let's begin now: {userInput.Career1} and {userInput.Career2}";
        }
    }


    public class UserInput
    {
        public string Spouse1 { get; set; } = string.Empty;
        public string Spouse2 { get; set; } = string.Empty;
        public string Kids1 { get; set; } = string.Empty;
        public string Kids2 { get; set; } = string.Empty;
        public string Car1 { get; set; } = string.Empty;
        public string Car2 { get; set; } = string.Empty;
        public string Career1 { get; set; } = string.Empty;
        public string Career2 { get; set; } = string.Empty;
        public string Spouse3 { get; set; } = string.Empty;
        public string Kids3 { get; set; } = string.Empty;
        public string Car3 { get; set; } = string.Empty;
        public string Career3 { get; set; } = string.Empty;
    }
}
