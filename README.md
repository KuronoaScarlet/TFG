# Stats Balancer
Estás desarrollando un nuevo RPG y no sabes cómo balancear a los enemigos? El balance de las curvas de dificultad y estadísticas de los personajes en cualquier género de videojuegos no es una tarea fácil, toma ingentes cantidades de tiempo a los diseñadores y largos procesos de playtesting para obtener los resultados deseados durante el desarrollo.
Sin embargo, gracias a esta herramienta, ahorrarás bastante tiempo! Con tan solo introducir las estadísticas base de tu personaje principal, seleccionar el prototipo de enemigo y modificar la curva de dificultad a las que las estadísticas deben ajustarse, obtendrás los stats de las entidades generadas completamente balanceadas.

## Features principales
- Creación de estadísticas y balanceo en base a las estadísticas base del jugador y la modificación de una curva de dificultad.
- Curva de dificultad basada en la curva de Bezier
- Algoritmo de balanceo mediante simulaciones de batalla.
- Creación y balanceo de varios enemigos.
- Exportación de las estadísticas generadas en formato .csv.
## Instrucciones
#### Pantalla principal
- Introducir las estadísticas base del personaje principal en las cajas blancas bajo "Player Initial Stats".
- Seleccionar un arquetipo de enemigo con el menú desplegable bajo "Enemy Type".
- Modificar la curva de dificultad que las estadísticas deben seguir al momento de generarse a través de los círculos negros presentes en el gráfico.
- Para generar los datos, pulsar el botón "Generate".

#### Pantalla de estadísticas
- Para generar nuevos enemigos, pulsar en la flecha que apunta hacia la derecha al lado del nombre del enemigo generado. A partir de ahí, seleccionar el tipo de enemigo a generar a través del menú desplegable, modificar la curva de dificultad como se ha mencionado antes, y pulsar "Generate".
- Para fijar las estadísticas del jugador al generar nuevos datos, pulsar en el toggle "Lock Player Stats". Esto hará que al generar nuevos enemigos, se generen a partir de las estadísticas ya existentes del jugador.
- Para exportar los datos creados, se debe pulsar el botón "Export". Este habilitará un botón llamado "Open Folder" que abrirá la carpeta en la que se encuentran los datos generados.
## Acerca del creador
Buenas! Soy Ignasi Pardo y desde siempre me han gustado los RPG. Soy un estudiante experimentado en el ámbito de la programación de algoritmos y gameplay en Unity, y también destaco en el campo del análisis de datos.
Esta herramienta ha sido creada como propuesta para el Trabajo de Final de Grado de la carrera de Diseño y Desarrollo de Videojuegos, realizada en la universidad CITM. Si tienes cualquier duda, o quieres contactar conmigo, no dudes en mandarme un correo a ignasi.pardo.carbo@gmail.com.
## Descarga la [última versión](https://github.com/KuronoaScarlet/TFG/releases/tag/v1.0)!
## Licencia
MIT License

Copyright (c) 2023 Ignasi Pardo Carbó

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
