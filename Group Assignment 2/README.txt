INFR 3110U: Game Engine Design and Implementation
Group Assignment 2

Sea Drive
Mithunan Jayaseelan (100787442), Cody Jensen (100591285), Nathaniel Moore (100785826), Kainat Rashid (100752341), and Kyra Trinidad (100784182)

Repo: https://github.com/One-Man-Gambit/GDW3
Demo Video: https://www.youtube.com/watch?v=NQqIbjDccyw

Design Patterns Implemented
Object Pooling: currently used for quest items, but will have other future uses like waypoints
State: NPC states during quests

Management Systems Implemented
Quest Management System: used for fetch and race quests
Asset Management System: used with object pooling for managing assets
Sound Management System: used for managing sounds

Bonus: We did a flowchart for our state design pattern

Game Controls
W - move forward
A - move left
S - move backward
D - move right
Space - jump
E - interact with NPC (cubes), rails, ziplines
Esc - open phone menu
` - toggle cheats
1 - sunrise (cheats must be on)
2 - noon (cheats must be on)
3 - sunset (cheats must be on)
4 - night (cheats must be on)
5 - add money (cheats must be on)
6 - remove money (cheats must be on)

Location of Files
Executable - Group Assignment 2/Executable
Game Scene - Sandbox/Assets/Scenes/A6 - City Block 
Performance Test Scene - Sandbox/Assets/Scenes/Performance Testings
Code - Sandbox/Assets/Scripts
Object Pooling Design Pattern and Asset Manager - Scripts/Aligned Components/Object Pooling
State Design Pattern and Quest System - Scripts/Aligned Components/Quest Systems -- State Systems
Quest Manager - Scripts/Managers
Sound Manager - Scripts/Managers