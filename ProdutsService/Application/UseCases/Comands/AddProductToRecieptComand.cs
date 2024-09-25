using Application.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class AddProductToRecieptComand:IRequest
    {
        public AddProductToRecieptDTO Model { get; set; }

        public AddProductToRecieptComand(AddProductToRecieptDTO model)
        {
            Model = model;
        }

        public AddProductToRecieptComand() { }
    }
}
