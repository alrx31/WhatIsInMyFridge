using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class DeleteUserCommand:IRequest
    {
        public int Id { get; set; }
        public int InitiatorId { get; set; }


        public DeleteUserCommand(int id, int initiatorId)
        {
            Id = id;
            InitiatorId = initiatorId;
        }
        public DeleteUserCommand()
        {
        }
    }
}
