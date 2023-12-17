using System.ComponentModel.DataAnnotations;

namespace Common.ConfigOptions;

public class EndPointConfig
{
    public const string EndPointSection = "EndPoint";

    [Required(AllowEmptyStrings = false)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [MinLength(1)]
    public required string UserApiEndPoint { get; set; }
}