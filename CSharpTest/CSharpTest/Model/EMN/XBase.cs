using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Models
{
    public class XBase
    {
        Dictionary<string, string> nameValues = new Dictionary<string, string>();
        public Dictionary<string, string> NameValues { get { return nameValues; } }
    }
}
