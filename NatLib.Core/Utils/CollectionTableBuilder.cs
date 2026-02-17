using NatLib.Core.Unification;

namespace NatLib.Core.Utils;

public static class CollectionTableBuilder
{
    public static string BuildTable<T>(List<T> collection) where T : class
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