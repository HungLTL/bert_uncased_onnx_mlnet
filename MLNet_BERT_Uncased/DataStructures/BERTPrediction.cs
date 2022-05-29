using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;

namespace MLNet_BERT_Uncased.DataStructures
{
    public class BERTPrediction
    {
        [VectorType(1, 512)]
        [ColumnName("output_0")]
        public float[] Output0 { get; set; }

        [VectorType(1, 512)]
        [ColumnName("output_1")]
        public float[] Output1 { get; set; }
    }
}
