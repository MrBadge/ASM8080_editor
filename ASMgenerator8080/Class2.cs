using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMgenerator8080
{
    class BinaryGeneratorException: Exception
    {
        public BinaryGeneratorException(string s) : base(s) {}
        public BinaryGeneratorException() : base() {}
    }
}
