INFR 3110U: Game Engine Design and Implementation
Course Project - GDW Requirements

Sea Drive
Mithunan Jayaseelan (100787442), Cody Jensen (100591285), Nathaniel Moore (100785826), Kainat Rashid (100752341), and Kyra Trinidad (100784182)

Repo: https://github.com/One-Man-Gambit/GDW3
Demo Video: https://youtu.be/WaMCpzGeXBs

Design Patterns Implemented
Object Pooling: currently used for quest items
State: NPC states during quests

Management Systems Implemented
Asset Management System: used with object pooling for managing assets
Sound Management System: used for managing sounds
Save/Load Management System (uses C++ plugin): used for saving race data 
Quest Management System: used for fetch and race quests

C++ Plugins
Tip Generator Plugin: found in phone menu's tip app
Save/Load Plugin: used to record race times

Bonus: extra management system (quest manager)

Game Controls
W - move forward
A - move left
S - move backward
D - move right
Space - jump
E - interact with NPC (cubes), walls (to wallrun), rails (to rail grind), ziplines
Esc - open phone menu

Location of Files
Executable - /Game Engine Final/Executable
Game Scene - Sandbox/Assets/Scenes/Stages/City Block 
Performance Test Scene - Sandbox/Assets/Scenes/Performance Testings
Code - Sandbox/Assets/Scripts
Object Pooling Design Pattern and Asset Manager - Scripts/Aligned Components/Object Pooling
State Design Pattern and Quest System - Scripts/Aligned Components/Quest Systems -- State Systems
Quest Manager - Scripts/Managers
Sound Manager - Scripts/Managers
Save Manager - Scripts/Managers
Tip Generator - Scripts/UI
Plugins - Plugins/DLLs