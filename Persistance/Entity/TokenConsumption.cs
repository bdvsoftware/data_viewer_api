using System.ComponentModel.DataAnnotations.Schema;

namespace DataViewerApi.Persistance.Entity;

public class TokenConsumption
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int FrameId { get; set; }
    public int ResponseStatus { get; set; }
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    
    public virtual Frame Frame { get; set; } = null!;

    public TokenConsumption(int frameId, int responseStatus, int promptTokens, int completionTokens)
    {
        FrameId = frameId;
        ResponseStatus = responseStatus;
        PromptTokens = promptTokens;
        CompletionTokens = completionTokens;
    }
}