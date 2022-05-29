using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLNet_BERT_Uncased.DataStructures
{
    class Predictor
    {
        private MLContext _mlContext;
        private PredictionEngine<BERTInputData, BERTPrediction> _predictionEngine;
        
        public Predictor(ITransformer trainedModel)
        {
            _mlContext = new MLContext();
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<BERTInputData, BERTPrediction>(trainedModel);
        }

        public BERTPrediction Predict(BERTInputData encodedInput)
        {
            return _predictionEngine.Predict(encodedInput);
        }
    }
}
