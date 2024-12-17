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

namespace Hello_World
{
    public partial class MainWindow : Window
    {
        private TextBlock StatusTextBlock;

        public MainWindow()
        {
            InitializeComponent();
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

                // Example: Update a TextBox with a random number
                Random random = new Random();
                int magicNumber = random.Next(1, 100);

                // Ensure the TextBox controls are not null before accessing them
                if (Spouse3 != null && Kids3 != null && Car3 != null)
                {
                    Spouse3.Text = magicNumber.ToString();
                    Kids3.Text = magicNumber.ToString();
                    Car3.Text = magicNumber.ToString();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("One or more TextBox controls are null.");
                }

                // Create and configure the ContentDialog
                System.Diagnostics.Debug.WriteLine("Creating ContentDialog...");
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Magic Number",
                    Content = $"Your magic number is {magicNumber}",
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
    }
}
