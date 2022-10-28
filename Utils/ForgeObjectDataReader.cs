using System.Runtime.InteropServices;

namespace ForgeTools;

public class ForgeObjectDataReader
{
    // c# implementation of https://github.com/ChimpsAtSea/Blam-Creation-Suite/blob/29dbb93f72063eb31bad9e6583f841fa178352cb/Tags/MandrillLib/CacheInterface/infinite/infinite_ucs_reader.h 
    // the pointer arithmetic and types are from that page


    // step over all the fields in the provided type and convert them into the expected type
    // 
    public static (T obj, int length) ByteParser<T>(ref T obj, byte[] bytes, int start = 0)
    {
        var fields = typeof(T).GetFields();
        var size = 0;
        var end = start;


        foreach (var field in fields)
        {
            var t = field.FieldType;
            size = Marshal.SizeOf(t);

            byte[] value = new byte[size];
            Array.Copy(bytes, start, value, 0, size);
            end += size;

            switch (t.Name)
            {
                case "Int32":
                    field.SetValue(obj, BitConverter.ToInt32(value));
                    break;
                case "Int64":
                    field.SetValue(obj, BitConverter.ToInt64(value));
                    break;
                case "Byte":
                    field.SetValue(obj, value[0]);
                    break;
                case "UInt32":
                    field.SetValue(obj, BitConverter.ToUInt32(value));
                    break;
                case "UInt64":
                    field.SetValue(obj, BitConverter.ToUInt64(value));
                    break;

                default:
                    throw new NotImplementedException(t.Name + " does not have a case");
                    break;
            }

        }

        return (obj, end);
    }
}

//https://github.com/ChimpsAtSea/Blam-Creation-Suite/blob/29dbb93f72063eb31bad9e6583f841fa178352cb/Tags/MandrillLib/CacheInterface/infinite/infinite_ucs_reader.h
public class USCHeader
{
    //Field Order matters(I think) reading with reflection
    public int Magic;
    public int Version;
    public long Unknown8;
    public long AssetChecksum;
    public int TagDependecyCount;
    public int NuggetCount;
    public int TagBlockCount;
    public int DataReferenceCount;
    public int StringTableSize;
    public int ZonesetDataSize;
    public int unknown19;
    public int HeaderSize;
    public int DataSize;
    public int ResourceDataSize;
    public int Unknown18;
    public byte HeaderAlignment;
    public byte TagDataAlignment;
    public byte ResourceData;
    public byte Unknow4B;
    public int Unknown4C;
}