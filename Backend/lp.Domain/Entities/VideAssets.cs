using lp.Domain.Entities.Enum;

namespace lp.Domain.Entities;

public class VideoAsset
{
    public int Id{get;set;}
    public string StorageKey{get;set;}=string.Empty;
    public string OriginalFileName{get;set;}=string.Empty;
    public string ContentType{get;set;}=string.Empty;
    public long FileSize{get;set;}
    public double? Duration{get;set;}

    public VideoStatus videoStatus{get;set;}
    public DateTime CreatedAt{get;set;}
}