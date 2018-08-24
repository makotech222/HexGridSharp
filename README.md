# HexGridSharp
Generate a hexogonal grid in c#

Easy to use Hex grid generator, written using .net standard 2.0. 

To Use:
Call the HexGrid constructor and pass in the number of hexes you want to generate, along with a outer radius value. Each cell can be accessed through the HexGrid.Cells property.

Imports SixLabors.ImageSharp and SixLabors.ImageSharp.Drawing when ToTexture() is called.

Special thanks to https://catlikecoding.com/unity/tutorials/hex-map/part-1/ for the hex tutorial where most of the formulas come from.