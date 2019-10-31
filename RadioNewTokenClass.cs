using UnityEngine;
using System.Collections;
using System;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Hex.HexConvertors.Extensions;

public class RadioNewTokenClass : IComparable<RadioNewTokenClass>
{
    public BigInteger tokenId;
    public string name;
    public string artist;

    
    public RadioNewTokenClass(BigInteger newTokenId, string newName, string newArtist)
    {
        tokenId = newTokenId;
        name = newName;
        artist = newArtist;
    }
    
    public int CompareTo(RadioNewTokenClass other)
    {
        if(other == null)
        {
            return 1;
        }
        
        return (int)Nethereum.Util.UnitConversion.Convert.FromWei(tokenId - other.tokenId, 18);
    }
}