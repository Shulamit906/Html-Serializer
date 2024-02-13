
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practi_code2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; } = "";
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();


        //public override string ToString()
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    if (Id != null)
        //        stringBuilder.Append($" Id: {Id}");
        //    //stringBuilder.Append($" Name: {Name}");

        //    if (Attributes.Count > 0)
        //    {
        //        stringBuilder.Append(" Attributes:");
        //        foreach (var attribute in Attributes)
        //        {
        //            stringBuilder.Append($"  {attribute.Key} - {attribute.Value}");
        //        }
        //    }

        //    if (Classes.Count > 0)
        //    {
        //        stringBuilder.Append(" Classes:");
        //        foreach (var className in Classes)
        //        {
        //            stringBuilder.Append($"  {className}");
        //        }
        //    }

        //    //if (!string.IsNullOrEmpty(InnerHtml))
        //    //{
        //    //    stringBuilder.Append($" InnerHtml: {InnerHtml}");
        //    //}


        //    return stringBuilder.ToString();
        //}
        public override string ToString()
        {
            string s = "";
            
            if (Id != null) s += " Id: " + Id;
            if (Classes.Count > 0)
            {
                s += " classes: ";
                foreach (var c in Classes)
                    s += c + " ";
            }
            return s;
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement currentElement = queue.Dequeue();

                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
                yield return currentElement;
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement currentElement = this;

            while (currentElement.Parent != null)
            {
                currentElement = currentElement.Parent;
                yield return currentElement;
            }
        }

        public HashSet<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();
            foreach (var child in Descendants())
                child.FindElementsRecursive(selector, results);

            return results;
        }

        private void FindElementsRecursive(Selector selector, HashSet<HtmlElement> results)
        {
      
            if (!MatchesSelector(selector))
            {
                return;
            }

            if (selector.Child == null)
                results.Add(this);

            else
                foreach (var child in Descendants())
                {
                    child.FindElementsRecursive(selector.Child, results);
                }
        }

        private bool MatchesSelector(Selector selector)
        {

            if (selector.TagName != null && !Name.Equals(selector.TagName))
                return false;

            if (selector.Id != null && !Id.Equals(selector.Id))
                return false;

            if (selector.Classes.Count > 0 && Classes.Intersect(selector.Classes).Count()!=selector.Classes.Count)
                return false;

            return true;
        }

    }

}




