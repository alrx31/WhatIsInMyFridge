using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Queries
{
    public class GetUserQueryByIdQuery:IRequest<User>
    {
        public int Id { get; set; }

        public GetUserQueryByIdQuery(int id)
        {
            this.Id = id;
        }
        public GetUserQueryByIdQuery()
        {
        }
    }
}
