using System.Net.Sockets;

const string file = @"C:\Users\SaLiVa\source\repos\AdventOfCode2023Day8\AdventOfCode2023Day8\Day8Input.txt";

List<string> map = [];
string instructions = string.Empty;

using (var reader = File.OpenText(file))
{
    // First line is the instructions
    instructions = reader.ReadLine();
    
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        if(line != "")
            map.Add(line);
    }
}

Map testMap = new Map();
// Build the tree
foreach (var mapvalue in map)
{
    var value = mapvalue.Substring(0, 3);
    var left = mapvalue.Substring(7, 3);
    var right = mapvalue.Substring(12, 3);
    testMap.AddToMapList(value, left, right);
}

// Start at A, then use the instructions to go Left or Right
var start = testMap.MapList.Where(x => x.Value.Contains('A')).ToArray();

var currentNode = start;
long[] stepmap = [..new long[start.Length]];

for (var i = 0; i < start.Length; i++)
{
    var iPos = 0;
    while (!currentNode[i].Value.Contains('Z'))
    {
        if (iPos >= instructions.Length)
            iPos = 0;
        
        var direction = instructions[iPos]; 
        currentNode[i] = direction switch
            {
                 'L' => currentNode[i].Left,
                 'R' => currentNode[i].Right,
                 _ => currentNode[i]
            } ?? throw new InvalidOperationException();
        stepmap[i]++;
        iPos++;
    }
}

var result = LeastCommonMultiple(stepmap);
Console.WriteLine("Number of steps: " + result);
return;

long LeastCommonMultiple(IReadOnlyList<long> numbers)
{
    var lcm = numbers[0];
    for (var i = 1; i < numbers.Count; i++)
        lcm = lcm * numbers[i] / GreatestCommonDenominator(lcm, numbers[i]);
    return lcm;
}

long GreatestCommonDenominator(long a, long b)
{
    while (b != 0)
    {
        var temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

public class Node<T> where T : IComparable<T>
{
    public Node(T value, Node<T> left, Node<T> right)
    {
        Value = value;
        Left = left;
        Right = right;
    }

    public Node(T value)
    {
        Value = value;
        Left = null;
        Right = null;
    }

    public T Value { get; private set; }
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }
}

public class Map
{
    public Map()
    {
        MapList = new List<Node<string>>();
    }

    public List<Node<string>> MapList { get; private set; }

    public void AddToMapList(string value, string left, string right)
    {
        var newvaluenode = MapList.FirstOrDefault(x => x.Value == value);
        var newleftnode = MapList.FirstOrDefault(x => x.Value == left);
        var newrightnode = MapList.FirstOrDefault(x => x.Value == right);
        
        if (newvaluenode == null)
        {
            newvaluenode = new Node<string>(value);
            MapList.Add(newvaluenode);
        }

        if (left == right)
        {
            if (newleftnode == null & newrightnode == null)
            {
                var newleftrightnode = new Node<string>(left);
                newvaluenode.Left = newvaluenode.Right = newleftrightnode;
                MapList.Add(newleftrightnode);
            }
            else if (newleftnode != null)
            {
                newvaluenode.Right = newvaluenode.Left = newleftnode;
            }
            else if (newrightnode != null)
            {
                newvaluenode.Right = newvaluenode.Left = newrightnode;
            }
        }
        else
        {
            // Assign left and right nodes to the node
            if (newleftnode == null)
            {
                newleftnode = new Node<string>(left);
                newvaluenode.Left = newleftnode;
                MapList.Add(newleftnode);
            }
            else
            {
                newvaluenode.Left = newleftnode;
            }

            if (newrightnode == null)
            {
                newrightnode = new Node<string>(right);
                newvaluenode.Right = newrightnode;
                MapList.Add(newrightnode);
            }
            else
            {
                newvaluenode.Right = newrightnode;
            }
        }
        
    }
    
}
