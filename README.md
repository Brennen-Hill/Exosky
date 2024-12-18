# Exosky, an Exoplanet Constellation Creator and Explorer
## Final Project
https://shoebills-in-space.web.app/

## Honorable Mention by NASA
Our project has been awarded an Honorable Mention at NASA Space Apps!

Out of 93,520 participants and 15,444 teams worldwide, only 19 projects earned this recognition from NASA Space Apps’ global judges for their ingenuity and creativity. We’re thrilled that our project stood out in such an incredible pool of talent!

## Winner at Space Apps Chicago Hackathon
The NASA Space Apps Hackathon is the largest global hackathon in the world. We designed our solution in the Chicago location, the largest location in the United States this year. We were one of three groups to win at NASA Space Apps Chicago. We were noted for having the best project presentation. Now we are waiting to see what happens at the global level.

## High-Level Summary
We developed an interactive 3D starfield based on 5500 exoplanets and half a million stars from NASA and ESA databases. A user can draw constellations and travel to real-life exoplanets to see how the stars — and their constellations — change with perspective. The user can also take pictures of the stars as viewed from an exoplanet as well as any constellations they drew.

## Project Details
Our project is an interactive 3D starfield. The user can travel between exoplanets, navigate around each exoplanet, draw constellations, and take screenshots of their constellations. The user can draw a constellation from the perspective of one exoplanet, travel to a different one, and see how the constellation has changed!
To build it, we first obtained star and exoplanet data from the NASA and ESA websites linked below. We used Google Colab and Python to transform the star data into cartesian coordinates, luminosity, and RGB star colors based on temperature. We cleaned the exoplanet data using similar methods as the stars. Some libraries used include Astropy, Skyfield, Pandas, and NumPy. 

We imported the data into Unity to generate stars and exoplanets with the right characteristics. We added functionality using C# scripts, so the user could draw and remove constellations, smoothly move between and traverse exoplanets, and take screenshots. Finally, we compiled and published the Unity project and hosted it using Google Firebase.
Even though our simulation is pretty, it is highly accurate as well. Star brightness is determined by probe measurements and star color is derived from measured temperature. Stars and planets are placed using their calculated position on January 1st, 2024. We made a few stylistic choices as well. Star and planet size is scaled to make the simulation more visibly appealing and easy to use, and out of the 2 billion stars in the Gaia archive, we only used the brightest half million to optimize performance.

## Space Agency Data
Gaia DR3 https://www.cosmos.esa.int/web/gaia/data-release-3 
NASA Exoplanet Archive https://exoplanetarchive.ipac.caltech.edu/ 

## Project Video
https://youtu.be/mWGtb8QGUds

## References
Unity https://unity.com/ 
Google Colab https://colab.research.google.com/ 
Google Firebase https://firebase.google.com/ 
Skyfield https://rhodesmill.org/skyfield/ 
Astropy https://www.astropy.org/ 
NumPy https://numpy.org/ 
Pandas https://pandas.pydata.org/

## Contributors
Brennen Hill, Jeremy Kintana, Max Maeder, Rahul Hathwar
