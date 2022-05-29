using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;

namespace MLNet_BERT_Uncased.DataStructures
{
    class ModelTrainer
    {
        private readonly MLContext _mlContext;

        public ModelTrainer()
        {
            _mlContext = new MLContext(11);
        }

        public ITransformer BuildAndTrain(string bertModelPath, bool useGpu)
        {
            var inputColumns = new string[] { "input_ids", "attention_mask", "token_type_ids" };
            var outputColumns = new string[] { "output_0", "output_1" };

            var pipeline = _mlContext.Transforms
                .ApplyOnnxModel(modelFile: bertModelPath,
                shapeDictionary: new Dictionary<string, int[]>
                {
                    {"input_ids", new[] {1, 512} },
                    {"attention_mask", new[] {1, 512} },
                    {"token_type_ids", new[] {1, 512} },
                    { "output_0", new[] {1, 512}},
                    {"output_1", new[] {1, 512} }
                },
                outputColumnNames: outputColumns,
                inputColumnNames: inputColumns,
                gpuDeviceId: useGpu ? 0 : (int?)null,
                fallbackToCpu: true);
            return pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new List<BERTInputData>()));
        }
    }
}
