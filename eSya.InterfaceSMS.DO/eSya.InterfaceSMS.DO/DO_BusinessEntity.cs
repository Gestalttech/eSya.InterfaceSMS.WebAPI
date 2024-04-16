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
        public bool IsMultiSegmentApplicable { get; set; }
        public string BusinessUnitType { get; set; }
        public int NoOfUnits { get; set; }
        public int ActiveNoOfUnits { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        //public string FormId { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }

    }
    public class DO_BusinessLocation
    {
        public int BusinessId { get; set; }
        public int LocationId { get; set; }
        public int BusinessKey { get; set; }
        public string ShortDesc { get; set; } = null!;
        public string LocationDescription { get; set; } = null!;
        public string BusinessName { get; set; } = null!;
        public int Isdcode { get; set; }
        public int CityCode { get; set; }
        public string CurrencyCode { get; set; } = null!;
        public bool? TolocalCurrency { get; set; }
        public bool TocurrConversion { get; set; }
        public bool TorealCurrency { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? CurrencyName { get; set; }
        public int SegmentId { get; set; }
        
    }

    public class DO_CountryCodes
    {
        public int Isdcode { get; set; }
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public string CountryFlag { get; set; } = null!;
        public string CurrencyCode { get; set; } = null!;
        public string MobileNumberPattern { get; set; } = null!;
        public int Nationality { get; set; }
        public bool IsPoboxApplicable { get; set; }
        public string? PoboxPattern { get; set; } = null!;
        public bool IsPinapplicable { get; set; }
        public string? PincodePattern { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int UserID { get; set; }
        public string TerminalID { get; set; } = null!;
        public string? CurrencyName { get; set; } = null!;
        public string DateFormat { get; set; }
        public string ShortDateFormat { get; set; }

    }
}
