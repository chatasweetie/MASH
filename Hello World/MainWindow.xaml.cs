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


namespace Hello_World
{
    public partial class MainWindow : Window
    {
        private TextBlock StatusTextBlock;
        private GenAIModel? _model;

        public MainWindow()
        {
            InitializeComponent();
            StatusTextBlock = new TextBlock();
            Debug.WriteLine("********************************");
            Debug.WriteLine("Initializing MainWindow");
            Debug.WriteLine("********************************");
            InitializeModel();
        }

        // Validates that the input contains only numbers
        private void ValidateNumberInput(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
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
            if (_model != null && _model.IsReady)
            {
                string prompt = "We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less and no other information. You must follow the rule. I'll give you two vehicle names or types of vehicle and I want you to tell me a, only one, vehicle name or type that would be funny or odd for the first two. Let's begin now.  Mini cooper and AUDI";
                string response = await _model.ProcessPromptAsync(prompt);
                Debug.WriteLine("Did it!");
                DisplayResponse(response);
                Debug.WriteLine("********************************");
            }
        }

        private void DisplayResponse(string response)
        {
            Debug.WriteLine("********************************");
            Debug.WriteLine(response);
            Debug.WriteLine(response.GetType());

        }


        private async void RollMagicNumberButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int magicNumber = GenerateMagicNumber();

                var userInput = GetUserInput();

                if (userInput != null)
                {
                    Debug.WriteLine($"Spouse1: {userInput.Spouse1}, Spouse2: {userInput.Spouse2}, Kids1: {userInput.Kids1}, Kids2: {userInput.Kids2}, Car1: {userInput.Car1}, Car2: {userInput.Car2}");

                    var aiSuggestions = GetAISuggestions(userInput);

                    var gameArray = new List<List<string>>
                                                                {
                                                                    new List<string> { "Mansion", "Apartment", "Shack", "House" },
                                                                    new List<string> { userInput.Spouse1, userInput.Spouse2, aiSuggestions.Spouse3 },
                                                                    new List<string> { userInput.Kids1, userInput.Kids2, aiSuggestions.Kids3 },
                                                                    new List<string> { userInput.Car1, userInput.Car2, aiSuggestions.Car3 }
                                                                };

                    var result = ProcessGameArray(gameArray, magicNumber) as List<string>;

                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Your True Fortune",
                        Content = $"Behold! Your fortune has become clear. With the power of your magic number, I see your future. You will live in a/an {result[0]}, marry {result[1]}, have {result[2]} child(ren) with them and drive a {result[3]}.",
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

        private int GenerateMagicNumber()
        {
            Random random = new Random();
            return random.Next(2, 5);
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
                Car2 = (container?.FindName("Car2") as TextBox)?.Text ?? string.Empty
            };
        }

        /////////////////////////////////////////////////////////////////
        private AISuggestions GetAISuggestions(UserInput userInput)
        {
            Debug.WriteLine("Entering GetAISuggestions method");


            string inputText = $"We are going to play a game, a very silly game. The one rule is that you can only provide a single response that is 3 words or less.  I'll give you two vehicle names or types of vehicle and I want you to tell me a, only one, vehicle name or type that would be funny or odd for the first two. Let's begin now.  Mini cooper and AUDI";
            Debug.WriteLine($"Input text: {inputText}");


            return new AISuggestions
            {
                Spouse3 = "Leonardo DiCaprio",
                Kids3 = "3",
                Car3 = "Limo"
            };
        }


        /////////////////////////////////////////////////////////////////
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
    }

    public class UserInput
    {
        public string Spouse1 { get; set; } = string.Empty;
        public string Spouse2 { get; set; } = string.Empty;
        public string Kids1 { get; set; } = string.Empty;
        public string Kids2 { get; set; } = string.Empty;
        public string Car1 { get; set; } = string.Empty;
        public string Car2 { get; set; } = string.Empty;
        public string Spouse3 { get; set; } = string.Empty;
        public string Kids3 { get; set; } = string.Empty;
        public string Car3 { get; set; } = string.Empty;
    }

    public class AISuggestions
    {
        public string Spouse3 { get; set; } = string.Empty;
        public string Kids3 { get; set; } = string.Empty;
        public string Car3 { get; set; } = string.Empty;
    }
}
