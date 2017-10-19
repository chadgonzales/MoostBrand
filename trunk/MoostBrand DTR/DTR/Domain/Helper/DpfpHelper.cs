using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class DpfpHelper
    {
        public static Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();	// Create a sample convertor.
            Bitmap bitmap = null;												            // TODO: the size doesn't matter
            Convertor.ConvertToPicture(Sample, ref bitmap);									// TODO: return bitmap as a result
            return bitmap;
        }

        public static DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();	// Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);			// TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        public static string ConvertTemplateToBase64(DPFP.Template Template)
        {
            //Courtesy of stackoverflow ==============================
            MemoryStream fingerprintData = new MemoryStream();
            Template.Serialize(fingerprintData);
            fingerprintData.Position = 0;
            BinaryReader br = new BinaryReader(fingerprintData);
            Byte[] bytes = br.ReadBytes((Int32)fingerprintData.Length);
            //=======================================================
            return Convert.ToBase64String(bytes);
        }
    }
}
