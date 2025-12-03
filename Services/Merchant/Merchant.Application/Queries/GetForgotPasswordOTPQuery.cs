using Merchants.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchants.Application.Queries
{
    public class GetForgotPasswordOTPQuery : IRequest<Response>
    {
        public string OTP { get; set; }
        public string email { get; set; }
        public GetForgotPasswordOTPQuery(string OTP, string email)
        {
            this.OTP = OTP;
            this.email = email;
        }
    }
}
