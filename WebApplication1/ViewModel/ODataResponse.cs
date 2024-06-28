using System.Text.Json.Serialization;

namespace WebApplication1.ViewModel;

public class ODataResponse<T>
{
    [JsonPropertyName("@odata.context")]
    public string ODataContext { get; set; }
    public List<T> Value { get; set; }
}