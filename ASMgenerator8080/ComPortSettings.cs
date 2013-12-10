using System.IO.Ports;

namespace ASMgenerator8080
{
    public class ComPortSettings
    {
        private static readonly ComPortSettings instance = new ComPortSettings();

        public static ComPortSettings Instance
        {
            get { return instance; }
        }

        public StopBits sb { get; set; }
        public int baud { get; set; }
        public Parity par { get; set; }
        public int databits { get; set; }
        public string ComPortName { get; set; }

        public void SetPortSet(StopBits s = StopBits.Two, Parity p = Parity.Even, int b = 4800, int d = 8,
            string name = "")
        {
            sb = s;
            par = p;
            baud = b;
            databits = d;
            if (name != "")
                ComPortName = name;
        }
    }
}