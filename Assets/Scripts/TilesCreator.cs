using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesCreator : MonoBehaviour
{
    void Start()
    {
        tiles = []
        for i in (0, width_subdivisions-1):
        for j in (0, height_subdivisions-1):
        hi = i*hW
        hj = j*hH
        tile = image.crop((hi, hj, hi + hW, hj + hH))
        tiles.append(Tile(tile, N))
        tiles.append(Tile(tile.transpose(Image.Transpose.ROTATE_90), N))
        tiles.append(Tile(tile.transpose(Image.Transpose.ROTATE_180), N))
        tiles.append(Tile(tile.transpose(Image.Transpose.ROTATE_270), N))
        for t0 in tiles:
        for i, t1 in enumerate(tiles):
        t0.compatible_adjacency(t1,i)
        return tiles
    }
    
}
