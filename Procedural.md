Procedural Generation: The Cave Game (awesome name)

## Concept

Het concept is als volgt: Je komt als speler steeds in een nieuwe (random gegenereerde) grot terecht. Het is aan jou de taak om alle vijanden te doden en vervolgens de uitgang te vinden. Als je de uitgang gevonden hebt, ga je door naar het volgende level. Om je te helpen het level te overwinnen, heb je de beschikking over een jetpack. Deze jetpack kun je echter maar beperkt gebruiken. Zodra je weer op de grond bent, wordt de jetpack weer bijgevuld.


## Gameplay/Interaction

Als speler kun je met de WASD toetsen rond bewegen door het level. Met de spatiebalk kun je springen. Als je de spatiebalk ingedrukt houdt kun je je jetpack gebruiken. Dit zorgt ervoor dat je een beperkte tijd kan vliegen. Vijanden kun je doden door op ze te springen. 


## Gebruikte algoritmen + Bronnen

### Cellular Automata
Er zijn twee algoritmen gebruikt voor het genereren van het level. Het eerste algoritme is Cellular automata. Dit algoritme is voornamelijk bekend van Conway's Game of Life. Cellular Automata komt neer op een grid van cellen. Elke cel heeft twee states: Aan of uit. Vervolgens wordt er een set van regels gemaakt. Aan de hand van die regels wordt de relevante cel aan of uit gezet. Dit kunnen regels zijn als: Als er twee of drie cellen direct rondom de relevante cel aan zijn, is de relevante cel zelf ook aan (Conway's Game of Life). Of: Als een relevante cel 4 of meer cellen om zich heen zijn die aan zijn, gaat de relevante cel uit. 
Zo ontstaat een organisch grid dat "leeft".

In deze game is Cellular Automata gebruikt om een grid te maken. Elke cel kan in dit grid maar één state hebben tegelijkertijd. De cel is "1" (aan) of "0" (uit). Als een cel 1 is, is het een muur. Als een cel 0 is, is het open ruimte (toegankelijk voor de speler). De cellen werden gegenereert met de volgende methode:

```
private void RandomFillMap() {
        if (useRandomSeed) {
            seed = DateTime.Now.Ticks.ToString();
        }
        System.Random prng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
                    map[x, y] = 1;
                } else {
                    map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }
```
Zoals er is te zien kan er met een gegeven seed gewerkt worden, alsook met een pseudorandom seed. Eerst wordt gekeken waar de buitenste randen zijn van het level. Dit moeten sowieso muren worden, dus die worden alvast een waarde van "1" meegegeven. Daarna worden alle waardes in de 2D array `map` gevuld met een random waarde. Dit kan nog worden beïnvloed met de `randomFillPercent` waarde. Hierdoor kun je bepalen hoe groot het percentage muur moet zijn in het level.

### Marching Squares
Het tweede algoritme wat gebruikt is, is Marching Squares. Marching Squares wordt voornamelijk gebruikt voor het maken van vormen of contouren. Het werkt als volgt: Gegeven een vierkant: Het algoritme probeert lijnen te trekken (lineare interpolatie) tussen de verschillende hoeken. Elke hoek heeft een bepaalde waarde, ook wel een bit waarde genoemd. Vervolgens wordt er een gemiddelde (mid point) bepaald tussen de punten en vanuit daar worden dan de lijnen getrokken.

Dit ziet er dan als volgt uit:

![alt tag](http://jamie-wong.com/images/14-08-11/marching-squares-mapping.png)
(Bron: http://jamie-wong.com/2014/08/19/metaballs-and-marching-squares/)

Zoals op dit plaatje is te zien zijn er 16 (0 meegeteld) verschillende combinaties die met één vierkant gemaakt kunnen worden. Elke combinatie staat voor een andere vorm.

In deze game wordt Marching Squares gebruikt voor het genereren van de vormen van de grot. Dit word gedaan via bovenstaande theorie. Dit algoritme gaat hand in hand met het Cellular Automata algoritme. Het marching squres algoritme gebruikt de cellen die Cellular Automata genereerde en "verkleint" deze cellen zodat elke cel een hoekpunt wordt in een nieuw vierkant. Deze hoekpunten worden `ControlNodes` genoemd. Elke `Control Node` kan aan of uit staan. Aan de hand daarvan (zie bovenstaand plaatje) kunnen Triangles worden gemaakt. Met die triangles worden dan meshes gemaakt. Elk gemaakt square kan 16 verschillende vormen hebben. Vervolgens worden alle squares aan elkaar "geplakt" en wordt er één grote mesh gemaakt.

De bronnen die ik heb gebruikt om dit te kunnen realiseren zijn tutorials afkomstig van Sebastian Lague (YOUTUBE). Zijn tutorials bevatten niet alleen het gebruik van de algoritmes, maar er wordt ook de theorie achter de algoritmes uitgelegd.

## TODO

Zoals jullie kunnen merken is de game verre van af. Door tijd gebrek ben ik er niet aan toegekomen om een 100% goed spelende game af te leveren met leuke gameplay.

Ik had graag nog een duidelijk doel willen neerzetten voor de speler (zoals: Verzamel alle objecten) zodat het daadwerkelijk een game werd. Daarnaast is het enemy systeem nog niet helemaal werken. Het enemy systeem werkt met een Finite State Machine en heeft drie states.

- Idle
  In Idle loopt de enemy te patrouilleren op een bepaald vlak. De vijand werkt met en Field Of View waardoor de vijand daadwerkelijk kan "zien". Als de enemy de speler ziet wisselt hij naar de Attack state.

- Attack
In deze state had ik graag gewild dat de vijand zijn zicht verkleinde en ging focusen op de speler en vervolgens de speler ging volgen. Als de vijand dan dichtbij genoeg is wordt de speler aangevallen. Dit werkt dan een beetje zoals de radar functionaliteit à RoboCode. helaas ben ik hier niet aan toegekomen. 

Zodra de speler het lukt om uit het gezichtsveld van de vijand te ontsnappen raakt de gaat de vijand naar de Search state.

- Search
In deze state gaat de vijand toe naar de laatst bekende locatie van de speler. Eenmaal aangekomen op die locatie gaat de vijand "random" zoeken naar naar de speler door te gaan bewegen naar random uitgekozen punten. Ook hier ben ik helaas niet aan toegekomen om af te maken.

Doordat ik een universele 2D Actor controller wilde maken die zowel de speler als de vijand kon gebruiken kwam ik tijd te kort om een AI te maken die zelf de input kon verzinnen op basis van zijn omstandigheden. Hierdoor heb ik op een soort "Hack" manier geprobeert om de vijand bepaalde acties te laten doen op basis van van bv. huidige velocity en richting.

Dit wil ik in de toekomst graag anders maken want ik ben van mening dat ik hiermee een erg sterke 2D platformer AI kan maken die zich aanpast aan elk level. Zodat er op uitzondering van een paar specifieke regels kant en klare AI gebruikt kan worden.
