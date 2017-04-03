Boids: The Game

## Concept

Het concept bestaat uit een groep vogeltjes die door gaten moeten vliegen om te blijven leven. Als ze uit het speelveld worden geduwd gaan de vogeltjes dood. Als er geen vogeltjes meer in leven zijn is het spel afgelopen.

## Gameplay/Interaction

De vogeltjes reageren op de muis. [NA EENMAAL KLIKKEN] gaan ze naar jouw muispositie toe. Op die manier moet jij proberen de vogeltjes door het speelveld heen te loodsen. Hoe langer je het volhoudt hoe hoger de score wordt. Uiteindelijk is het de bedoeling om de hoogste score te behalen.

## Gebruikte algoritmen + Bronnen

### Uitleg
Voor deze game is het Boids/Flocking algoritme gebruikt. Het is afgeleid van de pseudocode op de website http://www.kfish.org/boids/pseudocode.html en door mijzelf geïmplementeerd in Unity. 

Het algoritme bestaat uit een minimale set van drie regels. Deze drie regels zijn nodig voor de minimale werking van de boids.

De eerste regel zorgt ervoor dat alle boids naar een center of mass gaan. De center of mass is de gemiddelde positie van alle actieve boids. Daardoor gaan alle boids naar elkaar toe.

De tweede regel zorgt ervoor dat alle boids elkaar niet aanraken of ook wel ontwijken. Ook wel The Seperation Rule genoemd. Met deze regel wordt er gekeken of er een ander object (of boid) binnen een bepaalde afstand is, daar wordt vervolgens een afstand gecreëerd. In samenwerking met de eerste regel creëer je hier al flocking gedrag mee. 

De derde regel zorgt ervoor dat de groep boids een richting krijgen. Dit is belangrijk omdat het anders maar een wolkje van boids blijft die verder niets doet. Deze regel berekent de gemiddelde richting en snelheid van de groep boids en geeft dat gemiddelde terug om ze een algemene richting te geven.

### Gebruik
In de game heb ik gebruik gemaakt van de drie bovenstaande regels. Vervolgens heb ik nog een paar andere regels toegevoegd. De eerste regel die ik extra heb toegevoegd zorgt ervoor dat de boids het scherm niet uit kunnen. 
De tweede extra regel houdt in dat de boids naar de muispositie toegaan in plaats van regelnummer drie.

## Misc
Het level wordt een soort van gegenereerd. De balken met gaten die naar beneden komen worden aan de hand van een formule gemaakt. Het gat wat tussen de balken zit, de snelheid waarmee ze naar beneden komen en hoeveel tijd er tussen de balken zit kan dynamisch worden ingesteld.

## TODO

Ik had graag de art nog willen vervangen maar hier ben ik niet meer aan toegekomen.
