using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CSharpTest.Model
{
    public class FoundationMongo
    {
        public ObjectId _id { get; set; }
        public Guid RepeatId { get; set; }
        public string FoundName { get; set; }
        public string Location { get; set; }
        public string NetAssets { get; set; }
        public string OpenPolitics { get; set; }
        public string Website { get; set; }
        public string CreditCode { get; set; }
        public string Chairman { get; set; }
        public string Secretary { get; set; }
        public string EstablishTime { get; set; }
        public string Tel { get; set; }
        public string OriginalFund { get; set; }
        public string RegisterDepart { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Purpose { get; set; }
        public DateTime CreadAt { get; set; }
        public string FoundationType { get; set; }
    }

    public class Foundation_projectMongo
    {
        public ObjectId _id { get; set; }
        public ObjectId Fid { get; set; }
        public string ProjectName { get; set; }
        public string ExecutiveYear { get; set; }
        public string YearIncome { get; set; }
        public string YearExpenditure { get; set; }
        public string AttentionField { get; set; }
        public string FundUse { get; set; }
        public string BenefitGroup { get; set; }
        public string ProjectBrief { get; set; }
    }
}
