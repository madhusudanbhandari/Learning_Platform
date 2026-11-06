namespace lp.Domain.Entities;

public class Lesson
{
    public int Id{get;set;}
    public int CourseId{get;set;}
    public Course? Course{get;set;}
    public string Title{get;set;}=string.Empty;
    public int VideoAssetId{get;set;}
    public VideoAsset videoAsset{get;set;}=null!;
}