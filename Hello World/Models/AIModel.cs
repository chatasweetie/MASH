using Microsoft.ML.OnnxRuntimeGenAI;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hello_World.AIModel
{
    internal class GenAIModel : IDisposable
    {
        private const int DefaultMaxLength = 1024;
        private static readonly string ModelDirectory = @"C:\Users\jearleycha\source\repos\MASH\Hello World\Models\cpu-int4-rtn-block-32-acc-level-4\";
        private Model? _model;
        private Tokenizer? _tokenizer;
        private static readonly SemaphoreSlim _createSemaphore = new(1, 1);
        private static OgaHandle? _ogaHandle;

        private GenAIModel()
        {
            Debug.WriteLine("********************************");
            Debug.WriteLine("Initializing GenAI");
            Debug.WriteLine("********************************");
            _ogaHandle = new OgaHandle();
        }

        public static async Task<GenAIModel?> CreateModel(CancellationToken cancellationToken = default)
        {
            Debug.WriteLine("********************************");
            Debug.WriteLine("In CreateModel");
            Debug.WriteLine("********************************");

            var model = new GenAIModel();
            var lockAcquired = false;

            try
            {
                await _createSemaphore.WaitAsync(cancellationToken);
                lockAcquired = true;
                cancellationToken.ThrowIfCancellationRequested();
                await model.InitializeModel(ModelDirectory, cancellationToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in CreateModel: {ex.Message}");
                model?.Dispose();
                return null;
            }
            finally
            {
                if (lockAcquired)
                {
                    _createSemaphore.Release();
                }
            }

            return model;
        }

        public bool IsReady => _model != null && _tokenizer != null;

        public void Dispose()
        {
            _model?.Dispose();
            _tokenizer?.Dispose();
            _ogaHandle?.Dispose();
        }

        public async Task<string> ProcessPrompt(string prompt, CancellationToken ct = default)
        {
            if (!IsReady)
            {
                throw new InvalidOperationException("Model is not ready");
            }

            using var generatorParams = new GeneratorParams(_model);
            using var sequences = _tokenizer.Encode(prompt);

            generatorParams.SetSearchOption("max_length", DefaultMaxLength + sequences[0].Length);
            generatorParams.SetInputSequences(sequences);
            generatorParams.TryGraphCaptureWithMaxBatchSize(1);

            using var tokenizerStream = _tokenizer.CreateStream();
            using var generator = new Generator(_model, generatorParams);
            StringBuilder stringBuilder = new();

            while (!generator.IsDone())
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                await Task.Delay(0, ct).ConfigureAwait(false);

                generator.ComputeLogits();
                generator.GenerateNextToken();
                var part = tokenizerStream.Decode(generator.GetSequence(0)[^1]);

                if (ct.IsCancellationRequested)
                {
                    part = "<|end|>";
                }

                stringBuilder.Append(part);
            }

            return stringBuilder.ToString();
        }

        private Task InitializeModel(string modelDir, CancellationToken cancellationToken = default)
        {
            return Task.Run(
                () =>
                {
                    _model = new Model(modelDir);
                    cancellationToken.ThrowIfCancellationRequested();
                    _tokenizer = new Tokenizer(_model);
                },
                cancellationToken);
        }
    }
}