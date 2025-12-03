using Authentication.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Commands
{
    public class SetForgotPasswordOTPCommand : IRequest<Response>
    {
        [Required]
        public string email { get; set; }
    }
}
