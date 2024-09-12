using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateRecieptComand:IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan CookDuration { get; set; }
        public int Portions { get; set; }
        public int Kkal { get; set; }
        
        public UpdateRecieptComand(string id, string name, TimeSpan cookDuration, int portions, int kkal)
        {
            Id = id;
            Name = name;
            CookDuration = cookDuration;
            Portions = portions;
            Kkal = kkal;
        }
        public UpdateRecieptComand() { }
    }
}
