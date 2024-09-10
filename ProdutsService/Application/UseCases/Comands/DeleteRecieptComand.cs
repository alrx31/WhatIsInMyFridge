using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class DeleteRecieptComand:IRequest
    {
        public string Id { get; set; }
        public DeleteRecieptComand(string id)
        {
            Id = id;
        }
        public DeleteRecieptComand() { }
    }
}
