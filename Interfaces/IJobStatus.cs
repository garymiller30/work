using System.Drawing;


namespace Interfaces
{
    public interface IJobStatus
    {
        object Id { get; set; }
        
        string Name { get; set; }
        
        int Code { get; set; }
        
        bool IsDefault { get; set; }
        
        string ImgBase64 { get; set; }
        
        
        Image Img { get; set; }


    }
}
