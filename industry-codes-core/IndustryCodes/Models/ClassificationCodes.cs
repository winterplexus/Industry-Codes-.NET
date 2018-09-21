//
//  ClassificationCodes.cs
//
//  Copyright (c) Wiregrass Code Technology 2018
//
namespace IndustryCodes.Models
{
    public partial class ClassificationCodes
    {
        public int Id { get; set; }
        public string IndustrySector { get; set; }
        public string IndustrySubsector { get; set; }
        public int? NorthAmericanCode { get; set; }
        public string NorthAmericanDescription { get; set; }
        public int? StandardCode { get; set; }
        public string StandardDescription { get; set; }
        public int? KindCode { get; set; }
        public string KindCodeDescription { get; set; }
        public int? NorthAmericanCode2002 { get; set; }
        public string NorthAmericanDescription2002 { get; set; }
    }
}