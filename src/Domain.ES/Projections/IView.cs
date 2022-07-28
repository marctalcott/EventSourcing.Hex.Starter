using Newtonsoft.Json.Linq;

namespace Domain.ES.Projections
{
    public interface IView
    {
        JObject Payload { get; set; }
    }
}