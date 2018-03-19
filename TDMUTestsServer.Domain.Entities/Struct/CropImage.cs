using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Entities.Struct
{
    public struct CropImage
    {
        public string PathPhoto { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string OriginalUrl { get; set; }
        public string Scale { get; set; }
    }
}