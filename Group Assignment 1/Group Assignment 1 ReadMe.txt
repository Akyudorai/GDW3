
INFR 3110U: Game Engine Design and Implementation
Group Assignment 1

Sea Drive
Mithunan Jayaseelan (100787442), Cody Jensen (100591285), Nathaniel Moore (100785826), Kainat Rashid (100752341), and Kyra Trinidad (100784182)

Repo: https://github.com/One-Man-Gambit/GDW3
Demo Video: https://youtu.be/lV-UKaVJNlA

Design Patterns Implemented
Singleton: game managers (time manager, game manager, UI manager, quest manager)
Observer: street lamps
State Machine: NPC states during quests
Factory: passerby NPCs
Command: cheat codes

DLLs Implemented
TestPlugin: generates tips in pause menu
TimeCycleDLL: handles day/night cycle

Game Controls
W - move forward
A - move left
S - move backward
D - move right
Space - jump
E - interact with NPC
Esc - open pause menu
` - toggle cheats
1 - sunrise (cheats must be on)
2 - noon (cheats must be on)
3 - sunset (cheats must be on)
4 - night (cheats must be on)
5 - add money (cheats must be on)
6 - remove money (cheats must be on)

Location of Files
Executable - Executable/Aligned Components folder
Game Scene - Sandbox/Assets/Action Blocks/A5 - Aligned Components/ folder 
Code - NPCs folder, Quest Systems folder, or in Scripts folder where the scene is located
TestPlugin - Sandbox/Assets/Action Blocks/Quest Systems/Quest_DLL_Test/ folder
TimeCycleDLL - Sandbox/Assets/Plugins/

Additional Notes
Our game is currently being constructed out of action blocks (small segments of gameplay to test mechanics) which is why there isn't necessarily a "complete" game scene.

