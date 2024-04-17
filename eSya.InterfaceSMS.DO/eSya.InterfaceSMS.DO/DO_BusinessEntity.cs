using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.InterfaceSMS.DO
{
    public class DO_BusinessEntity
    {
        public int BusinessId { get; set; }
        public string BusinessDesc { get; set; }
        
    }
    public class DO_BusinessLocation
    {
        public int BusinessKey { get; set; }
        public string LocationDescription { get; set; } = null!;
        
    }

    public class DO_CountryCodes
    {
        public int Isdcode { get; set; }
        public string CountryName { get; set; } = null!;
        

    }
}
