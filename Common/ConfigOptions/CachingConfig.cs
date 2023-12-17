using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ConfigOptions;

public class CachingConfig
{
    public const string CachingSection = "Caching";

    [Required(AllowEmptyStrings = false)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    [MinLength(1)]
    public required string RedisConnectionEndpoint { get; set; }
}
