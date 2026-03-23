using System.ComponentModel;
using System.Reflection;
using System.Text;
using NatLib.Core.Enums;
using NatLib.Core.Unification;

namespace NatLib.Core.Utils;

// TODO: Deal with generic limitation, causes crash when there is nothing to reflect
public class CollectionTablePresenter<T>
{
    private readonly PropertyInfo[] _properties;
    private int[] _lengths;
    private bool _needUpdate = true;
    private string _tableContainer = "";

    public BindingList<T> Collection { get; }

    public StringStructuralConfiguration Configuration { get; }

    public bool ShowNumbers
    { get => field;
      set
      { field = value;
        Invalidate(); } }

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

        var lineLength = 1 + _lengths.Sum(l => l + 3);

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

        bld.AppendLine(headerTopDivider);
        bld.AppendLine(header);
        bld.AppendLine(headerBottomDivider);
        bld.AppendJoin('\n', dataStrings);
        bld.Append('\n');
        bld.Append(footerDivider);
        _tableContainer = bld.ToString();

        return _tableContainer;
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