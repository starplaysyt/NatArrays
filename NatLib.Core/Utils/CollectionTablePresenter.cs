using System.ComponentModel;
using System.Reflection;
using System.Text;
using NatLib.Core.Enums;
using NatLib.Core.Unification;

namespace NatLib.Core.Utils;

public class CollectionTablePresenter<T> where T : class
{
    private readonly PropertyInfo[] _properties;
    private int[] _lengths;
    private bool _needUpdate = true;
    private string _tableContainer = "";

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
        if (!_needUpdate) return _tableContainer;
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
            SpanCharUtils.WrapJoinSpan
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
                    SpanCharUtils.WrapJoinSpan);
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
        _tableContainer = bld.ToString();
        
        var stringBuilderLengthCalc = (lineLength + 1) * (4 + dataStrings.Length) - 1;
        var stringBuilderLengthAct = bld.Length;
        if (stringBuilderLengthAct != stringBuilderLengthCalc)
            Console.WriteLine($"Result and planting mismatch! Act: {stringBuilderLengthAct} Calc: {stringBuilderLengthCalc}");
        
        return _tableContainer;
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
            SpanCharUtils.WrapJoinSpan
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
                SpanCharUtils.WrapJoinSpan)
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
}