//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using mpDataBase;

//namespace api
//{
//    public class Songs : mpDataBase.mpAudio
//    {
//        public int aid { get; set; }
//        public int owner_id { get; set; }
//        public string artist { get; set; }
//        public string title { get; set; }
//        public int duration { get; set; }
//        public string url { get; set; }
//        public string lurlcs_id { get; set; }
//        public int genre { get; set; }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api
{
    public class Songs
    {
        public int aid { get; set; }
        public int owner_id { get; set; }
        public string artist { get; set; }
        public string title { get; set; }
        public int duration { get; set; }
        public string url { get; set; }
        public string lurlcs_id { get; set; }
        public int genre { get; set; }
    }
}
