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
        public string message { get; set; }

        public BinaryGeneratorException(string s, int lineNumb): base(s)
        {
            line = lineNumb;
            message = s;
        }

        public BinaryGeneratorException(string s): base(s)
        {
            
        }

        public BinaryGeneratorException(): base()
        {
        }
    }
}
