using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RelayHostData
{
    public string joinCode;
    public string IPv4Adress;
    public ushort Port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] Key;
}
