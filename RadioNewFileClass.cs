using UnityEngine;
using System.Collections;
using System;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Hex.HexConvertors.Extensions;

public class RadioNewFileClass : IComparable<RadioNewFileClass>
{
    public BigInteger fileId;
    public string name;
    public string artist;

    
    public RadioNewFileClass(BigInteger newFileId, string newName, string newArtist)
    {
        fileId = newFileId;
        name = newName;
        artist = newArtist;
    }
    
    public int CompareTo(RadioNewFileClass other)
    {
        if(other == null)
        {
            return 1;
        }
        
        return (int)Nethereum.Util.UnitConversion.Convert.FromWei(fileId - other.fileId, 18);
    }
}