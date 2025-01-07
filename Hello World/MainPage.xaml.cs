using Hello_World.AIModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Hello_World.AIModel;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Hello_World
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private TextBlock StatusTextBlock;
        private GenAIModel? _model;
        private MainViewModel _viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            StatusTextBlock = new TextBlock();
            Debug.WriteLine("********************************");
            Debug.WriteLine("Initializing MainWindow");
            Debug.WriteLine("********************************");
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
            InitializeModel();
        }

        private async void InitializeModel()
        {
            Debug.WriteLine("********************************");
            Debug.WriteLine("Initializing Model");
            Debug.WriteLine("********************************");
            GenAIModel.InitializeGenAI();
            _model = await GenAIModel.CreateAsync(@"C:\Users\jearleycha\source\repos\MASH\Hello World\Models\cpu-int4-rtn-block-32-acc-level-4\");
            Debug.WriteLine("********************************");
            Debug.WriteLine("Model is ready");
            Debug.WriteLine("********************************");
        }

        private async void RollMagicNumberButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int magicNumber = GenerateMagicNumber(2, 5);
                var userInput = GetUserInput();

                if (userInput != null)
                {
                    Debug.WriteLine($"Spouse1: {userInput.Spouse1}, Spouse2: {userInput.Spouse2}, Kids1: {userInput.Kids1}, Kids2: {userInput.Kids2}, Car1: {userInput.Car1}, Car2: {userInput.Car2}, Career1: {userInput.Career1}, Career2: {userInput.Career2}");

                    var aiSuggestions = await GetAISuggestions(userInput);
                    _viewModel.AISuggestions = aiSuggestions;
                    Debug.WriteLine(aiSuggestions.Spouse3);
                    Debug.WriteLine(aiSuggestions.Car3);
                    Debug.WriteLine(aiSuggestions.Kids3);
                    Debug.WriteLine(aiSuggestions.Career3);

                    var gameArray = new List<List<string>>
                            {
                                new List<string> { "Mansion", "Apartment", "Shack", "House" },
                                new List<string> { userInput.Spouse1, userInput.Spouse2, aiSuggestions.Spouse3 },
                                new List<string> { userInput.Kids1, userInput.Kids2, aiSuggestions.Kids3 },
                                new List<string> { userInput.Car1, userInput.Car2, aiSuggestions.Car3 },
                                new List<string> { userInput.Career1, userInput.Career2, aiSuggestions.Career3 }
                            };

                    var result = ProcessGameArray(gameArray, magicNumber) as List<string>;

                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Your True Fortune",
                        Content = $"Behold! Your fortune has become clear. With the power of your magic number, I see your future. You'll have an amazing career in {result[4]}, you'll live in a/an {result[0]}, marry {result[1]}, have {result[2]} child(ren) with them and drive a {result[3]}.",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };

                    await dialog.ShowAsync();
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

        private async Task<AISuggestions> GetAISuggestions(UserInput userInput)
        {
            Debug.WriteLine("Entering GetAISuggestions method");

            string spouseInputText = $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less. I'll give you two people names and I want you to tell me a, only one, name and nothing else. You want you to only use very popular celebrity names. Let's begin now: {userInput.Spouse1} and {userInput.Spouse2}";
            string carInputText = $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less. I'll give you two vehicle names or types of vehicle and I want you to tell me a, only one, vehicle name or type and nothing else. Ideally something that would be funny or odd for the first two. Let's begin now: {userInput.Car1} and {userInput.Car2}";
            string careerInputText = $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less. I'll give you two career or jobs and I want you to tell me a, only one, career or job and nothing else. Ideally something that would be funny or odd for the first two. Let's begin now: {userInput.Career1} and {userInput.Career2}";

            string spouse = string.Empty;
            string car = string.Empty;
            string career = string.Empty;

            if (_model != null && _model.IsReady)
            {
                Debug.WriteLine("DOING SPOUSE:");
                string spouseResponse = await _model.ProcessPromptAsync(spouseInputText);
                spouse = CleanAIResponseGetLastLineWithoutQuotes(spouseResponse);
                Debug.WriteLine(spouse);
                _viewModel.AISuggestions.Spouse3 = spouse;

                Debug.WriteLine("DOING CAR:");
                string carResponse = await _model.ProcessPromptAsync(carInputText);
                car = CleanAIResponseGetLastLineWithoutQuotes(carResponse);
                Debug.WriteLine(car);
                _viewModel.AISuggestions.Car3 = car;

                Debug.WriteLine("DOING CAREER:");
                string careerResponse = await _model.ProcessPromptAsync(careerInputText);
                career = CleanAIResponseGetLastLineWithoutQuotes(careerResponse);
                Debug.WriteLine(career);
                _viewModel.AISuggestions.Career3 = career;
            }

            string kids3 = GenerateMagicNumber(0, 5).ToString();
            _viewModel.AISuggestions.Kids3 = kids3;

            return new AISuggestions
            {
                Spouse3 = spouse,
                Kids3 = kids3,
                Car3 = car,
                Career3 = career
            };
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

        // Validates that the input contains only numbers
        private void ValidateNumberInput(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
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
