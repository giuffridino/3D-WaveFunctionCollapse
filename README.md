# 3D Wave Function Collapse - Castle Run
This Unity project expands the WFC algorithm we had studied in the coursework from 2D to 3D. The final objective was to procedurally generate a castle maze traversable from the ground floor to the top floor. We wrapped everything up as a simple mini-game called "**Castle Run**" you can download and play
play [here](https://giuffridino.itch.io/castle-run). This project was completed for the Generative Methods for Computer Graphics course at DTU (Technical University of Denmark).

# Algorithm
The WFC script partitions the environment in a 3D cubic grid of cells. In each cell we place one of the different object tiles we have prepared. Each tile object is described by its neighboring constraints which we manually input. These constraints
make sure that in all 6 directions (up/down/right/left/front/back) there are no conflicting neighbors as well as in diagonal directions to make sure stair tiles are fully traversable. We also created some editor scripts to facilitate the inputting of constraints.

# Path Generator
Before running the WFC algorithm we create a path going from the ground floor to the top floor. This is done by blending the methods of random placement and shortest path together. By alternating these two methods we can provide a compelling challenge to the 
player who is finding this path, as it is not as easy as shortest path, but not so difficult and chaotic as random placement (which doesn't even ensure that a path is found in reasonable time). 

# Screenshots
<img src="https://github.com/giuffridino/3D-WaveFunctionCollapse/assets/123559640/325383f9-f582-4162-aac2-a809d114cb3c" width="450" />
<img src="https://github.com/giuffridino/3D-WaveFunctionCollapse/assets/123559640/dd9bf2ea-3577-4aab-936b-76a4d7ade0b2" width="450" />
<img src="https://github.com/giuffridino/3D-WaveFunctionCollapse/assets/123559640/185d665d-11ca-4f73-9ba2-459b9636bf31" width="450" />
<img src="https://github.com/giuffridino/3D-WaveFunctionCollapse/assets/123559640/1e38bd32-c314-402b-8718-edd630c3eefc" width="450" />

