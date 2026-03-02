using System.ComponentModel;
using System.Reflection;
using System.Text;
using NatLib.Core.Enums;
using NatLib.Core.Unification;

namespace NatLib.Core.Utils;

public class CollectionTablePresenter<T>
{
    private readonly PropertyInfo[] _properties;
    private int[] _lengths;
    private bool _needUpdate = true;
    private string _buildedTable = "";

    public BindingList<T> Collection { get; }
    
    public StringStructuralConfiguration Configuration { get; }

    public bool ShowNumbers
    {
        get => field;
        set
        {
            field = value;
            Invalidate();
        }
    }

    public CollectionTablePresenter(BindingList<T> collection, StringStructuralConfiguration? configuration = null)
    {
        _properties = ReflectionUtils.GetPropertyInfos(typeof(T));
        _lengths = new int[_properties.Length];
        
        Collection = collection;

        Collection.ListChanged += (_, _) => Invalidate();
        
        Configuration = configuration ?? StringStructuralConfiguration.Instance;
    }

    public void Invalidate() => _needUpdate = true;

    public string BuildTable()
    {
        if (!_needUpdate) return _buildedTable;
        var showNumbers = ShowNumbers;
        var numbersLength = ShowNumbers ? Collection.Count.ToString().Length : 0;
        var configuration = Configuration;
        _lengths = GetMaxLengthArray(Collection.AsEnumerable(), _properties, numbersLength);
        
        Console.WriteLine(string.Join('\n', _lengths.Zip(Collection).Select((tuple) => $"{tuple.First} : {tuple.Second}")));

        var lineLength = 1 + _lengths.Sum(l => l + 3);

        Console.WriteLine("LineLength: " + lineLength);
        
        var headerTopDivider = StringUtils.GenerateJoin(
            configuration.CornerTopLeft, 
            configuration.CornerTopRight, 
            configuration.HorizontalLine, 
            configuration.SectionTBottom, 
            _lengths); // Header upper block
        
        var header = string.Create( // generating headers
                lineLength,
                (
                    Array: showNumbers 
                                ? ReflectionUtils.GetPropertiesNames<T>(_properties).Prepend("№").ToArray() 
                                : ReflectionUtils.GetPropertiesNames<T>(_properties).ToArray(),
                    Lengths: _lengths,
                    Separator: configuration.VerticalLine,
                    Alignment: Alignment.Begin
                ),
            StringUtils.WrapJoinSpan
            );

        var headerBottomDivider = StringUtils.GenerateJoin(
            configuration.SectionTRight,
            configuration.SectionTLeft,
            configuration.HorizontalLine,
            configuration.SectionX,
            _lengths);

        var dataStrings = Collection
            .Select((item, i) =>
            {
                var dataRequest = showNumbers 
                    ? ReflectionUtils.GetPropertiesToString(_properties, item).Prepend(i.ToString()).ToArray()
                    : ReflectionUtils.GetPropertiesToString(_properties, item).ToArray();
                return string.Create( // generating data strings
                    lineLength,
                    (
                        Array: dataRequest,
                        Lengths: _lengths,
                        Separator: configuration.VerticalLine,
                        Alignment: Alignment.Begin
                    ),
                    StringUtils.WrapJoinSpan);
            }).ToArray();
        
        var footerDivider = StringUtils.GenerateJoin(
            configuration.CornerBottomLeft,
            configuration.CornerBottomRight,
            configuration.HorizontalLine,
            configuration.SectionTTop,
            _lengths);

        StringBuilder bld = new(
            (lineLength + 1) * (4 + dataStrings.Length) - 1);
        
        // if (headerTopDivider.Length != lineLength 
        //     || header.Length != lineLength
        //     || headerBottomDivider.Length != lineLength
        //     || dataStrings.Select(ds => ds.Length != lineLength).All(b => b )
        //     || footerDivider.Length != lineLength)
        //     Console.WriteLine("Exception in lines!");
        
        bld.AppendLine(headerTopDivider);
        bld.AppendLine(header);
        bld.AppendLine(headerBottomDivider);
        bld.AppendJoin('\n', dataStrings);
        bld.Append('\n');
        bld.Append(footerDivider);
        _buildedTable = bld.ToString();
        
        var stringBuilderLengthCalc = (lineLength + 1) * (4 + dataStrings.Length) - 1;
        var stringBuilderLengthAct = bld.Length;
        if (stringBuilderLengthAct != stringBuilderLengthCalc)
            Console.WriteLine($"Result and planting mismatch! Act: {stringBuilderLengthAct} Calc: {stringBuilderLengthCalc}");
        
        return _buildedTable;
    }

    private string BuildTableNonNumerical()
    {
        var configuration = Configuration;

        _lengths = GetMaxLengthArray(Collection.AsEnumerable(), _properties);

        var lineLength = 1 + _lengths.Sum(l => l + 3);
        
        var headerTopDivider = StringUtils.GenerateJoin(
            configuration.CornerTopLeft, 
            configuration.CornerTopRight, 
            configuration.HorizontalLine, 
            configuration.SectionTBottom, 
            _lengths);
        
        var header = string.Create( // generating headers
                lineLength,
                (
                    ReflectionUtils.GetPropertiesNames<T>(_properties).ToArray(),
                    _lengths,
                    configuration.VerticalLine,
                    Alignment.Begin
                ),
            StringUtils.WrapJoinSpan
            );

        var headerBottomDivider = StringUtils.GenerateJoin(
            configuration.SectionTRight,
            configuration.SectionTLeft,
            configuration.HorizontalLine,
            configuration.SectionX,
            _lengths);

        var dataStrings = Collection
            .AsParallel()
            .Select(item => string.Create( // generating data strings
                lineLength,
                (
                    ReflectionUtils.GetPropertiesToString(_properties, item).ToArray(),
                    _lengths,
                    configuration.VerticalLine,
                    Alignment.Begin
                ),
                StringUtils.WrapJoinSpan)
            ).ToArray();
        
        var footerDivider = StringUtils.GenerateJoin(
            configuration.CornerBottomLeft,
            configuration.CornerBottomRight,
            configuration.HorizontalLine,
            configuration.SectionTTop,
            _lengths);

        StringBuilder bld = new(
            headerBottomDivider.Length + 
            header.Length + 
            headerBottomDivider.Length + 
            dataStrings.Sum(t => t.Length) + 
            footerDivider.Length +
            5 + dataStrings.Length);
        
        bld.AppendLine(headerTopDivider);
        bld.AppendLine(header);
        bld.AppendLine(headerBottomDivider);
        bld.AppendJoin('\n', dataStrings);
        bld.Append('\n');
        bld.AppendLine(footerDivider);
        return bld.ToString();
    }

