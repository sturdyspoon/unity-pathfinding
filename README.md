# Pathfinding
Find paths in Unity Tilemaps with A* Search

Download and Import the Pathfinding [package](https://github.com/antonpantev/pathfinding/raw/master/Pathfinding.unitypackage) then use `AStar.FindPath()` like so:

```c#
List<Vector3> path = AStar.FindPath(tilemap, startPos, endPos);
```
