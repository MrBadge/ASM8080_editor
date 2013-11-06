using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMgenerator8080
{
    class BinaryGeneratorWarning: Exception
    {
        public int[] linenums { get; set; }
        public string message { get; set; }
        
        public BinaryGeneratorWarning(string msg, int[] linenumbers) : base(msg) 
        {
            linenums = linenumbers;
            message = msg;
        }
    }
}
