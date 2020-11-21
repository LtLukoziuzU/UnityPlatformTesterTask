* One-time issues
  * On first load while creating a new project, I got a dialogue asking to either Load Default Layout or to Reset them completely. Neither option worked though, returning the same dialogue. After closing and reopening the project, Unity opened up as expected without this dialogue.
    * Update: Looks like it's related to the 1293851 issue, and reoccurs on some reloads
<br /> <br />
* Reproducible bugs
  * Two editor windows load when launching project every time - one proper, and one broken rendering only white/black space or corrupted view of proper window - https://i.imgur.com/SNbmLKd.png . Closing either window closes Unity fully. Tried looking for a duplicate in Issue Tracker, but didn't find any, so reported a bug ( 1293851 ).
    * After some annoyances looks like I managed to make the issue disappear after completely nuking the layouts section and making a new one
<br /> <br />
* Annoyances
  * New Input System's "Touch Samples" package has dependancy on Cinemachine and Probuilder, making it annoying to grab it when I only want to just look up how the touch controls are used there, requiring me to import multiple packages I won't be using for my own project
