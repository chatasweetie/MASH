using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Text.RegularExpressions;
using Windows.Devices.Lights;

namespace Hello_World
{
    public partial class MainWindow : Window
    {
        private TextBlock StatusTextBlock;

        public MainWindow()
        {
            InitializeComponent();
            StatusTextBlock = new TextBlock(); // Initialize StatusTextBlock to avoid CS8618
        }

        private void NumberValidationTextBox(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private async void RollMagicNumberButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Displays message to user that the button was clicked
                System.Diagnostics.Debug.WriteLine("Button clicked, generating magic number...");

                // Selects Magic Number
                Random random = new Random();
                int magicNumber = random.Next(2, 5);

                // Gets user input from TextBox controls
                var container = this.Content as FrameworkElement;
                TextBox Spouse1 = container?.FindName("Spouse1") as TextBox;
                TextBox Spouse2 = container?.FindName("Spouse2") as TextBox;
                TextBox Kids1 = container?.FindName("Kids1") as TextBox;
                TextBox Kids2 = container?.FindName("Kids2") as TextBox;
                TextBox Car1 = container?.FindName("Car1") as TextBox;
                TextBox Car2 = container?.FindName("Car2") as TextBox;

                // Ensure the TextBox controls are not null before accessing them
                if (Spouse1 != null && Spouse2 != null && Kids1 != null && Kids2 != null && Car1 != null && Car2 != null)
                {
                    // Retrieve the text entered by the user
                    string spouse1Text = Spouse1.Text;
                    string spouse2Text = Spouse2.Text;
                    string kids1Text = Kids1.Text;
                    string kids2Text = Kids2.Text;
                    string car1Text = Car1.Text;
                    string car2Text = Car2.Text;

                    // Optionally, you can use the retrieved text here
                    System.Diagnostics.Debug.WriteLine($"Spouse1: {spouse1Text}, Spouse2: {spouse2Text}, Kids1: {kids1Text}, Kids2: {kids2Text}, Car1: {car1Text}, Car2: {car2Text}");

                    // Set the TextBox controls' text to the magic number
                    Spouse3.Text = magicNumber.ToString();
                    Kids3.Text = magicNumber.ToString();
                    Car3.Text = magicNumber.ToString();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("One or more TextBox controls are null.");
                }

                // Get AI's suggestions for 3rd options
                // need to check out AI gallary on how the chat was incorporated at the moment
                // will fake the call
                string AISpouse3 = "Leonardo DiCaprio";
                string AIKids3 = "3";
                string AICar3 = "Limo";

                // Set the TextBox controls' text to the AI's suggestions
                Spouse3.Text = AISpouse3;
                Kids3.Text = AIKids3;
                Car3.Text = AICar3;

                var GameArray = new List<List<string>>
                {
                    new List<string> { "M", "A", "S", "H" },
                    new List<string> { Spouse1.Text, Spouse2.Text, AISpouse3 },
                    new List<string> { Kids1.Text, Kids2.Text, AIKids3 },
                    new List<string> { Car1.Text, Car2.Text, AICar3 }
                };

                // Selects fortune with MagicNumber
                var result = ProcessGameArray(GameArray, magicNumber) as List<string>;



                // Create and configure the ContentDialog
                System.Diagnostics.Debug.WriteLine("Creating ContentDialog...");
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Your True Fortune",
                    Content = $"Behold! Your fortune has become clear. With the power of yoru magic number, I see your future. You will live in {result[0]}, marry {result[1]}, have {result[2]} child(ren) with them and drive a {result[3]}.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot // Set the XamlRoot property
                };

                // Show the ContentDialog
                System.Diagnostics.Debug.WriteLine("Showing ContentDialog...");
                await dialog.ShowAsync();
                System.Diagnostics.Debug.WriteLine("ContentDialog shown successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred: {ex.Message}");
            }
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
            // Flatten the list of lists into a single list
            var flattenedList = gameArray.SelectMany(subArray => subArray).ToList();
            System.Diagnostics.Debug.WriteLine("In ProcessGameArray");
            Console.WriteLine("Flattened List:");
            foreach (var item in flattenedList)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }

            return flattenedList;
        }
    }

}