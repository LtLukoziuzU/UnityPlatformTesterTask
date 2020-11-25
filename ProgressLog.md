## Nov 19

* Started up the repository for the task
* Got Unity 2020.2b9 and VSCode
* Decided to go with Stardew Valley. Current plan:
  * Three levels/scenes - farm, house, shop
  * Farming. Plant a crop, water it every day, harvest after a few days getting the grown crop
  * In-game clock, limiting the day. Day cycle
  * Shop to sell crops and buy seeds (make like two or three different seeds to fiddle with different stages/values)
  * If time permits: Add a couple of NPCs to befriend. Fishing Minigame.
* Got my Android phone set for development (sadly, don't have an iOS device, so will only test the Editor script for that)

## Nov 21

* Continue setting up:
  * Get 2D RP asset for Graphics
  * Make a generic 'map square' prefab and fill out the map (8x8 map, still need to bound player character somehow)
  * Add a player character sprite
  * Set up UI canvas and camera to fit
  * Try setting up touch controls with a couple different methods from the google before deciding to just use New Input System
* Waste time getting hassled by 1293851 I encountered on Thursday. Finally get it workarounded after getting interrupted multiple times while window switching
* Spend about 30 minutes trying to import and examine New Input Systems' "Touch Samples" package "thanks" to it being a complete project and also requiring multiple package dependencies (Cinemachine, Probuilder)

## Nov 23

* Switch back to old Input as I was having issues and didn't want to waste time on learning new system on rather limited time
  * On hindsight, those issues most likely were unrelated to old/new Input (bad collider setup), so probably would have been okay if I realized that 30 minutes earlier and kept New Input System, but after realizing that, didn't want to make a switch again and locked the choice to stop wasting time.
* Got movement set up for both keyboard and touch
* Fiddled with map's colliders, finally realized I need to make them triggers to not block movement
* Made rudimentary Inventory and its controller. Current functionality: 
  * 6 slots
  * Shows which slot is currently selected
  * Stores info on what object each slot holds and how many of it 
    * While writing these notes, realized that I will need to somehow rethink how I visualize holding multiple items, as current system doesn't really accomodate that
* Nested Prefabs
* Made a MapController (for now just populates each Map Square info, will be necessary for growth cycling)
* Made the Player find both Controllers (so that they all can interact in script)
* Next up: Finally write logic for digging ground

## Nov 24

* Got the tools to work. Ground now can be tilled and watered
* Got the day cycle working too. Going to "Bed" (house) and pressing "Use" now passes to next day, causing ground to slowly revert back to grass
* Added some sprites, then realized, that maybe I shouldn't used copyrighted sprites. I am a terrible artist though, so seeds/plants' quality deteriorated severely.
* Fully did shop and its logic. Can now buy seeds and sell seeds/harvested plants, and Player cannot walk while in Shop
* Tomorrow finally time to finish this. Only two tasks left: get seeds to plant, grow and get harvested; and build script

## Nov 25

* Finished the game part - seeds can be planted, grown if watered and harvested once it hits the last stage
* Wrote up comments to the code (should have done it as I've wrote code but better later than never)
* Tested a bit more extensively on my personal phone but the only issue I found was that I didn't anchor one of UI elements (Count) correctly, so it was off-screen
* Wrote the build script. Left it last as I am already familiar with how it works and didn't have any trouble making it (the only real troubles were build time (the PC I have isn't the greatest thing) and not immediately realizing that GraphicsAPIs were in UnityEngine.Rendering namespace)
* Built all seven builds, now packing it up as a release here

# THE PROJECT IS PRESENTABLE, AND WILL BE SENT TO UNITY AT THIS POINT. HOWEVER, I WANT TO DO SOME IMPROVEMENTS TO IT OVER NEXT WEEK, SO THIS LOG AND UPDATES TO PROJECT WILL CONTINUE FOR A FEW MORE DAYS. 
