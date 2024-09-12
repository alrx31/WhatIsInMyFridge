using MediatR;

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
