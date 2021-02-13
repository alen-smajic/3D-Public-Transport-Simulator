# 3D Public Transport Simulator, Goethe University Frankfurt (Fall 2019) Bachelor Thesis

## General Information
<img align="right" width="300" height="" src="https://upload.wikimedia.org/wikipedia/commons/1/1e/Logo-Goethe-University-Frankfurt-am-Main.svg">

**Supervisor:**
* [Prof. Dr. Alexander Mehler](https://www.texttechnologylab.org/team/alexander-mehler/), email: mehler@em.uni-frankfurt.de
* [Giuseppe Abrami](https://www.texttechnologylab.org/team/giuseppe-abrami/), email: abrami@em.uni-frankfurt.de

**Institutions:**
  * **[Goethe University](http://www.informatik.uni-frankfurt.de/index.php/en/)**
  * **[TTLab - Text Technology Lab](https://www.texttechnologylab.org/)**
  
**Bachelor Thesis Topic:**

(German): Entwicklung und Erprobung eines interaktiven 3D - Stadtmodells am Beispiel des Personennahverkehrsnetzwerks der Stadt Frankfurt

(English): Development and testing of an interactive 3D city model using the example of the local public transport network of the city of Frankfurt
  
## Project Description ##
The aim of this project is the development of a public transport simulation framework which can be used as a backbone for the development of more complex simulations and applications. It consists of a 3D city model, a graphical representation of the local public transport and its network connections and an interactive feature which lets you browse all possible network connections and generates transport vehicles, that move across the stations. This all is incorporated in a realistic geographical environment, which is true to scale. To achieve this, the framework uses data from the [OpenStreetMap project](https://www.openstreetmap.org) as well as the [Mapbox SDK for Unity](https://www.mapbox.com/unity).  

[OpenStreetMap (OSM)](https://www.openstreetmap.org) is a collaborative project to create a free editable map of the world. The geodata underlying the map is considered the primary output of the project. The creation and growth of OSM has been motivated by restrictions on use or availability of map data across much of the world, and the advent of inexpensive portable satellite navigation devices. The provided data can be extracted in the form of an [OSM XML](https://wiki.openstreetmap.org/wiki/OSM_XML) file for further use. 

The simulation framework uses an OSM XML parser algorithm to extract the necessary information from the [OSM XML](https://wiki.openstreetmap.org/wiki/OSM_XML) file. The backbone for this algorithm was developed by Sloan Kelly in his work [Real World Map Data](https://github.com/codehoose/real-world-map-data). The data is then being processed and stored for the scene generation. In the last step the simulation environment is built using the pre stored data and the [Mapbox SDK for Unity](https://www.mapbox.com/unity). 

You can find a detailed description on the software architecture within the code comments and the written thesis. Furthermore, there is an instruction menu within the simulation which showes how to get the data to start the simulation.

## Publications ##
  * **[ResearchGate](https://www.researchgate.net/publication/344784145_Entwicklung_und_Erprobung_eines_interaktiven_3D_-_Stadtmodells_am_Beispiel_des_Personennahverkehrsnetzwerks_der_Stadt_Frankfurt)**
  * **[YouTube Video](https://www.youtube.com/watch?v=etmjra_CXOc)**
  
## Tools ## 
* Unity
* C#
* OSM XML
* Blender
* Mapbox SDK for Unity

## Results ##
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Cities/New%20York.png">
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Public%20Transport%20Network/Station%20UI.gif">
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Vehicles/Transport%20Vehicle.gif">
<img align="left" width="390" height="" src="Images%20of%20the%20simulation/Controls/Instructions%202.png">
<img align="right" width="390" height="" src="Images%20of%20the%20simulation/Cities/New%20York%202.png">
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Public%20Transport%20Networks/New%20York.png">
<img align="left" width="390" height="" src="Images%20of%20the%20simulation/Public%20Transport%20Networks/London.png">
<img align="right" width="390" height="" src="Images%20of%20the%20simulation/Vehicles/Bus.png">
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Vehicles/Train.png">
<img align="left" width="390" height="" src="Images%20of%20the%20simulation/Vehicles/Subway.png">
<img align="right" width="390" height="" src="Images%20of%20the%20simulation/Vehicles/Tram.png">
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Vehicles/Vehicles.png">
<img align="center" width="1000" height="" src="Images%20of%20the%20simulation/Public%20Transport%20Networks/Paris%202.png">
<img align="left" width="390" height="" src="Images%20of%20the%20simulation/Menu/Main%20Menu.png">
<img align="right" width="390" height="" src="Images%20of%20the%20simulation/Menu/Data%20Source%20Menu.png">
<img align="left" width="390" height="" src="Images%20of%20the%20simulation/Menu/Options%20Menu.png">
<img align="right" width="390" height="" src="Images%20of%20the%20simulation/Menu/Credits%20Menu.png">
