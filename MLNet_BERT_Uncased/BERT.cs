using BERTTokenizers;
using MLNet_BERT_Uncased.DataStructures;
using MLNet_BERT_Uncased.Helpers;
using MLNet_BERT_Uncased.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLNet_BERT_Uncased
{
    class BERT
    {
        private List<string> _vocabulary;

        private Predictor _predictor;
        private BertUncasedBaseTokenizer _tokenizer;

        public BERT(string vocabularyFilePath, string bertModelPath)
        {
            _vocabulary = FileReader.ReadFile(vocabularyFilePath);
            _tokenizer = new BertUncasedBaseTokenizer();

            var trainer = new ModelTrainer();
            var trainedModel = trainer.BuildAndTrain(bertModelPath, false);
            _predictor = new Predictor(trainedModel);
        }

        public List<string> Predict(string context, string question)
        {
            List<string> results = new List<string>();
            var tokens = _tokenizer.Encode(512, new string[] { question, context });
            var input = BuildInput(context, question);
            var predictions = _predictor.Predict(input);
            var contextStart = tokens.FindIndex(o => o.InputIds == Tokens.Separation);

            List<string> output = new List<string>();
            List<(int, int, float)> list = GetPredictions(predictions, contextStart, 20, 512).OrderByDescending(o => o.Item3).ToList();
            float totalScore = 0;
            for (int i = 0; i < list.Count; i++)
            {
                int output0 = list[i].Item1;
                int output1 = list[i].Item2;
                float score = list[i].Item3;

                var predictedTokens = input.InputIds
                .Skip(output0)
                .Take(output1 + 1 - output0)
                .Select(o => _vocabulary[(int)o])
                .ToList();
                var connectedTokens = _tokenizer.Untokenize(predictedTokens);

                string answer = "";
                foreach (string s in connectedTokens)
                    answer += s + " ";

                totalScore += score;
                output.Add(answer + " (Confidence: " + score.ToString() + ")\n");
            }
            output.Add("average: " + (totalScore / list.Count).ToString());
            return output;
        }

        private IEnumerable<(int, int, float)> GetPredictions(BERTPrediction results, int minIndex, int topN, int maxLength)
        {
            var bestOutput0 = results.Output0
                .Select((output, index) => (Output: output, Index: index))
                .OrderByDescending(o => o.Output)
                .Take(topN);

            var bestOutput1 = results.Output1
                .Select((output, index) => (Output: output, Index: index))
                .OrderByDescending(o => o.Output)
                .Take(topN);

            var bestResultsWithScore = bestOutput0
                .SelectMany(output0
                => bestOutput0
                .Select(output1 =>
                    (
                        Output0: output0.Index,
                        Output1: output1.Index,
                        Score: output0.Output + output1.Output
                    )
                )
            )
            .Where(entry => !(entry.Output1 < entry.Output0 || entry.Output1 - entry.Output0 > maxLength || entry.Output0 == 0 && entry.Output1 == 0 || entry.Output0 < minIndex))
            .Take(topN);

            return bestResultsWithScore;
        }

        private BERTInputData BuildInput(string context, string question)
        {
            var encoded = _tokenizer.Encode(512, new string[] { question, context });
            
            return new BERTInputData()
            {
                InputIds = encoded.Select(t => t.InputIds).ToArray(),
                AttentionMask = encoded.Select(t => t.AttentionMask).ToArray(),
                TokenTypeIds = encoded.Select(t => t.TokenTypeIds).ToArray()
            };
        }
    }
}
