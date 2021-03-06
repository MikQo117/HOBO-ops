﻿using System;
using UnityEngine;

[Serializable]
public class PixelData
{
    public Color PixelColor;
    public Vector2 Position;

    public enum Tile
    {
        Null,
        Road,
        Sidewalk,
        Grass,
        ParkingLot
    }

    public Tile TileType;
}