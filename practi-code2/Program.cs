
using practi_code2;
using System.Text.RegularExpressions;

var html = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/#html-query");
var cleanHtml = new Regex("[\\r\\n\\t]").Replace(new Regex("\\s{2,}").Replace(html, ""), "");

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToArray();

HtmlElement root = CreateChild(htmlLines[1].Split(" ")[0], null, htmlLines[1]);
BuildTree(root, htmlLines.Skip(2).ToList());
Console.WriteLine("htmlTree: ");
PrintTree(root, 0);

var list1 = root.FindElements(Selector.ParseSelector("footer.md-footer a"));
var list2 = root.FindElements(Selector.ParseSelector("div article.md-typeset"));
var list3 = root.FindElements(Selector.ParseSelector("label a.md-logo"));


Console.ReadKey();


async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
static HtmlElement CreateChild(string name, HtmlElement parent, string line)
{
    HtmlElement child = new HtmlElement { Name = name, Parent = parent };
    var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line);
    foreach (var attr in attributes)
    {
        string attrName = attr.ToString().Split("=")[0];
        string attrValue = attr.ToString().Split("=")[1].Replace("\"", "");
        if (attrName.ToLower().Equals("class"))
            foreach (var c in attrValue.Split(" "))
                child.Classes.Add(c);
        else if (attrName.ToLower().Equals("id"))
            child.Id = attrValue;
        else child.Attributes.Add(attrName, attrValue);
    }
    return child;
}
static void BuildTree(HtmlElement root, List<string> htmlLines)
{
    HtmlElement current = root;
    foreach (var line in htmlLines)
    {
        string name = line.Split(' ')[0];
        if (name == "/html")
            break;
        if (name.StartsWith("/"))
        {
            current = current.Parent;
            continue;
        }
        if (!HtmlHelper.Instance.Tags.Contains(name))
        {
            current.InnerHtml += line;
            continue;
        }
        HtmlElement child = CreateChild(name, current, line);
        current?.Children.Add(child);

        if (!(HtmlHelper.Instance.VoidTags.Contains(name) && line.EndsWith("/")))
            current = child;
    }

}
static void PrintTree(HtmlElement node, int depth)
{
    if (node != null)
    {
        // Print current node
        Console.WriteLine($"{new string(' ', depth * 2)}<{node.Name}>" + node);

        // Print children
        foreach (var child in node.Children)
        {
            PrintTree(child, depth + 1);
        }

        // Print closing tag
        Console.WriteLine($"{new string(' ', depth * 2)}</{node.Name}>");
    }
}



