# Paint Or Perish 
### Table of Contents
[Description](https://github.com/CK-28/PaintOrPerish/new/main?readme=1#description)

[How To Run](https://github.com/CK-28/PaintOrPerish/new/main?readme=1#how-to-run)

[Game Features](https://github.com/CK-28/PaintOrPerish/new/main?readme=1#game-features)

[Developer Roles](https://github.com/CK-28/PaintOrPerish/new/main?readme=1#developer-roles)

<img src="https://user-images.githubusercontent.com/59154699/235538800-d2db42f4-1b05-411b-bbbf-459ef794926e.png"  width="900" height="550">

## Description
Paint or Perish is a paintball simulator we created while taking a course on Game Programming at the University of Guelph. Gameplay consists of a team deathmatch where the player competes with and against teams of AI. This game mode is composed of two teams of five working towards winning the game by either reaching the highest score first or having the highest score when the timer runs out.

<img src="https://user-images.githubusercontent.com/59154699/235540665-c52cd51a-36e2-4b90-af87-379ca862ba3d.png"  width="450" height="300"> <img src="https://user-images.githubusercontent.com/59154699/235540984-fa83b812-054b-45b0-aba5-38f255e9e286.png"  width="450" height="300"> 

## How To Run
1. Download Unity version 2021.3.16f1
2. Clone the repository locally and checkout main branch
3. Import the project to Unity
4. Open the project
5. Open MainMenu scene (located at Assets/Scenes/MainMenu) and click play

## Game Features
### Level Design
Our map was built to resemble a typical outdoor paintball arena, filled with different types of cover players can use to their advantage. There are also a number of buildings around the map, to allow for fun and strategic gameplay, and spawn points at either end of the arena, where each team will start.
<img src="https://user-images.githubusercontent.com/59154699/235538138-8202b836-0da2-4ab5-aa39-a68688b8f3c5.png"  width="450" height="325"> <img src="https://user-images.githubusercontent.com/59154699/235538148-c647bacb-51b2-4d34-bfcb-f2dcd40672da.png"  width="450" height="325"> 
### Player Mechanics
The player is controllable using typical first person shooter mechanics. This includes using WASD to move, mouse to aim, and left-click to shoot. Additionally, players can run using left shift and crouch using left control. The player has exactly one health, because in paintball you are out as soon as you are hit, and will automatically return to their spawn point once dead, playing a walking, hand-raising animation, to mimic returning to your own side in paintball. When the player reaches their spawn point, they will become controllable again and can continue playing.
### AI Mechanics
There are two main categories of AI - enemy and teammate - which act pretty much the same except for their spawn location and who they shoot at. All AI then have one of two objectives (roles): defend or roam. Defending AI will randomly choose a location, from a predetermined list, to defend, meaning they go to the location and camp until they are killed. Roaming AI will choose one of three set paths to follow until they are killed. When any AI is hit, it will return to its spawn point, playing the walking, hand-raised animation. Once there, they will generate a new path or location and continue playing their objective. 

The AI all use raycasting to determine if an enemy can be seen. If there is an enemy in sight, but not within attack range, they will approach until in range. Once in range, they will begin shooting. The AI traverses the map using a navigation mesh.
### Assets and Animation
Most Game Objects are built using our very own assets. These assets have been created using Blender (version number here).

The player model consists of a full bodied human with a custom classic paintball gun attached to their hand. Animations have also been created for this model. Animations include walking, running, crouching, crouch walking, and raising of the arm to signal being hit.

The map is built by piecing together different building items such as walls, windows, doors, and stairs. We also created smaller objects to be used around the map for a variety of covers. These items are typical of an actual arena and include crates, tires and metal sheets.

All textures have also been custom made with the exception of the grass and the stone texture used in both towers.
### Other

## Developer Roles
 - Crestena Khidhir (Team lead, Level design)
 - Alanna Tees (Development lead, AI programmer)
 - Luca Alfieri (Animation and Assets lead, Additional programming)
