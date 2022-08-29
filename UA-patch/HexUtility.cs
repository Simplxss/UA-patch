
/// <summary>
/// 二进制数据 操作
/// </summary>
public class HexUtility
{
    /// <summary>
    /// 比较当前byte数组与另一数组是否相等。
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="target">需要比较的数组。</param>
    /// <returns></returns>
    public static bool EqualsBytes(byte[] b1, params byte[] b2)
    {
        if (b1.Length != b2.Length)
            return false;
        for (int i = 0; i < b1.Length; i++)
        {
            if (b1[i] != b2[i])
                return false;
        }
        return true;
    }

    /// <summary>
    /// 二进制替换,如果没有替换则返回原数组对像的复本.
    /// </summary>
    /// <param name="sourceByteArray">源数据</param>
    /// <param name="replaces">需要替换的数据集合</param>
    public static byte[] Replace(byte[] sourceByteArray, List<HexReplaceEntity> replaces)
    {
        //创建新数据
        byte[] newByteArray = new byte[sourceByteArray.Length];
        Buffer.BlockCopy(sourceByteArray, 0, newByteArray, 0, sourceByteArray.Length);
        //替换数据
        int offset = 0;
        foreach (HexReplaceEntity rep in replaces)
        {
            if (EqualsBytes(rep.oldValue, rep.newValue))
            {
                continue;
            }

            for (; offset < sourceByteArray.Length; offset++)
            {
                //查找要替换的数据
                if (sourceByteArray[offset] == rep.oldValue[0])
                {
                    if (sourceByteArray.Length - offset < rep.oldValue.Length)
                        break;

                    bool find = true;
                    for (int i = 1; i < rep.oldValue.Length - 1; i++)
                    {
                        if (sourceByteArray[offset + i] != rep.oldValue[i])
                        {
                            find = false;
                            break;
                        }
                    }
                    if (find)
                    {
                        Buffer.BlockCopy(rep.newValue, 0, newByteArray, offset, rep.newValue.Length);
                        offset += (rep.newValue.Length - 1);
                        break;
                    }
                }
            }
        }
        return newByteArray;
    }
}

/// <summary>
/// 替换数据实体
/// </summary>
public class HexReplaceEntity
{
    /// <summary>
    /// 需要替换的原始值
    /// </summary>
    public byte[] oldValue { get; set; }

    /// <summary>
    /// 新值
    /// </summary>
    public byte[] newValue { get; set; }

    /// <summary>
    /// 默认开始结束标记
    /// </summary>
    internal int start = -1;
    /// <summary>
    /// 默认开始结束标记
    /// </summary>
    internal int end = -1;

    //当前查找到的索引
    internal int oldCurindex = 0;

}
