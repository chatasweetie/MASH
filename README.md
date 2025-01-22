# MASH AI Model

[MASH](https://en.wikipedia.org/wiki/MASH_(game)) is a classic paper-and-pencil fortune-telling game that playfully predicts a player's future. The name MASH stands for Mansion, Apartment, Shack, and House, representing potential living situations. 
This project demonstrates the implementation of a simple AI model using the Microsoft ML OnnxRuntimeGenAI library to enhance the MASH game experience.

## Project Overview

This project is written in C# and targets .NET 8. It includes a WPF application that allows users to input options for various categories and generate predictions using an AI model.

## Project Structure

- **Models/AIModel.cs**: Contains the `GenAIModel` class for AI model initialization, processing, and disposal.
- **MainPage.xaml**: Defines the UI layout.
- **MainPage.xaml.cs**: Handles user interactions and binds data to the UI.

![MASH Demo](Hello%20World/Assets/MASH-demo.gif)
UI Design and Assets created by Jessie Vitale

## Running the Application

1. **Download the Model**: Download the model from Hugging Face's [Phi-3-mini-4k-instruct-onnx](https://huggingface.co/microsoft/Phi-3-mini-4k-instruct-onnx/tree/main/cpu_and_mobile/cpu-int4-rtn-block-32-acc-level-4).
2. **Clone the Repository**: Clone the repository to your local machine.
3. **Open the Solution**: Open the solution in Visual Studio 2022.
4. **Update Model Directory**: Update the `ModelDirectory` constant in `Models/AIModel.cs` to the path where you downloaded the model.
5. **Restore NuGet Packages**:
    1. Right-click on the solution in Solution Explorer.
    2. Select `Restore NuGet Packages`.
    3. Wait for the process to complete.
6. **Build the Solution**: Build the solution to ensure all dependencies are resolved.
7. **Run the Application**: Start the application by pressing `F5` or selecting `Debug > Start Debugging`.

## Requirements

- **C# Version**: 12.0
- **.NET Target**: .NET 8
- **Dependencies**: Microsoft.ML.OnnxRuntimeGenAI

## License

This project is licensed under the MIT License.
