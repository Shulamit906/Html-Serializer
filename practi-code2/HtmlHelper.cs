using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace practi_code2
{
  public class HtmlHelper
  {
    private readonly static HtmlHelper _instance = new HtmlHelper();
    public static HtmlHelper Instance => _instance;
    public List<string> Tags = new List<string>();
    public List<string> VoidTags = new List<string>();
    private HtmlHelper()
    {
      Tags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("seed/HtmlTags.json"));
      VoidTags = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("seed/HtmlVoidTags.json"));
    }

  }
}