    private int[] GetMaxLengthArray(IEnumerable<T> collection, PropertyInfo[] properties, int numericsLength = 0)
    {
        var query = properties.Select(property =>
        {
            return collection.Select(item => (property.GetValue(item)?.ToString() ?? "").Length)
                .Append(property.Name.Length).Max();
        });
        
        return numericsLength > 0 ? query.ToArray().Prepend(numericsLength).ToArray() : query.ToArray();
    }

    public string BuildTable<T>(List<T> collection) where T : class
    {
        var type = typeof(T);
        var properties = ReflectionUtils.GetPropertyInfos(type);
        var lengths = new int[properties.Length];

        foreach (var element in collection)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var localLength = properties[i].GetValue(element)?.ToString()?.Length ?? 5;
                if (localLength > lengths[i]) lengths[i] = localLength;
            }
        }

        var lineNumberLength = collection.Count.ToString().Length;

        var lineLength = 1 + lengths.Sum(l => l + 2) + lengths.Length;

        var returnString = string.Create(
            lineLength * (collection.Count + 2 + 2 + 2),
            (properties, lengths, collection, StringStructuralConfiguration.Instance, lineLength, lineNumberLength),
            static (span, tuple) =>
            {
                var (properties, lengths, collection, conf, lineLength, headers) = tuple;

                var pointer = 0;

                WriteChar(span, conf.CornerTopLeft);
                for (var i = 0; i < lengths.Length; i++)
                {
                    Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                    WriteChar(span,
                        i == lengths.Length - 1 ? conf.CornerTopRight : conf.SectionTBottom);
                }

                WriteString(span, "\r\n");

                WriteChar(span, conf.VerticalLine);

                for (var i = 0; i < properties.Length; i++)
                {
                    var header = properties[i].Name;

                    if (header.Length > lengths[i])
                        header = header[..lengths[i]];

                    WriteChar(span, conf.EmptyBlock);
                    WriteString(span, header);
                    Repeat(span, conf.EmptyBlock, lengths[i] - header.Length);
                    WriteChar(span, conf.EmptyBlock);
                    WriteChar(span, conf.VerticalLine);
                }

                WriteString(span, "\r\n");

                WriteChar(span, conf.SectionTRight);
                for (var i = 0; i < lengths.Length; i++)
                {
                    Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                    WriteChar(span,
                        i == lengths.Length - 1 ? conf.SectionTLeft : conf.SectionX);
                }

                WriteString(span, "\r\n");

                var index = 1;

                foreach (var item in collection)
                {
                    WriteChar(span, conf.VerticalLine);

                    var indexStr = index.ToString();
                    if (indexStr.Length > lengths[0])
                        indexStr = indexStr[..lengths[0]];

                    WriteChar(span, conf.EmptyBlock);
                    WriteString(span, indexStr);
                    Repeat(span, conf.EmptyBlock, lengths[0] - indexStr.Length);
                    WriteChar(span, conf.EmptyBlock);
                    WriteChar(span, conf.VerticalLine);

                    for (var p = 1; p < properties.Length; p++)
                    {
                        var value = properties[p].GetValue(item)?.ToString() ?? "null";

                        if (value.Length > lengths[p])
                            value = value[..lengths[p]];

                        WriteChar(span, conf.EmptyBlock);
                        WriteString(span, value);
                        Repeat(span, conf.EmptyBlock, lengths[p] - value.Length);
                        WriteChar(span, conf.EmptyBlock);
                        WriteChar(span, conf.VerticalLine);
                    }

                    WriteString(span, "\r\n");

                    if (index < collection.Count)
                    {
                        WriteChar(span, conf.SectionTRight);

                        for (var i = 0; i < lengths.Length; i++)
                        {
                            Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                            WriteChar(span,
                                i == lengths.Length - 1 ? conf.SectionTLeft : conf.SectionX);
                        }

                        WriteString(span, "\r\n");
                    }

                    index++;
                }

                WriteChar(span, conf.CornerBottomLeft);
                for (var i = 0; i < lengths.Length; i++)
                {
                    Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                    WriteChar(span,
                        i == lengths.Length - 1 ? conf.CornerBottomRight : conf.SectionTTop);
                }

                return;

                void Repeat(Span<char> spn, char c, int count)
                {
                    var sl = spn.Slice(pointer, count);
                    for (var i = 0; i < count; i++)
                        sl[i] = c;
                    pointer += count;
                }

                void WriteString(Span<char> spn, string s)
                {
                    s.AsSpan().CopyTo(spn[pointer..]);
                    pointer += s.Length;
                }

                void WriteChar(Span<char> spn, char c)
                {
                    spn[pointer++] = c;
                }
            }
        );

        return returnString;
    }
}