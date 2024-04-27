using UnityEngine;
using UnityEditor;

public class Tile : MonoBehaviour
{
    public Tile[] frontNeighbors;
    public Tile[] backNeighbors;
    public Tile[] rightNeighbors;
    public Tile[] leftNeighbors;
    public Tile[] upNeighbors;
    public Tile[] downNeighbors;
 	public Tile[] front_upNeighbors;
    public Tile[] back_upNeighbors;
    public Tile[] right_upNeighbors;
    public Tile[] left_upNeighbors;
	public Tile[] front_downNeighbors;
    public Tile[] back_downNeighbors;
    public Tile[] right_downNeighbors;
    public Tile[] left_downNeighbors;

    [SerializeField] private Tile motherTile;
 
}