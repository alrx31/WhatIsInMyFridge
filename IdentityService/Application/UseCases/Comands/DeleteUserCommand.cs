using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class DeleteUserCommand(
        int Id,
        int InitiatorId
        ):IRequest
    {
        public int id { get; set; } = Id;
        public int initiatorId { get; set; } = InitiatorId;
    }
}
