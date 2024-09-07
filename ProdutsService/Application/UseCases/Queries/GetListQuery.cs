using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Queries
{
    public class GetListQuery
        (
            string id
        ):IRequest<ProductsList>
    {
        public string Id = id;
    }
}
