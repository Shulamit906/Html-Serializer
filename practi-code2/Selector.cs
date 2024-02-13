using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practi_code2
{
    public class Selector
    {
        public string Id { get; set; }
        public string TagName { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }


        public override string ToString()
        {
            string s = "";
            if (TagName != null) s += "Name: " + TagName;
            if (Id != null) s += " Id: " + Id;
            if (Classes.Count > 0)
            {
                s += " classes: ";
                foreach (var c in Classes)
                    s += c + " ";
            }
            return s;
        }
        public static Selector ParseSelector(string query)
        {
            var root = new Selector();
            Selector selector = root;
            var selectors = query.Split(" ");
            foreach (var s in selectors)
            {
               
                var queries = new Regex("(?=[#\\.])").Split(s).Where(s => s.Length > 0).ToArray();
                foreach (var q in queries)
                {
                    if (q.StartsWith("#"))
                        selector.Id = q.Substring(1);
                    else if (q.StartsWith("."))
                        selector.Classes.Add(q.Substring(1));
                    else if (HtmlHelper.Instance.Tags.Contains(q))
                        selector.TagName = q;
                    else
                        Console.WriteLine($"Invalid value: {q}");
                }
                Selector newSelector = new Selector();
                newSelector.Parent = selector;
                selector.Child = newSelector;
                selector = newSelector;
            }
            selector.Parent.Child = null;
            return root;
        }
    }
}
