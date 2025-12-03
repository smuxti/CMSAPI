using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Behaviours
{
    public static class EnumReasonCodes
    {
        public const string ActivatedAgain = "002";
        public const string DeactivatedByAggregator = "003";
        public const string DeactivatedByMerchantRequest = "004";
        public const string BlockedByAggregatorTerminate = "006";
        public const string BlockedByAggregatorCompliance = "007";
        public const string BlockedByAggregatorFraud = "008";
    }
    public static class ReasonCodeHelper
    {
        public static readonly Dictionary<string, string> ReasonDescriptions = new Dictionary<string, string>
    {
        { EnumReasonCodes.ActivatedAgain, "Activated again" },
        { EnumReasonCodes.DeactivatedByAggregator, "De-activated by aggregator" },
        { EnumReasonCodes.DeactivatedByMerchantRequest, "De-activated on Merchant request" },
        { EnumReasonCodes.BlockedByAggregatorTerminate, "Blocked by aggregator - relationship terminate" },
        { EnumReasonCodes.BlockedByAggregatorCompliance, "Blocked by aggregator - compliance issues" },
        { EnumReasonCodes.BlockedByAggregatorFraud, "Blocked by aggregator - fraud" }
    };

        public static string GetReasonDescription(string code)
        {
            if (ReasonDescriptions.ContainsKey(code))
            {
                return ReasonDescriptions[code];
            }
            return "Invalid Reason Code";
        }
    }

}
