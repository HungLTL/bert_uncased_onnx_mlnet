using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLNet_BERT_Uncased.DataStructures
{
    class BERTInputData
    {
        [VectorType(1, 512)]
        [ColumnName("input_ids")]
        public long[] InputIds { get; set; }

        [VectorType(1, 512)]
        [ColumnName("attention_mask")]
        public long[] AttentionMask { get; set; }

        [VectorType(1, 512)]
        [ColumnName("token_type_ids")]
        public long[] TokenTypeIds { get; set; }
    }
}
