using NatLib.Core.Unification;

namespace NatLib.Core.Utils;

public static class CollectionTableBuilder
{
    public static string BuildTable<T>(List<T> collection) where T : class
    {
        var type = typeof(T);
        var properties = ReflectionUtils.GetPropertyInfos(type);
        var lengths = new int[properties.Length];
        
        // Finding max lengths
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

                // ┌───────────────── верхняя граница ────────────────
                WriteChar(span, conf.CornerTopLeft);
                for (int i = 0; i < lengths.Length; i++)
                {
                    Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                    if (i == lengths.Length - 1)
                        WriteChar(span, conf.CornerTopRight);
                    else
                        WriteChar(span, conf.SectionTBottom);
                }
                WriteString(span, "\r\n");

                // ───────────────── заголовок ─────────────────
                WriteChar(span, conf.VerticalLine);

                for (int i = 0; i < properties.Length; i++)
                {
                    var header = properties[i].Name;

                    if (header.Length > lengths[i])
                        header = header.Substring(0, lengths[i]);

                    WriteChar(span, conf.EmptyBlock);
                    WriteString(span, header);
                    Repeat(span, conf.EmptyBlock, lengths[i] - header.Length);
                    WriteChar(span, conf.EmptyBlock);
                    WriteChar(span, conf.VerticalLine);
                }

                WriteString(span, "\r\n");

                // ─────────────── разделитель после заголовка ────────────────
                WriteChar(span, conf.SectionTRight);
                for (int i = 0; i < lengths.Length; i++)
                {
                    Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                    if (i == lengths.Length - 1)
                        WriteChar(span, conf.SectionTLeft);
                    else
                        WriteChar(span, conf.SectionX);
                }
                WriteString(span, "\r\n");

                // ─────────────── строки данных с нумерацией ────────────────
                int index = 1;

                foreach (var item in collection)
                {
                    WriteChar(span, conf.VerticalLine);

                    // ------ колонка № ------
                    var indexStr = index.ToString();
                    if (indexStr.Length > lengths[0])
                        indexStr = indexStr.Substring(0, lengths[0]);

                    WriteChar(span, conf.EmptyBlock);
                    WriteString(span, indexStr);
                    Repeat(span, conf.EmptyBlock, lengths[0] - indexStr.Length);
                    WriteChar(span, conf.EmptyBlock);
                    WriteChar(span, conf.VerticalLine);

                    // ------ остальные свойства ------
                    for (int p = 1; p < properties.Length; p++)
                    {
                        var value = properties[p].GetValue(item)?.ToString() ?? "null";

                        if (value.Length > lengths[p])
                            value = value.Substring(0, lengths[p]);

                        WriteChar(span, conf.EmptyBlock);
                        WriteString(span, value);
                        Repeat(span, conf.EmptyBlock, lengths[p] - value.Length);
                        WriteChar(span, conf.EmptyBlock);
                        WriteChar(span, conf.VerticalLine);
                    }

                    WriteString(span, "\r\n");

                    // разделитель строк
                    if (index < collection.Count)
                    {
                        WriteChar(span, conf.SectionTRight);

                        for (int i = 0; i < lengths.Length; i++)
                        {
                            Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                            if (i == lengths.Length - 1)
                                WriteChar(span, conf.SectionTLeft);
                            else
                                WriteChar(span, conf.SectionX);
                        }

                        WriteString(span, "\r\n");
                    }

                    index++;
                }

                // ─────────────── нижняя граница ────────────────
                WriteChar(span, conf.CornerBottomLeft);
                for (int i = 0; i < lengths.Length; i++)
                {
                    Repeat(span, conf.HorizontalLine, lengths[i] + 2);

                    if (i == lengths.Length - 1)
                        WriteChar(span, conf.CornerBottomRight);
                    else
                        WriteChar(span, conf.SectionTTop);
                }

                return;

                void Repeat(Span<char> spn, char c, int count)
                {
                    var sl = spn.Slice(pointer, count);
                    for (int i = 0; i < count; i++)
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