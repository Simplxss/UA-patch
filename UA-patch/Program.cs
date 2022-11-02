using System;
using System.Drawing;
using System.IO;

byte[] UA;
byte[] UA_CN;
byte[] UA_OS;
byte[] UA_key;

try
{
    UA = File.ReadAllBytes("UserAssembly.dll");
    UA_CN = File.ReadAllBytes("UA CN.txt");
    UA_OS = File.ReadAllBytes("UA OS.txt");

    try
    {
        Console.WriteLine("Used key: " + args[0]);
        UA_key = File.ReadAllBytes(args[0]);
    }
    catch
    {
        Console.WriteLine("Used key: UA key.txt");
        UA_key = File.ReadAllBytes("UA key.txt");
    }
}
catch (IOException e)
{
    Console.WriteLine(e.Message + "\n Cannot open file.");
    return;
}

if (UA_CN.Length != UA_key.Length || UA_OS.Length != UA_key.Length)
{
    Console.WriteLine("key len doesn't match.");
    return;
}

int Offset = 0;
int DataLength;

List<HexReplaceEntity> UA_OS_list = new List<HexReplaceEntity>();
while ((DataLength = UA_OS.Length - Offset) > 0)
{
    if (DataLength > 8)
        DataLength = 8;
    HexReplaceEntity hexReplaceEntity = new HexReplaceEntity();
    hexReplaceEntity.oldValue = new byte[8];
    Buffer.BlockCopy(UA_OS, Offset, hexReplaceEntity.oldValue, 0, DataLength);
    hexReplaceEntity.newValue = new byte[8];
    Buffer.BlockCopy(UA_key, Offset, hexReplaceEntity.newValue, 0, DataLength);
    UA_OS_list.Add(hexReplaceEntity);
    Offset += DataLength;
}

byte[] UA_OS_patched = HexUtility.Replace(UA, UA_OS_list);

if (!HexUtility.EqualsBytes(UA, UA_OS_patched))
{
    try
    {
        File.WriteAllBytes("UserAssembly.dll.ospatch", UA_OS_patched);
    }
    catch (IOException e)
    {
        Console.WriteLine(e.Message + "\n Cannot write to file.");
        return;
    }
    return;
}


Offset = DataLength = 0;

List<HexReplaceEntity> UA_CN_list = new List<HexReplaceEntity>();
while ((DataLength = UA_CN.Length - Offset) > 0)
{
    if (DataLength > 8)
        DataLength = 8;
    HexReplaceEntity hexReplaceEntity = new HexReplaceEntity();
    hexReplaceEntity.oldValue = new byte[8];
    Buffer.BlockCopy(UA_CN, Offset, hexReplaceEntity.oldValue, 0, DataLength);
    hexReplaceEntity.newValue = new byte[8];
    Buffer.BlockCopy(UA_key, Offset, hexReplaceEntity.newValue, 0, DataLength);
    UA_CN_list.Add(hexReplaceEntity);
    Offset += DataLength;
}


byte[] UA_CN_patched = HexUtility.Replace(UA, UA_CN_list);

if (!HexUtility.EqualsBytes(UA, UA_CN_patched))
{
    try
    {
        File.WriteAllBytes("UserAssembly.dll.cnpatch", UA_CN_patched);
    }
    catch (IOException e)
    {
        Console.WriteLine(e.Message + "\n Cannot write to file.");
        return;
    }
    return;
}
