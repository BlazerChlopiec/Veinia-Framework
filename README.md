# Veinia-Framework
C# Game Creation Tool
<br>
[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

## Features
An independent framework that extends MonoGame helping you jump straight into developing with ready-to-use features such as:
* Level->GameObject->Component System
* Loading/Unloading Levels
* Powerful Prefab-Based Level Editor
* Unity-like Input System
* Binary Level Encryption
* Aether.Physics For 2D Realtime Simulations
* Particle Effects
* 2D Camera (Shake, Lookahead)
* Screen System With Viewport Scaling
* Changing Shaders (Automatic Sprite Batching)
* Mixing SpriteBatch With GraphicsDevice Drawing
* World Tools (Instantiating, Finding By Components)
* Built-in Components (Parallax, Sprite)
* Useful Debug Tools
* Two Handy UI Systems (GeonBit & Myra)

## Installation

Veinia framework is only possible to be installed manually because of libraries such as GeonBit.UI which require referencing the Content folder

Because certain libraries (such as GeonBit.UI) require referencing the Content folder, Veinia Framework must be installed manually as a project linked to your development environment. This approach also makes it easier to customize and adapt the source to your needs.

1. **Download Source**
2. **Link To Project:** Make sure that your project references ```Veinia``` in the .csproj file (Use the correct path)
```xml
<ItemGroup>
  <ProjectReference Include="..\Veinia Framework\VeiniaFramework.csproj" />
</ItemGroup>
```
or by using the terminal

```xml
dotnet add reference "..\Veinia Framework\VeiniaFramework.csproj"
```

3. **Optional - Add To Solution:** This makes things easier if Veinia files are in a completly different directory (Use the correct path)
```xml
dotnet sln add "..\Veinia Framework\Veinia\Veinia.csproj"
```

You should now be able to compile and use ```Veinia-Framework```  in your projects.

For proper functionality, ensure that level files are copied to the output directory. This can be done by adding the following to your .csproj file: (Use the correct path)
```xml
<ItemGroup>
  <None Update="SampleLevel.veinia">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

## Examples
Veinia comes with three sample projects to help you get started.

<img src="Images/PlatformerSampleEditor.png">

Initialization Example:
```csharp
var veinia = new Veinia((Game)this, graphics);
var screen = new Screen(1280, 720, fullscreen: false)

veinia.Initialize(GraphicsDevice, Content, Window, screen, unitSize: 100, Vector2.UnitY * -20);

Globals.loader.DynamicalyLoad(new Level("Level1.veinia"));
```

Custom Level Example (To Bring Up The Built-in Editor Press TAB While Playing a Level):
```csharp
public class ForestLevel : Level
{
	public ForestLevel(string levelPath) : base(levelPath)
	{
	}

	public override void CreateScene(bool loadObjectsFromPath = true)
	{
		base.CreateScene(loadObjectsFromPath);

		var player = Instantiate(new Transform(Vector2.Zero), new List<Component>
		{
			new Sprite("Sprites/Player", layer: 0, Color.Green, pixelsPerUnit: 200),
			new Movement()
		}, isStatic: false);

		player.body = Globals.physicsWorld.CreateRectangle(width: 300, height: 100, density: 1, bodyType: BodyType.Dynamic);
	}
}
```

Custom Component Example:
```csharp
public class Movement : Component
{
	private float speed = 10;

	public override void Update()
	{
		var movement = new Vector2(Globals.input.horizontal, Globals.input.vertical) * speed;
		body.LinearVelocity += movement;
	}
}
```

## Roadmap

* Lights
* 2D Optimizations (Improve FrustumCulling)
* Editor Undo & Redo
* Editor Handles
* Editor Automatic Tilemap Painting
