namespace TIFFB2JDF
{
    public enum PatternEnum
    {
        Any,
        Color,
        Customer,
        FrontBack,
        Separator,
        JobName,
        PageNumber,
        
    }

    //по какому шаблону разбирать имя файла
    public class NamePattern
    {
        public PatternEnum PatternEnums { get; set; } = PatternEnum.Any;
        public string  Separator { get; set; }
        
    }
}
