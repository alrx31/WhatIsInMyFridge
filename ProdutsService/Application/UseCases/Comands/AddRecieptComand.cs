using MediatR;

namespace Application.UseCases.Comands
{
    public class AddRecieptComand:IRequest
    {
        public string Name { get; set; }
        public TimeSpan CookDuration { get; set; }
        public int Portions { get; set; }
        public int Kkal { get; set; }
    }
}
