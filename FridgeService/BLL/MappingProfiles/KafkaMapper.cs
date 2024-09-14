using AutoMapper;


namespace BLL.MappingProfiles
{
    public class KafkaMapper:Profile
    {
        public KafkaMapper()
        {
            CreateMap<(string, int), DAL.Entities.MessageBrokerEntities.Product>()
                .ConstructUsing(x => new DAL.Entities.MessageBrokerEntities.Product(new List<string>{x.Item1}, x.Item2));
            
            CreateMap<(List<string>, int), DAL.Entities.MessageBrokerEntities.Product>()
                .ConstructUsing(x => new DAL.Entities.MessageBrokerEntities.Product(x.Item1, x.Item2));
        }
    }
}
