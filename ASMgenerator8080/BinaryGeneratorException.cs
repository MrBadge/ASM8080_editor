using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMgenerator8080
{
    class BinaryGeneratorException: Exception
    {
        public int line { set; get; }
        public string Message { get; set; }

        public BinaryGeneratorException(string s, int lineNumb)
        {
            line = lineNumb;
            Message = s;
        }

        public BinaryGeneratorException(string s)
        {
            
        }

        public BinaryGeneratorException()
        {
        }
    }
}
