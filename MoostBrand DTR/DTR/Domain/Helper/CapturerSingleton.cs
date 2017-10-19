using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTR
{
    public class CapturerSingleton
    {
        static readonly DPFP.Capture.Capture _capturer = new DPFP.Capture.Capture();
        public static DPFP.Capture.Capture Instance
        {
            get
            {
                return _capturer;
            }
        }
    }
}